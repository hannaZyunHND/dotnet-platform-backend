using HtmlAgilityPack;
using MI.Bo.Bussiness;
using MI.Dapper.Data.Repositories.Interfaces;
using MI.Entity.Common;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Utils;
using PlatformCMS.ViewModel;
using MI.Entity.CustomClass;
using Microsoft.AspNetCore.Hosting;
using MI.Dal.IDbContext;
using MI.Entity.Models;
using Microsoft.CodeAnalysis.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Wordprocessing;
using MoreLinq.Extensions;
using NPOI.SS.Formula.Functions;
namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : ControllerBase
    {
        [HttpPost]
        [Route("CreateSurvey")]
        public async Task<IActionResult> CreateSurvey([FromBody] SurveyCreateDto request)
        {
            using (var _context = new IDbContext())
            {
                if (request != null)
                {
                    var survey = new Surveys()
                    {
                        Title = request.Title,
                        Description = request.Description,
                        Created_At = DateTime.Now,
                        Updated_At = DateTime.Now,
                    };

                    await _context.Surveys.AddAsync(survey);

                    // Tạo danh sách câu hỏi
                    var questions = new List<Questions>();
                    foreach (var question in request.Questions)
                    {
                        var newQuestion = new Questions()
                        {
                            Survey_Id = survey.Survey_Id,
                            Text = question.Text,
                            Question_Type = question.QuestionType,
                            Order = question.Order,
                        };

                        questions.Add(newQuestion);

                        var options = new List<Options>();
                        foreach (var option in question.Options)
                        {
                            options.Add(new Options()
                            {
                                Question_Id = newQuestion.Question_Id,
                                Text = option.Text,
                                Value = option.Value
                            });
                        }

                        // Thêm câu hỏi và các lựa chọn vào ngữ cảnh
                        await _context.Questions.AddAsync(newQuestion);
                        await _context.Options.AddRangeAsync(options);
                    }

                    // Lưu tất cả các thay đổi
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Khảo sát đã được tạo thành công." });
                }

                return BadRequest("Yêu cầu không hợp lệ.");
            }

        }

        [HttpPost]
        [Route("Get_Survey_Form")]
        public async Task<IActionResult> Get_Survey_Form([FromBody] SurveyResponseDto request)
        {
            try
            {
                using (var _context = new IDbContext())
                {
                    if (request != null)
                    {
                        var response = new Responses
                        {
                            Survey_Id = request.SurveyId,
                            Email = request.Email,
                        };

                        await _context.Responses.AddAsync(response);
                        await _context.SaveChangesAsync();

                        var answers = new List<Answers>();
                        foreach (var answer in request.Answers)
                        {
                            var answerEntity = new Answers
                            {
                                Response_Id = response.Response_Id,
                                Question_Id = answer.QuestionId,
                                Option_Id = answer.OptionId,
                                Text_Answer = answer.TextAnswer
                            };

                            answers.Add(answerEntity);
                        }

                        await _context.Answers.AddRangeAsync(answers);
                        await _context.SaveChangesAsync();
                        return Ok(answers);
                    }

                    return BadRequest("Invalid request");
                }
            }
            catch (Exception ex)
            {
                // Trả về thông tin chi tiết về lỗi
                return StatusCode(500, new { Msg = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }



        [HttpPost]
        [Route("Get_All_Survey")]
        public async Task<IActionResult> Get_All_Survey([FromBody] Pagination request)
        {

            if (request == null)
            {
                request = new Pagination { page_size = 10, page_index = 1 };
            }

            using (var _context = new IDbContext())
            {
                List<Survey_Data_Output> surveyResults = new List<Survey_Data_Output>();

                List<Surveys> surveys = await _context.Surveys.ToListAsync();

                foreach (var survey in surveys)
                {
                    var responses = await _context.Responses
                                                  .Where(r => r.Survey_Id == survey.Survey_Id)
                                                  .ToListAsync();

                    foreach (var response in responses)
                    {
                        var answers = await _context.Answers
                                                     .Where(a => a.Response_Id == response.Response_Id)
                                                     .ToListAsync();

                        int totalValue = answers
                                        .Join(_context.Options,
                                              answer => answer.Option_Id,
                                              option => option.Option_Id,
                                              (answer, option) => option.Value)
                                        .Sum();

                        var surveyDataOutput = new Survey_Data_Output()
                        {
                            Answer_Id = response.Response_Id,
                            Survey_Name = survey.Title,
                            Email = response.Email,
                            Time_To_Submit = response.Submitted_At,
                            Total_Responses = totalValue
                        };

                        surveyResults.Add(surveyDataOutput);
                    }
                }

                var total = surveyResults.Count();
                var pageResults = surveyResults
                                   .Skip((request.page_index - 1) * request.page_size)
                                   .Take(request.page_size)
                                   .ToList();

                var result = new
                {
                    TotalCount = total,
                    PageIndex = request.page_index,
                    PageSize = request.page_size,
                    Results = pageResults
                };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("Get_Survey_By_Id/{response_Id}")]
        public async Task<IActionResult> Get_Survey_By_Id(int response_Id)
        {
            using (var _context = new IDbContext())
            {
                if (response_Id == 0)
                {
                    return BadRequest("Invalid response ID.");
                }

                // Find the response entry
                var response = await _context.Responses.FirstOrDefaultAsync(r => r.Response_Id == response_Id);

                if (response == null)
                {
                    return NotFound("Response not found.");
                }

                // Prepare a list to store detailed answers for each question
                List<QuestionResponseDetail> questionDetails = new List<QuestionResponseDetail>();

                // Fetch all answers associated with this response
                var answers = await _context.Answers
                                            .Where(a => a.Response_Id == response_Id)
                                            .ToListAsync();

                // Process each answer
                foreach (var answer in answers)
                {
                    var question = _context.Questions.Find(answer.Question_Id);
                    var option = _context.Options.Find(answer.Option_Id);
                    questionDetails.Add(new QuestionResponseDetail
                    {

                        QuestionId = question.Question_Id,
                        QuestionText = question.Text,
                        SelectedOption = option.Text,  // This will be null if the answer was a text input
                        Value = option.Value,
                    });
                }

                // Create the result object with survey and user response details
                var result = new
                {
                    Responses = questionDetails
                };

                return Ok(result);
            }
        }

        [HttpGet]
        [Route("Get_All_Options/{question_Id}")]
        public async Task<IActionResult> Get_All_Options(int question_Id)
        {
            using (var _context = new IDbContext()) // Sử dụng lớp thực thi
            {
                if (question_Id != 0) // Kiểm tra giá trị khác 0
                {
                    var options = await _context.Options
                        .Where(r => r.Question_Id == question_Id)
                        .ToListAsync(); // Thực hiện truy vấn bất đồng bộ
                    return Ok(options);
                }

                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Question_By_Id/{Id}")]
        public async Task<IActionResult> Get_Question_By_Id(int Id)
        {
            using (var _context = new IDbContext())
            {
                if (Id != 0)
                {
                    var questions = await _context.Questions.FindAsync(Id);
                    return Ok(questions);
                }
                return BadRequest();
            }
        }


        [HttpDelete]
        [Route("Delete/{Id}")]
        public async Task<IActionResult> Delete_Survey_Item(int SurveyId)
        {
            if (SurveyId == null)
            {
                return BadRequest("Survey Id is not exist !!");
            }

            return Ok();
        }
    }
}
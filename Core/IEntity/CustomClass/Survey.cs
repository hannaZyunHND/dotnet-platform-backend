using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Models
{
    public class SurveyCreateDto
    {
        public string Title { get; set; } // Tiêu đề của khảo sát
        public string Description { get; set; } // Mô tả khảo sát
        public List<QuestionCreateDto> Questions { get; set; } = new List<QuestionCreateDto>(); // Danh sách câu hỏi
    }

    public class QuestionCreateDto
    {
        public string Text { get; set; } // Nội dung câu hỏi
        public string QuestionType { get; set; } // Kiểu câu hỏi
        public int Order { get; set; } // Thứ tự câu hỏi
        public List<OptionCreateDto> Options { get; set; } = new List<OptionCreateDto>(); // Danh sách lựa chọn
    }

    public class OptionCreateDto
    {
        public string Text { get; set; } // Nội dung lựa chọn
        public string Value { get; set; } // Giá trị của lựa chọn
    }

    public class SurveyResponseDto
    {
        public string Feedback { get; set; }
        public int SurveyId { get; set; } // ID của khảo sát
        public string Email { get; set; } // Email của người dùng (tuỳ chọn)
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>(); // Danh sách câu trả lời của người dùng
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; } // ID của câu hỏi
        public int? OptionId { get; set; }
        public string TextAnswer { get; set; } // Nội dung câu trả lời (cho câu hỏi tự luận)
    }

    public class Survey_Data_Output
    {
        public int Answer_Id { get; set; }
        public string Email { get; set; }
        public string Survey_Name { get; set; }
        public DateTime Time_To_Submit { get; set; }
        public int Total_Responses { get; set; }
    }

    public class Survey_Detail_Data
    {
        public int Survey_Id { get; set; }
        public string Survey_Name { get; set; }
        public string Survey_Description { get; set; }
        public DateTime Created_At { get; set; }
    }


    public class Subscriber_Data_Output
    {
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UnsubscribeAt { get; set; }
    }

    public class Pagination
    {
        public int page_size { get; set; } = 10;
        public int page_index { get; set; } = 1;
    }

    public class QuestionResponseDetail
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string SelectedOption { get; set; }
        public string Value { get; set; }
        public string AnswerFeedback { get; set; }
    }
    public class OptionOutput
    {
        public int Option_Id { get; set; }
        public string Text { get; set; }
    }

    public class QuestionOutput
    {
        public int Question_Id { get; set; }
        public string Text { get; set; }
        public List<OptionOutput> Options { get; set; }
    }

    public class SurveyOutput
    {
        public int Survey_Id { get; set; }
        public string Survey_Title { get; set; }
        public List<QuestionOutput> Questions { get; set; }
    }


}

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
    public class SubscriberController : ControllerBase
    {
        [Route("Get_All_Subscribers")]
        public async Task<IActionResult> Get_All_Subscribers([FromBody] Pagination request)
        {

            if (request == null)
            {
                request = new Pagination { page_size = 10, page_index = 1 };
            }

            using (var _context = new IDbContext())
            {
                List<Subscribers> subscribers = await _context.Subscribers.ToListAsync();

                var total = subscribers.Count();
                var pageResults = subscribers
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
    }
}

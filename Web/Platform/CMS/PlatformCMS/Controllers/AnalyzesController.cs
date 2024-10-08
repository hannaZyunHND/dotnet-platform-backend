using Dapper;
using MI.Dal.IDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AnalyzesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("AnalyzeFullProductDetail")]
        public async Task<IActionResult> AnalyzeFullProductDetail()
        {
            var productDetails = await GetAnalyzeFullProductDetail();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");
                worksheet.Cells[1, 1].Value = "ParentName";
                worksheet.Cells[1, 2].Value = "PackageName";
                worksheet.Cells[1, 3].Value = "ZoneNames";
                worksheet.Cells[1, 4].Value = "PriceEachNguoiLon";
                worksheet.Cells[1, 5].Value = "PriceEachTreEm";
                worksheet.Cells[1, 6].Value = "NetEachNguoiLon";
                worksheet.Cells[1, 7].Value = "NetEachTreEm";
                worksheet.Cells[1, 8].Value = "DestinationName";
                worksheet.Cells[1, 9].Value = "ServiceName";

                int row = 2;
                foreach (var detail in productDetails)
                {
                    worksheet.Cells[row, 1].Value = detail.ParentName;
                    worksheet.Cells[row, 2].Value = detail.PackageName;
                    worksheet.Cells[row, 3].Value = detail.ZoneNames;
                    worksheet.Cells[row, 4].Value = detail.PriceEachNguoiLon;
                    worksheet.Cells[row, 5].Value = detail.PriceEachTreEm;
                    worksheet.Cells[row, 6].Value = detail.NetEachNguoiLon;
                    worksheet.Cells[row, 7].Value = detail.NetEachTreEm;
                    worksheet.Cells[row, 8].Value = detail.DestinationName;
                    worksheet.Cells[row, 9].Value = detail.ServiceName;
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    var base64String = Convert.ToBase64String(stream.ToArray());
                    return Ok(base64String);
                }
            }
        }

        [HttpPost("AnalyzeProductByServiceAndDestination")]
        public async Task<IActionResult> AnalyzeProductByServiceAndDestination()
        {
            var dictService = new Dictionary<int, string>();
            var dictDestination = new Dictionary<int, string>();
            using (var context = new IDbContext())
            {
                var qService = from z in context.Zone
                               join zil in context.ZoneInLanguage on z.Id equals zil.ZoneId
                               where z.Type == 1 && z.ParentId > 0 && z.Status == 1 && zil.LanguageCode == "vi-VN"
                               select new KeyValuePair<int, string>(z.Id, zil.Name);

                dictService = qService.ToDictionary(x => x.Key, x => x.Value);

                var qDestination = from z in context.Zone
                                   join zil in context.ZoneInLanguage on z.Id equals zil.ZoneId
                                   where z.Type == 5 && z.ParentId > 0 && z.Status == 1 && zil.LanguageCode == "vi-VN"
                                   select new KeyValuePair<int, string>(z.Id, zil.Name);

                dictDestination = qDestination.ToDictionary(x => x.Key, x => x.Value);
            }

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");
                worksheet.Cells[1, 1].Value = "Service";
                worksheet.Cells[1, 1, 2, 1].Merge = true;
                worksheet.Cells[1, 1, 2, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[1, 1, 2, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                worksheet.Cells[1, 1, 2, 1].Style.Font.Bold = true;

                int startCol = 2;
                bool alternateYellow = true;
                foreach (var destination in dictDestination)
                {
                    worksheet.Cells[1, startCol].Value = destination.Value;
                    worksheet.Cells[1, startCol, 1, startCol + 2].Merge = true;
                    worksheet.Cells[1, startCol, 1, startCol + 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, startCol, 1, startCol + 2].Style.Font.Bold = true;

                    worksheet.Cells[2, startCol].Value = "Num";
                    worksheet.Cells[2, startCol + 1].Value = "Price";
                    worksheet.Cells[2, startCol + 2].Value = "Cost";

                    if (alternateYellow)
                    {
                        worksheet.Cells[1, startCol, 2, startCol + 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, startCol, 2, startCol + 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow);
                    }

                    alternateYellow = !alternateYellow;
                    startCol += 3;
                }

                int startRow = 3;
                foreach (var service in dictService)
                {
                    worksheet.Cells[startRow, 1].Value = service.Value;
                    worksheet.Cells[startRow, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                    int col = 2;
                    foreach (var destination in dictDestination)
                    {
                        var resItem = await GetAnalyzeProductByServiceAndDestinationItem(service.Key, destination.Key);
                        worksheet.Cells[startRow, col].Value = Math.Round((decimal)resItem.total.Value);
                        worksheet.Cells[startRow, col].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[startRow, col + 1].Value = Math.Round(resItem.aPrice.Value);
                        worksheet.Cells[startRow, col + 1].Style.Numberformat.Format = "#,##0";
                        worksheet.Cells[startRow, col + 2].Value = Math.Round(resItem.aCost.Value);
                        worksheet.Cells[startRow, col + 2].Style.Numberformat.Format = "#,##0";

                        col += 3;
                    }

                    startRow++;
                }

                worksheet.Cells[1, 1, startRow - 1, startCol - 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, startRow - 1, startCol - 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, startRow - 1, startCol - 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[1, 1, startRow - 1, startCol - 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells.AutoFitColumns();

                using (var stream = new MemoryStream())
                {
                    package.SaveAs(stream);
                    var base64String = Convert.ToBase64String(stream.ToArray());
                    return Ok(base64String);
                }
            }
        }

        private async Task<List<ResponseAnalyzeFullProductDetail>> GetAnalyzeFullProductDetail()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                return null;
            }

            using (var conn = new SqlConnection(connectionString))
            {
                var commandText = "usp_Tool_Export_Full_Product_Data";
                var result = await conn.QueryAsync<ResponseAnalyzeFullProductDetail>(commandText);
                return result.ToList();
            }
        }

        private async Task<ResponseAnalyzeProductByServiceAndDestinationItem> GetAnalyzeProductByServiceAndDestinationItem(int serviceId, int destinationId)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                return new ResponseAnalyzeProductByServiceAndDestinationItem
                {
                    total = 0,
                    aPrice = 0,
                    aCost = 0
                };
            }

            using (var conn = new SqlConnection(connectionString))
            {
                var commandText = "usp_Analyze_ProductPriceInServiceAndDestination";
                var parameters = new DynamicParameters();
                parameters.Add("@zoneService", serviceId);
                parameters.Add("@zoneDestination", destinationId);
                var result = await conn.QueryFirstOrDefaultAsync<ResponseAnalyzeProductByServiceAndDestinationItem>(commandText, parameters, commandType: System.Data.CommandType.StoredProcedure);
                return result ?? new ResponseAnalyzeProductByServiceAndDestinationItem
                {
                    total = 0,
                    aPrice = 0,
                    aCost = 0
                };
            }
        }
    }
    public class ResponseAnalyzeFullProductDetail
    {
        public string ParentName { get; set; }

        public string PackageName { get; set; }

        public string ZoneNames { get; set; }

        public decimal PriceEachNguoiLon { get; set; }

        public decimal PriceEachTreEm { get; set; }

        public decimal NetEachNguoiLon { get; set; }

        public decimal NetEachTreEm { get; set; }

        public string DestinationName { get; set; }

        public string ServiceName { get; set; }
    }
    public class ResponseAnalyzeProductByServiceAndDestinationItem
    {
        public int? total { get; set; }

        public decimal? aPrice { get; set; }

        public decimal? aCost { get; set; }
    }
}

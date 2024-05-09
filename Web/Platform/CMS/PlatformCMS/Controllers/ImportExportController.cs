using MI.Dal.IDbContext;
using MI.Dapper.Data.Repositories.Interfaces;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using PlatformCMS.ExcelHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportExportController : ControllerBase
    {
        private readonly IExportUtility _export;
        IProductRepository _productRepository;
        IHostingEnvironment _hostingEnvironment;
        public ImportExportController(IProductRepository productRepository, IHostingEnvironment hostingEnvironment, IExportUtility export)
        {

            _productRepository = productRepository;
            _hostingEnvironment = hostingEnvironment;
            _export = export;

        }
        [Route("ExportProductPriceLocation")]
        public IActionResult Test([FromBody] FilterProduct filter)
        {

            var x = _export.ExportProductPriceLocationExcel(filter);
            return Ok(x);
        }
        [Route("ExportSpectification")]
        public IActionResult TestMaintain([FromBody] FilterProduct filter)
        {

            var x = _export.ExportMaintainSpectificationInProductExcel(filter);
            return Ok(x);
        }
        [Route("ExportProductInBankInstallment")]
        [HttpPost]
        public IActionResult ExportProductInBankInstallment(int productId, int type)
        {
            if (productId > 0 && type > 0)
            {
                using (IDbContext _context = new IDbContext())
                {
                    var listBankInstallment = _context.BankInstallment.Where(r => r.Type == type).ToList();
                    var listProductInBankInstallment = _context.ProductInBankInstallment.Where(r => r.ProductId == productId).ToList();

                    try
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            foreach (var bank in listBankInstallment)
                            {
                                var sheetName = string.Format("{0} [{1}]", bank.Name, bank.Id);
                                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                                workSheet.TabColor = System.Drawing.Color.Black;
                                workSheet.DefaultRowHeight = 12;
                                workSheet.Row(1).Height = 20;
                                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Row(1).Style.Font.Bold = true;

                                //Header

                                workSheet.Cells[1, 1].Value = "Kỳ hạn";
                                workSheet.Cells[1, 2].Value = "Trả trước (Theo số tiền)";
                                workSheet.Cells[1, 3].Value = "Trả trước (Theo phần trăm)";
                                workSheet.Cells[1, 4].Value = "Lãi suất";
                                workSheet.Cells[1, 5].Value = "Phụ phí";

                                var productInBank = listProductInBankInstallment.Where(r => r.BankInstallmentId == bank.Id);
                                var start_row = 2;
                                foreach (var prod in productInBank)
                                {
                                    workSheet.Cells[start_row, 1].Value = prod.Period;
                                    workSheet.Cells[start_row, 2].Value = prod.PaymentFirst;
                                    workSheet.Cells[start_row, 3].Value = prod.PaymentFirstPercent;
                                    workSheet.Cells[start_row, 4].Value = prod.InterestPercent;
                                    workSheet.Cells[start_row, 5].Value = prod.OthersFee;
                                }
                            }
                        }
                        string excelName = "ExportProductId_" + productId + ".xlsx";
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return BadRequest();
        }


    }
}

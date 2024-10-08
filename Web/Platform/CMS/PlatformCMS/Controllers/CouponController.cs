using MI.Bo.Bussiness;
using MI.Dal.IDbContext;
using MI.Dapper.Data.Models;
using MI.Dapper.Data.Repositories.Interfaces;
using MI.Dapper.Data.ViewModels;
using MI.Entity.Common;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
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
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponRepository;

        public CouponInProductBCL couponInProductBCL;
        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            couponInProductBCL = new CouponInProductBCL();
        }


        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var responseData = new ResponseData();
            try
            {
                var ads = MI.Cache.RamCache.DicCoupon;
                return Ok(ads.Values.ToList());
            }
            catch (Exception ex)
            {
            }
            return Ok(new List<MI.Entity.Models.Coupon>());
        }

        [HttpGet("UnPublish")]
        public async Task<ResponseData> Unpublish(int id, int type)
        {
            var responseData = new ResponseData();
            try
            {
                var ads = await _couponRepository.Unpublish(id, type);
                if (ads)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        [HttpPost("Add")]
        public async Task<ResponseData> Add(CouponViewModel coupon)
        {
            var responseData = new ResponseData();
            responseData.Id = 0;
            try
            {
                MI.Cache.RamCache.LastestLoadCoupon = DateTime.MinValue;
                if (coupon.DiscountOption == 1 && int.Parse(coupon.ValueDiscount) > 30)
                {
                    responseData.Success = false;
                    responseData.Message = "Giảm giá tối đa 30%";
                }
                else
                {
                    var ads = await _couponRepository.Create(coupon);
                    if (ads.Key > 0)
                    {
                        responseData.Success = true;
                        responseData.Message = "Thành công";
                        responseData.Id = ads.Key;
                    }
                    else
                    {
                        responseData.Message = ads.Value;
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return responseData;
        }

        [HttpPut("Update")]
        public async Task<ResponseData> Update(CouponViewModel coupon)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                if (coupon.DiscountOption == 1 && int.Parse(coupon.ValueDiscount) > 30)
                {
                    responseData.Success = false;
                    responseData.Message = "Giảm giá tối đa 30%";
                }
                else
                {
                    MI.Cache.RamCache.LastestLoadCoupon = DateTime.MinValue;
                    var ads = await _couponRepository.Update(coupon);
                    if (ads.Key)
                    {
                        responseData.Success = true;
                        responseData.Message = "Thành công";
                    }
                    else
                    {
                        responseData.Message = ads.Value;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return responseData;
        }

        [HttpGet("GetAllPaging")]
        public async Task<IActionResult> Get([FromQuery] FilterQuery filter)
        {
            try
            {
                var data = await _couponRepository.GetAllPaging(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var ads = await _couponRepository.GetById(id);
                return Ok(ads);
            }
            catch (Exception ex)
            {
            }
            return Ok();
        }

        [HttpPost("ImportCouponInProducts")]

        public async Task<IActionResult> ImportCouponInProducts([FromForm] RequestImportCouponInProducts request)
        {
            try
            {
                // Kiểm tra file Excel đầu vào
                var excelFile = request.excelFile;
                if (excelFile == null || excelFile.Length == 0)
                {
                    return BadRequest("File Excel không hợp lệ.");
                }

                // Tạo danh sách lưu sản phẩm mã giảm giá từ file Excel
                var couponInProductList = new List<CouponInProduct>();

                using (var stream = new MemoryStream())
                {
                    // Copy nội dung của file Excel vào MemoryStream
                    await excelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        int row = 2; // Dòng đầu tiên thường là tiêu đề

                        // Duyệt qua từng dòng của Excel để lấy dữ liệu
                        while (row <= worksheet.Dimension.End.Row)
                        {
                            var productIdCell = worksheet.Cells[row, 1].Value;
                            var discountValueCell = worksheet.Cells[row, 4].Value;

                            if (productIdCell != null && discountValueCell != null)
                            {
                                // Kiểm tra và chuyển đổi giá trị của các cột
                                if (int.TryParse(productIdCell.ToString(), out int productId) &&
                                    decimal.TryParse(discountValueCell.ToString(), out decimal discountValue))
                                {
                                    // Tạo mới CouponInProduct và thêm vào danh sách
                                    var couponInProduct = new CouponInProduct
                                    {
                                        ProductId = productId,
                                        DiscountValue = discountValue,
                                        CouponId = request.couponId,
                                        DiscountOption = 1
                                    };
                                    couponInProductList.Add(couponInProduct);
                                }
                            }
                            row++;
                        }
                    }
                }

                using (var context = new IDbContext())
                {
                    // Tìm mã giảm giá hiện tại
                    var coupon = await context.Coupon.FindAsync(request.couponId);
                    if (coupon == null)
                    {
                        return BadRequest("Coupon không tồn tại.");
                    }

                    // Xóa các mã giảm giá cũ của sản phẩm (nếu có)
                    var oldValues = context.CouponInProduct.Where(c => c.CouponId == request.couponId);
                    context.CouponInProduct.RemoveRange(oldValues);

                    // Thêm các sản phẩm có mã giảm giá mới
                    context.CouponInProduct.AddRange(couponInProductList);
                    await context.SaveChangesAsync();
                }

                return Ok("Import thành công!");
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        [HttpGet("ExportCouponInProducts/{couponId}")]
        public async Task<IActionResult> ExportCouponInProducts(int couponId)
        {
            try
            {
                using (var context = new IDbContext())
                {
                    // Query all products, including the ones without a coupon, but apply couponId match if available
                    var query = (from p in context.Product
                                 where p.ParentId == 0 && p.Status == 1
                                 join cip in context.CouponInProduct
                                 on new { ProductId = p.Id, CouponId = couponId }
                                 equals new { ProductId = cip.ProductId, CouponId = cip.CouponId }
                                 into productCouponGroup
                                 from couponProduct in productCouponGroup.DefaultIfEmpty()

                                 select new
                                 {
                                     Id = p.Id,
                                     Code = p.Code,
                                     Name = p.Name,
                                     // If couponProduct is not null and couponId matches, set DiscountValue, otherwise set to 0
                                     DiscountValue = couponProduct != null
                                                     ? couponProduct.DiscountValue
                                                     : 0M
                                 }).ToList();


                    // Tạo file Excel
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Products");

                        // Thiết lập tiêu đề cho các cột
                        worksheet.Cells[1, 1].Value = "Product ID";
                        worksheet.Cells[1, 2].Value = "Code";
                        worksheet.Cells[1, 3].Value = "Name";
                        worksheet.Cells[1, 4].Value = "Discount Value";

                        // Định dạng tiêu đề
                        using (var range = worksheet.Cells[1, 1, 1, 4])
                        {
                            range.Style.Font.Bold = true;
                            range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        }

                        // Ghi dữ liệu sản phẩm vào file Excel
                        int row = 2;
                        foreach (var item in query)
                        {
                            worksheet.Cells[row, 1].Value = item.Id;
                            worksheet.Cells[row, 2].Value = item.Code;
                            worksheet.Cells[row, 3].Value = item.Name;
                            worksheet.Cells[row, 4].Value = item.DiscountValue;
                            row++;
                        }

                        // Tự động căn chỉnh độ rộng của các cột
                        worksheet.Cells.AutoFitColumns();

                        // Lấy dữ liệu Excel dưới dạng byte array
                        var excelBytes = package.GetAsByteArray();

                        // Chuyển đổi byte array thành chuỗi base64
                        var base64Excel = Convert.ToBase64String(excelBytes);

                        // Trả về chuỗi base64 của file Excel
                        return Ok(base64Excel);
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetCouponInProducts/{couponId}")]
        public async Task<IActionResult> GetCouponInProducts(int couponId)
        {
            try
            {
                using (var context = new IDbContext())
                {
                    // Query all products, including the ones without a coupon, but apply couponId match if available
                    var query = (from p in context.Product
                                 where p.ParentId == 0 && p.Status == 1
                                 join cip in context.CouponInProduct
                                 on new { ProductId = p.Id, CouponId = couponId }
                                 equals new { ProductId = cip.ProductId, CouponId = cip.CouponId }
                                 into productCouponGroup
                                 from couponProduct in productCouponGroup.DefaultIfEmpty()

                                 select new
                                 {
                                     Id = p.Id,
                                     Code = p.Code,
                                     Name = p.Name,
                                     // If couponProduct is not null and couponId matches, set DiscountValue, otherwise set to 0
                                     DiscountValue = couponProduct != null
                                                     ? couponProduct.DiscountValue
                                                     : 0M
                                 }).ToList();


                    // Trả về kết quả dưới dạng JSON hoặc danh sách các sản phẩm kèm giảm giá
                    return Ok(query);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
    public class RequestImportCouponInProducts
    {
        
        // Properties
        public int couponId { get; set; }
        public IFormFile excelFile { get; set; }
    }


}

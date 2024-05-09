using MI.Bo.Bussiness;
using MI.Entity.Common;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utils;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOldRenewalController : ControllerBase
    {
        ProductOldRenewalBCL _productOldRenewalBCL;
        public ProductOldRenewalController()
        {
            _productOldRenewalBCL = new ProductOldRenewalBCL();
        }
        [HttpPut("InsertOrUpdate")]
        public ResponseData InsertOrUpdate([FromBody] List<ProductOldRenewal> obj)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                bool rs = true;
                foreach (var item in obj)
                {
                    if (!rs)
                        break;
                    if (item.Id > 0)
                        rs = _productOldRenewalBCL.Update(item);
                    else
                        rs = _productOldRenewalBCL.Add(item);
                }
                if (rs)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = "Sản phẩm thu cũ đổi mới lỗi!";
                }
            }
            catch (Exception ex)
            {

            }
            return responseData;
        }
        [HttpGet("Delete")]
        public ResponseData Delete(int ID)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                var isAdd = _productOldRenewalBCL.Remove(ID);
                if (isAdd)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
            }
            catch (Exception ex)
            {
                responseData.Success = false;
                responseData.Message = ex.Message;
            }
            return responseData;
        }
        [HttpGet("Get")]
        public ResponseData Get(int Id)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                responseData.Data = _productOldRenewalBCL.FindAll(x => x.ProductId == Id); ;
            }
            catch (Exception ex)
            {

            }
            return responseData;
        }

        [HttpPost("GetAllOldRenewalCustomer")]
        public IActionResult GetAllOldRenewalCustomer([FromBody] Utils.FilterQuery filter)
        {
            ResponsePaging responseData = new ResponsePaging();
            var total = 0;
            var data = _productOldRenewalBCL.Find(filter, out total);
            responseData.ListData = data.ToList<object>();
            responseData.Total = total;
            return Ok(responseData);
        }

        [HttpPut("UpdateStatus")]
        public IActionResult UpdateStatus([FromBody] CustomerRequestProductOldRenewal obj)
        {
            dynamic response = new System.Dynamic.ExpandoObject();
            response.Success = false;
            response.Message = "Thất bại";
            var data = _productOldRenewalBCL.UpdateStatus(obj);
            if (data == true)
            {
                response.Success = true;
                response.Message = "Thành công";
            }
            return Ok(response);
        }
    }
}

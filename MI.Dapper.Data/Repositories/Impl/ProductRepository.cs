using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MI.Dal.IDbContext;
using MI.Dapper.Data.Models;
using MI.Dapper.Data.Repositories.Interfaces;
using MI.Dapper.Data.ViewModels;
using MI.Entity.ViewModel;
using Microsoft.Extensions.Configuration;

namespace MI.Dapper.Data.Repositories.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ExportExcelProductPriceInLocation> ExportExcel(string idList, int loc_id)
        {
            //convert
            if (idList.Length > 0 && loc_id > 0)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        var parameters = new DynamicParameters();
                        parameters.Add("@idList", idList);
                        parameters.Add("@loc_id", loc_id);
                        var result = connection.Query<ExportExcelProductPriceInLocation>("usp_CMS_ExportExcelProductPrice", parameters, null, true, null, CommandType.StoredProcedure);
                        return result.ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return null;
                    }
                }
            }
            return null;
        }

        public List<ExportExcelMaintainSpectificationInProduct> ExportExcelMaintainSpectification(string idList, int spec_id, string lang_code)
        {
            //usp_CMS_ExportExcelMaintainSpectificationInProduct
            if (idList.Length > 0 && spec_id > 0)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        var parameters = new DynamicParameters();
                        parameters.Add("@idList", idList);
                        parameters.Add("@spec_id", spec_id);
                        parameters.Add("@lang_code", lang_code);
                        var result = connection.Query<ExportExcelMaintainSpectificationInProduct>("usp_CMS_ExportExcelMaintainSpectificationInProduct", parameters, null, true, null, CommandType.StoredProcedure);
                        return result.ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return null;
                    }
                }
            }
            return null;
            //throw new NotImplementedException();
        }

        public async Task<List<ProductAtArticle>> GetAllProduct()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                var result = await conn.QueryAsync<ProductAtArticle>(
                    "select p.id,Title as name from product p inner join ProductInLanguage pil on p.id=pil.ProductId where pil.LanguageCode='vi-VN'",
                    null, null, null,
                    CommandType.Text);
                var listProduct = result.ToList();
                return listProduct;
            }
        }

        public string ImportExcelProductPriceInLocation(DataTable mergeProduct)
        {
            if (mergeProduct != null)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        var parameters = new DynamicParameters();
                        parameters.Add("@mergeTbl", mergeProduct.AsTableValuedParameter("dbo.type_ProductInLocation_v1"));
                        //parameters.Add("@loc_id", loc_id);
                        //var result = connection.Execute("usp_CMS_ImportExcelProductPriceInLocation", parameters, null, true, null, CommandType.StoredProcedure);
                        var result1 = connection.Execute("usp_CMS_ImportExcelProductPriceInLocation", parameters, null, null, CommandType.StoredProcedure);
                        return "success";
                        //return result.ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return e.Message;
                    }
                }
            }
            return null;
        }
        public string ImportExcelMaintainSpectificatinInProduct(DataTable mergeProduct)
        {
            if (mergeProduct != null)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        var parameters = new DynamicParameters();
                        parameters.Add("@mergeTbl", mergeProduct.AsTableValuedParameter("dbo.type_MaintainSpectificationInProduct_v1"));
                        parameters.Add("@lang_code", "vi-VN");
                        //parameters.Add("@loc_id", loc_id);    
                        //var result = connection.Execute("usp_CMS_ImportExcelProductPriceInLocation", parameters, null, true, null, CommandType.StoredProcedure);
                        var result1 = connection.Execute("usp_CMS_ImportExcelMaintainSpectificationInProduct", parameters, null, null, CommandType.StoredProcedure);
                        return "success";
                        //return result.ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return e.Message;
                    }
                }
            }
            return null;
        }

        //Add by AnhNV start
        public async Task<int> ProductAdd(Product product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@Status", product.Status);
                    parameters.Add("@Url", product.Url);
                    parameters.Add("@Avatar", product.Avatar);
                    parameters.Add("@AvatarArray", product.AvatarArray);
                    parameters.Add("@Price", product.Price);
                    parameters.Add("@DiscountPrice", product.DiscountPrice);
                    parameters.Add("@Warranty", product.Warranty);
                    parameters.Add("@ManufacturerId", product.ManufacturerId);
                    parameters.Add("@Code", product.Code);
                    parameters.Add("@CreatedBy", product.CreatedBy);
                    parameters.Add("@ModifyBy", product.ModifyBy);
                    parameters.Add("@Unit", product.Unit);
                    parameters.Add("@Quantity", product.Quantity);
                    parameters.Add("@PropertyId", product.PropertyId);
                    parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync("ProductAdd", parameters, null, null,
                        CommandType.StoredProcedure);
                    var newId = parameters.Get<int>("@id");
                    return newId;
                }
                catch (Exception e)
                {
                }
            }

            return 0;
        }

        public async Task<int> CloneAProductAsync(int productId, PhienBans phienBans, int isCloneLanguage = 1)
        {
            using(var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@idProduct", productId);
                    parameters.Add("@Id", phienBans.id);
                    parameters.Add("@ParentId", phienBans.parentId);
                    parameters.Add("@Name", phienBans.tenPhienBan);
                    parameters.Add("@Price", phienBans.giaPhienBan);
                    parameters.Add("@DiscountPrice", phienBans.giaGiam);
                    parameters.Add("@DiscountPercent", phienBans.percentGiaGiam);
                    parameters.Add("@GiaNguoiLon", phienBans.giaNguoiLon);
                    parameters.Add("@GiaTreEm", phienBans.giaTreEm);
                    parameters.Add("@GiaEmBe", phienBans.giaEmBe);
                    parameters.Add("@NgayBatDau", phienBans.NgayBatDau);
                    parameters.Add("@NgayKetThuc", phienBans.NgayKetThuc);
                    parameters.Add("@isCloneLanguage", isCloneLanguage);
                    
                    parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync("usp_CMS_CloneAProduct", parameters, null, null,
                        CommandType.StoredProcedure);
                    var newId = parameters.Get<dynamic>("@outId");
                    return newId;
                }
                catch (Exception e)
                {
                }

            }
            return 0;
        }

        public async Task<int> CloneAProductAsyncV2(int productId, SimTopUp topup, int isCloneLanguage = 1)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    
                    var parameters = new DynamicParameters();
                    parameters.Add("@idProduct", productId);
                    parameters.Add("@id", topup.id);
                    parameters.Add("@parentId", topup.parentId);
                    parameters.Add("@title", topup.title);
                    parameters.Add("@price", topup.price);
                    parameters.Add("@coverage", topup.coverage);
                    parameters.Add("@dataLimit", topup.dataLimit);
                    parameters.Add("@validity", topup.validaty);
                    parameters.Add("@smsNumber", topup.smsNumber);
                    parameters.Add("@phoneMinute", topup.phoneMinute);
                    parameters.Add("@randomString", GenerateRandomString());
                    parameters.Add("@isCloneLanguage", isCloneLanguage);

                    parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync("usp_CMS_CloneAProduct_v2", parameters, null, null,
                        CommandType.StoredProcedure);
                    var newId = parameters.Get<dynamic>("@outId");
                    return newId;
                }
                catch (Exception e)
                {
                }

            }
            return 0;
        }
        //usp_CMS_CloneAProduct_v2
        private string GenerateRandomString()
        {
            Random random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

        StringBuilder stringBuilder = new StringBuilder(16);

            for (int i = 0; i < 16; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
        public List<PhienBans> GetListPhienBanByParentId(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@idProduct", productId);
                    
                    var result = connection.Query<PhienBans>("usp_CMS_GetPhienBanByParentId", parameters, null, true, null, CommandType.StoredProcedure);
                    return result.ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            return null;
        }

        public List<SimTopUp> GetListTopUpByParentId(int productId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@idProduct", productId);

                    var result = connection.Query<SimTopUp>("usp_CMS_GetTopUpsByParentId", parameters, null, true, null, CommandType.StoredProcedure);
                    return result.ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            return null;
        }
        public async Task<int> InsertCustomerProductOldRenewal(CustomerProductOldRenewal product)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@productId", product.productId);
                    parameters.Add("@fullName", product.fullName);
                    parameters.Add("@phoneNumber", product.phoneNumber);
                    parameters.Add("@email", product.email);
                    parameters.Add("@locationId", product.locationId);
                    parameters.Add("@type", product.type);
                    parameters.Add("@outId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var result = await connection.ExecuteAsync("usp_Web_InsertToCustomerProductOldRenewal", parameters, null, null,
                        CommandType.StoredProcedure);
                    var newId = parameters.Get<dynamic>("@outId");
                    return newId;
                }
                catch (Exception e)
                {
                }

            }
            return 0;
        }

        public List<Entity.Models.ProductComponent> GetAllProductComponent(Utils.FilterQuery filter, out int total)
        {
            total = 0;
            try
            {
                using (IDbContext _context = new IDbContext())
                {
                    IQueryable<Entity.Models.ProductComponent> items = _context.ProductComponent;


                    if (!String.IsNullOrEmpty(filter.keyword))
                        items = items.Where(x => x.Name.Contains(filter.keyword));

                    total = items.Count();
                    var result = items.OrderByDescending(x => x.CreatedDate).Skip((filter.pageIndex - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {

            }
            return new List<Entity.Models.ProductComponent>();
        }


        public async Task<int> DeleteProductChild(string childProductId, int parentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@childProductId", childProductId);
                    parameters.Add("@parentId", parentId);

                    var result = await connection.ExecuteAsync("usp_CMS_DeleteProductChild", parameters, null, null,
                        CommandType.StoredProcedure);
                    return result;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 0;
                }
            }
            return 0;
        }

        public string ImportProductInBankInstallment(DataTable mergeProduct)
        {
            if (mergeProduct != null)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        var parameters = new DynamicParameters();
                        parameters.Add("@data_table", mergeProduct.AsTableValuedParameter("dbo.type_import_productInBankInstallment_v1"));
                        //parameters.Add("@loc_id", loc_id);    
                        //var result = connection.Execute("usp_CMS_ImportExcelProductPriceInLocation", parameters, null, true, null, CommandType.StoredProcedure);
                        var result1 = connection.Execute("usp_CMS_ImportProductInBankInstallment", parameters, null, null, CommandType.StoredProcedure);
                        return "success";
                        //return result.ToList();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        return e.Message;
                    }
                }
            }
            return null;
        }

        public List<Entity.Models.ProductSerialNumbers> GetListProductSerialNumbers(int productId)
        {

            using (var context = new IDbContext())
            {
                var result = context.ProductSerialNumbers.Where(r => r.ProductId == productId).ToList();
                return result;
            }
            return null;
        }

        public List<EsSearchItem> GetAllSearchItems()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //parameters.Add("@loc_id", loc_id);    
                //var result = connection.Execute("usp_CMS_ImportExcelProductPriceInLocation", parameters, null, true, null, CommandType.StoredProcedure);
                var result = connection.Query<EsSearchItem>("usp_Web_ElasticAll", commandType: CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        //Add by AnhNV end
    }
}



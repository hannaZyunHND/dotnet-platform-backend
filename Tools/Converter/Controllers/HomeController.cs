using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Converter.Models;
using Dapper;
using System.Data.SqlClient;

namespace Converter.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult InsertZoneTemplate([FromBody] List<InsertZone> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_InsertZoneTopTenOld";
        foreach (var item in inserts)
        {
            var parameters = new DynamicParameters();
            parameters.Add("term_id", item.term_id);
            parameters.Add("name", item.name);
            parameters.Add("parent", item.parent);
            parameters.Add("type", 2);
            using (var connection = new SqlConnection(conn))
            {
                connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        return Ok();
    }
    [HttpPost]
    public IActionResult InsertProductInZoneTemplate([FromBody] List<InsertProductInZone> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_InsertProductInZoneFromTopTenOld";
        foreach (var item in inserts)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("product_id", item.ID);
                parameters.Add("zone_id", item.zone_id);
                using (var connection = new SqlConnection(conn))
                {
                    connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    Console.WriteLine("Success with product: " + item.ID + " and Zone " + item.zone_id);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with product: " + item.ID + " and Zone " + item.zone_id);
            }
            
        }
        return Ok();
    }
    [HttpPost]
    public IActionResult InsertArticleInZoneTemplate([FromBody] List<InsertProductInZone> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_InsertArticleInZoneFromTopTenOld";
        foreach (var item in inserts)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("product_id", item.ID);
                parameters.Add("zone_id", item.zone_id);
                using (var connection = new SqlConnection(conn))
                {
                    connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
                    Console.WriteLine("Success with product: " + item.ID + " and Zone " + item.zone_id);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed with product: " + item.ID + " and Zone " + item.zone_id);
            }

        }
        return Ok();
    }
    [HttpPost]
    public IActionResult InsertProductTemplate([FromBody] List<InsertProduct> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_InsertProductFromTopTenOld";
        foreach (var item in inserts)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", item.ID);
                parameters.Add("post_title", item.post_title);
                parameters.Add("post_content", item.post_content);
                parameters.Add("mo_ta", item.mo_ta);
                parameters.Add("post_url", item.post_url);
                parameters.Add("avatar", item.avatar);
                parameters.Add("tour_price", item.tour_price);
                parameters.Add("ngay_dem", item.ngay_dem);
                var lich_Tours = new List<MetaItems>();
                if (!string.IsNullOrEmpty(item.chuong_trinh))
                {
                    var lich_tour = new MetaItems();
                    lich_tour.tieuDe = "Lịch trình Tour";
                    lich_tour.noiDung = item.chuong_trinh;
                    lich_Tours.Add(lich_tour);
                }
                parameters.Add("lich_tour", Newtonsoft.Json.JsonConvert.SerializeObject(lich_Tours));
                var thong_tin_Tours = new List<MetaItems>();
                if (!string.IsNullOrEmpty(item.mo_ta))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Mô tả";
                    part.noiDung = item.mo_ta;
                    thong_tin_Tours.Add(part);
                }
                if (!string.IsNullOrEmpty(item.bang_gia))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Bảng giá";
                    part.noiDung = item.bang_gia;
                    thong_tin_Tours.Add(part);
                }
                if (!string.IsNullOrEmpty(item.khoi_hanh))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Khởi hành";
                    part.noiDung = item.khoi_hanh;
                    thong_tin_Tours.Add(part);
                }
                if (!string.IsNullOrEmpty(item.bao_gom))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Bao gồm";
                    part.noiDung = item.bao_gom;
                    thong_tin_Tours.Add(part);
                }
                parameters.Add("thong_tin_tour", Newtonsoft.Json.JsonConvert.SerializeObject(thong_tin_Tours));
                var totalCount = 100;
                int.TryParse(item.view_count, out totalCount);
                parameters.Add("view_count", totalCount);
                using (var connection = new SqlConnection(conn))
                {
                    connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                Console.WriteLine("Insertd tour id = " + item.ID);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Failed tour id = " + item.ID);
            }
            
        }
        return Ok();
    }
    [HttpPost]
    [DisableRequestSizeLimit]
    public IActionResult InsertArticleTemplate([FromBody] List<InsertProduct> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_InsertArticleFromTopTenOld";
        foreach (var item in inserts)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("id", item.ID);
                parameters.Add("post_title", item.post_title);
                parameters.Add("post_content", item.post_content);
                parameters.Add("mo_ta", item.mo_ta);
                parameters.Add("post_url", item.post_url);
                parameters.Add("avatar", item.avatar);
                parameters.Add("tour_price", item.tour_price);
                parameters.Add("ngay_dem", item.ngay_dem);
                var lich_Tours = new List<MetaItems>();
                if (!string.IsNullOrEmpty(item.chuong_trinh))
                {
                    var lich_tour = new MetaItems();
                    lich_tour.tieuDe = "Lịch trình Tour";
                    lich_tour.noiDung = item.chuong_trinh;
                    lich_Tours.Add(lich_tour);
                }
                parameters.Add("lich_tour", Newtonsoft.Json.JsonConvert.SerializeObject(lich_Tours));
                var thong_tin_Tours = new List<MetaItems>();
                if (!string.IsNullOrEmpty(item.mo_ta))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Mô tả";
                    part.noiDung = item.mo_ta;
                    thong_tin_Tours.Add(part);
                }
                if (!string.IsNullOrEmpty(item.bang_gia))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Bảng giá";
                    part.noiDung = item.bang_gia;
                    thong_tin_Tours.Add(part);
                }
                if (!string.IsNullOrEmpty(item.khoi_hanh))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Khởi hành";
                    part.noiDung = item.khoi_hanh;
                    thong_tin_Tours.Add(part);
                }
                if (!string.IsNullOrEmpty(item.bao_gom))
                {
                    var part = new MetaItems();
                    part.tieuDe = "Bao gồm";
                    part.noiDung = item.bao_gom;
                    thong_tin_Tours.Add(part);
                }
                parameters.Add("thong_tin_tour", Newtonsoft.Json.JsonConvert.SerializeObject(thong_tin_Tours));
                var totalCount = 100;
                int.TryParse(item.view_count, out totalCount);
                parameters.Add("view_count", totalCount);
                using (var connection = new SqlConnection(conn))
                {
                    connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                Console.WriteLine("Insertd tour id = " + item.ID);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Failed tour id = " + item.ID);
            }

        }
        return Ok();
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public IActionResult MergeZoneSlug([FromBody] List<MergeZone> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_Merge_Slug_Zone";
        foreach (var item in inserts)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("term_id", item.term_id);
                parameters.Add("slug", item.slug);
                
                using (var connection = new SqlConnection(conn))
                {
                    connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                Console.WriteLine("Merged zone id = " + item.term_id);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Failed zone id = " + item.term_id);
            }

        }
        return Ok();
    }
    [HttpPost]
    [DisableRequestSizeLimit]
    public IActionResult MergeProductSlug([FromBody] List<MergeProduct> inserts)
    {
        var conn = "Server=123.25.15.166;Database=top10_final;Trusted_Connection=False;User Id=sa;password=midb@123";
        var stored = "usp_Tools_Merge_Slug_Product_Article";
        foreach (var item in inserts)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("term_id", item.Id);
                parameters.Add("slug", item.post_name);

                using (var connection = new SqlConnection(conn))
                {
                    connection.Execute(stored, parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
                Console.WriteLine("Merged product id = " + item.Id);
            }
            catch (Exception ex)
            {

                Console.WriteLine("Failed product id = " + item.Id);
            }

        }
        return Ok();
    }


}
public class MetaItems
{
    public string tieuDe { get; set; }
    public string noiDung { get; set; }
}
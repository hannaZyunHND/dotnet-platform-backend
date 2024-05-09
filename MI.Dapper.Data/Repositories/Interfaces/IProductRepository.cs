using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MI.Dapper.Data.Models;
using MI.Dapper.Data.ViewModels;
using MI.Entity.Models;
using MI.Entity.ViewModel;
using Product = MI.Dapper.Data.Models.Product;

namespace MI.Dapper.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductAtArticle>> GetAllProduct();
        Task<int> ProductAdd(Product product);

        List<ExportExcelProductPriceInLocation> ExportExcel(string idList, int loc_id);
        List<ExportExcelMaintainSpectificationInProduct> ExportExcelMaintainSpectification(string idList, int spec_id, string lang_code);
        string ImportExcelProductPriceInLocation(DataTable mergeProduct);
        string ImportExcelMaintainSpectificatinInProduct(DataTable mergeProduct);
        string ImportProductInBankInstallment(DataTable mergeProduct);
        Task<int> CloneAProductAsync(int productId, PhienBans phienBans, int isCloneLanguage = 1);
        Task<int> CloneAProductAsyncV2(int productId, SimTopUp topups, int isCloneLanguage = 1);
        List<SimTopUp> GetListTopUpByParentId(int productId);
        Task<int> DeleteProductChild(string childProductId, int ParentId);
        List<PhienBans> GetListPhienBanByParentId(int productId);
        Task<int> InsertCustomerProductOldRenewal(CustomerProductOldRenewal product);
        List<Entity.Models.ProductComponent> GetAllProductComponent(Utils.FilterQuery filter, out int total);

        List<ProductSerialNumbers> GetListProductSerialNumbers(int productId);

        List<EsSearchItem> GetAllSearchItems();

    }
}
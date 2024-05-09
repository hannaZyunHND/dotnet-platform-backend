using PlatformWEBAPI.Services.Extra.ViewModel;
using PlatformWEBAPI.Services.Product.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.ViewModel.Installment
{
    public class InstallmentProductDetailViewModel
    {
        public ProductDetail Infomation { get; set; } = new ProductDetail();
        public List<ProductSpectificationDetail> Spectification { get; set; } = new List<ProductSpectificationDetail>();
        public List<ProductMinify> SameZone { get; set; } = new List<ProductMinify>();
        public ProductPriceInLocationDetail DefaultLocationPrice { get; set; } = new ProductPriceInLocationDetail();
        public List<ProductPriceInLocationDetail> ListLocation { get; set; } = new List<ProductPriceInLocationDetail>();
        public List<ProductMinify> Combo { get; set; } = new List<ProductMinify>();
        public List<PromotionInProduct> Promotion { get; set; } = new List<PromotionInProduct>();
        public List<CommentDetail> Comments { get; set; } = new List<CommentDetail>();

        public int SameTotal { get; set; }
        public string DefaultLocation { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }

    }
}

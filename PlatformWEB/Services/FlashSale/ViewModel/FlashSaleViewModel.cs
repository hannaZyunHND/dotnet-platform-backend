﻿using PlatformWEB.Services.Product.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Services.FlashSale.ViewModel
{
    public class FlashSaleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string GiamGiaDen { get; set; }
    }

    public class ProductInFlashSale : ProductMinify
    {
        public decimal FlashSalePrice { get; set; }
        public int FlashSaleQuantity { get; set; }
        public int CountSellInFlashSale { get; set; }
        public int FlashSaleQuantityRemain { get; set; }
        public int IdProductFlashSale { get; set; }

    }
}

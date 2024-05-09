﻿using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.Product.Repository;
using PlatformWEB.Services.Product.ViewModel;
using PlatformWEB.Services.Zone.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Controllers
{
    public class EstimatesController : BaseController
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly IProductRepository _productRepository;
        private readonly IArticleRepository _articleRepository;

        public EstimatesController(IZoneRepository zoneRepository, IProductRepository productRepository, IArticleRepository articleRepository)
        {
            _zoneRepository = zoneRepository;
            _productRepository = productRepository;
            _articleRepository = articleRepository;
        }

        public IActionResult ContructionEstimates()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetSpectificationMenuByMaterialType(int material_type)
        {
            return ViewComponent("EstimatesByMaterialType", new { material_type = material_type });
        }
        public IActionResult GetProductInSpectificationSearch(FilterProductBySpectification fp)
        {
            return ViewComponent("ProductInEstimatesSearch", new { fp = fp });
        }
    }
}
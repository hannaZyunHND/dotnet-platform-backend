﻿using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PlatformWEB.Services.Locations.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Controllers
{
    public class LocationController : BaseController
    {
        //private readonly ILocationsRepository _locationsRepository;
        //private readonly IStringLocalizer<HomeController> _localizer;

        public IActionResult Index()
        {
            return View();
        }


    }
}
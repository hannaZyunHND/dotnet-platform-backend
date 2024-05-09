using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    public class P404Controller : BaseController
    {
        public IActionResult P404()
        {
            //return View(");
            return RedirectToAction("IndexPublic", "Home");
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using PlatformWEBAPI.Services.Locations.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEBAPI.Utility
{
    public interface ICookieUtility
    {
        CookieLocation SetCookieDefault();
    }
    public class CookieUtility : ICookieUtility
    {
        private readonly ILocationsRepository _locationsRepository;
        private readonly IActionContextAccessor _accessor;
        const string CookieLocationId = "_LocationId";
        const string CookieLocationName = "_LocationName";
        private IHttpContextAccessor _httpContextAccessor;

        private string _currentLanguage;
        private string _currentLanguageCode;
        public CookieUtility(ILocationsRepository locationsRepository, IHttpContextAccessor httpContextAccessor, IActionContextAccessor accessor)
        {
            _locationsRepository = locationsRepository;
            _httpContextAccessor = httpContextAccessor;
            _accessor = accessor;
        }
        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                    return _currentLanguage;

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguage = feature.RequestCulture.Culture.TwoLetterISOLanguageName.ToLower();
                }

                return _currentLanguage;
            }
        }
        private string CurrentLanguageCode
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguageCode))
                    return _currentLanguageCode;

                if (string.IsNullOrEmpty(_currentLanguageCode))
                {
                    var feature = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguageCode = feature.RequestCulture.Culture.ToString();
                }

                return _currentLanguageCode;
            }


        }

        public CookieLocation SetCookieDefault()
        {
            CookieOptions cookie = new CookieOptions()
            {
                Path = "/",
                HttpOnly = false,
                Secure = false,
                IsEssential = true,
                Expires = DateTime.Now.AddDays(7),
                SameSite = SameSiteMode.None
            };
            if (_httpContextAccessor.HttpContext.Request.Cookies[CookieLocationId] == null)
            {
                //Lay IP
                var location_default = _locationsRepository.GetLocationFirst(CurrentLanguageCode);
                if (location_default != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(7);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieLocationId, location_default.Id.ToString(), cookie);
                    _httpContextAccessor.HttpContext.Response.Cookies.Append(CookieLocationName, location_default.Name, cookie);
                    var result = new CookieLocation() { LocationId = location_default.Id, LocationName = location_default.Name };
                    return result;

                }
            }
            else
            {
                var result = new CookieLocation() { LocationId = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies[CookieLocationId]), LocationName = _httpContextAccessor.HttpContext.Request.Cookies[CookieLocationName] };
                return result;
            }
            return null;
        }
    }

    public class CookieLocation
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
}

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PlatformWEB.Services.Article.Repository;
using PlatformWEB.Services.BankInstallment.Repository;
using PlatformWEB.Services.Zone.Repository;
using PlatformWEB.ViewModel.Base;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PlatformWEB.Controllers
{
    public class InstallmentController : Controller
    {

        private readonly IZoneRepository _zoneRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IBankInstallmentRepository _bankRepository;
        const string CookieLocationId = "_LocationId";
        const string CookieLocationName = "_LocationName";
        private string _currentLanguage;
        private string _currentLanguageCode;

        public InstallmentController(IZoneRepository zoneRepository, IArticleRepository articleRepository, IBankInstallmentRepository bankRepository)
        {
            _zoneRepository = zoneRepository;
            _articleRepository = articleRepository;
            _bankRepository = bankRepository;

        }

        public IActionResult Index(string alias)
        {
            var zoneTar = _articleRepository.GetObjectByAlias(alias, CurrentLanguageCode);
            if (zoneTar != null)
            {
                ZoneViewModel model = new ZoneViewModel();
                model.ZoneId = zoneTar.ObjectId;
                model.Type = zoneTar.ObjectType;
                return View(model);
            }
            return View("~/Views/P404/P404.cshtml");

        }


        [HttpPost]
        public IActionResult GetPayOnData(object data)
        {
            string skey = "kYZRewgDo7gMfRr5BPLyYK";
            dynamic model = new System.Dynamic.ExpandoObject();
            model.app_id = "10000002750Uk93iL6U";
            model.data = EncryptAES256CBC("{}", skey);
            model.checksum = MD5Hash(model.app_id + model.data + skey);
            return Json(model);

        }

        private string CurrentLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentLanguage))
                    return _currentLanguage;

                if (string.IsNullOrEmpty(_currentLanguage))
                {
                    var feature = HttpContext.Features.Get<IRequestCultureFeature>();
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
                    IRequestCultureFeature feature = HttpContext.Features.Get<IRequestCultureFeature>();
                    _currentLanguageCode = feature.RequestCulture.Culture.ToString();
                }

                return _currentLanguageCode;
            }


        }

        static byte[] StringToByteArray(string str, int length)
        {
            return Encoding.ASCII.GetBytes(str.PadRight(length, ' '));
        }

        public static string EncryptAES256CBC(string plainText, string keyString)
        {
            byte[] cipherData;
            byte[] keyEncode = StringToByteArray(keyString, 32);
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = keyEncode;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            ICryptoTransform cipher = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cipher, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }

                cipherData = ms.ToArray();
            }

            byte[] combinedData = new byte[aes.IV.Length + cipherData.Length];
            Array.Copy(aes.IV, 0, combinedData, 0, aes.IV.Length);
            Array.Copy(cipherData, 0, combinedData, aes.IV.Length, cipherData.Length);
            return Convert.ToBase64String(combinedData);
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        [HttpGet]
        public IActionResult GetPeriods(int productId, int bankId)
        {
            var result = _bankRepository.GetBankPeriod(productId, bankId);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetBankPeriodDetail(int productId, int bankId, int period)
        {
            var result = _bankRepository.GetBankPeriodDetail(productId, bankId, period);
            return Ok(result);
        }
    }
}

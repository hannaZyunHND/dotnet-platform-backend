using HtmlAgilityPack;
using MI.Entity.Models;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace PlatformWEBAPI.Utility
{
    public class CMSHelper
    {
        public static IConfigurationRoot Configuration { get; set; }
        static CMSHelper()
        {
            Configuration = ConfigurationHelper.Init();

        }

        public static string Version()
        {

            try
            {

                return Configuration["AppSettings:Version"];
            }
            catch (Exception e)
            {

            }

            return "1.0.0";



        }
        public static string RandomCode(int size)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            var random = new Random();
            var result = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        
        public static string RenderLazyLoadBody(string body)
        {
            HtmlDocument doc = new HtmlDocument();
            if (!string.IsNullOrEmpty(body))
            {
                doc.LoadHtml(body);
                var imgs = doc.DocumentNode.SelectNodes("//img");
                if (imgs != null)
                {
                    foreach (var item in imgs)
                    {
                        if (item != null)
                        {

                            var origin = item.GetAttributeValue("src", null);
                            item.SetAttributeValue("src", "~/images/gray.jpg");

                            if (!string.IsNullOrEmpty(origin) && (origin.StartsWith("/uploads/") || origin.StartsWith("https://janhome.vn/wp-content")))
                            {
                                var domain_img_store = Configuration["AppSettings:FoderImg"];
                                origin = domain_img_store + origin.Replace("https://janhome.vn/wp-content", "");
                            }
                            item.SetAttributeValue("data-src", origin);
                            item.SetAttributeValue("srcset", origin);
                            item.AddClass("lazy");
                        }
                    }
                }
                var figures = doc.DocumentNode.SelectNodes("//figure");
                if (figures != null)
                {

                    foreach (var item in figures)
                    {
                        var _img = item.SelectSingleNode(".//img");
                        var _a = item.SelectSingleNode(".//a");
                        if (_img != null)
                        {
                            item.SetAttributeValue("style", "");
                            _img.AddClass("cust-ag");
                            //var url = _img.GetAttributeValue("data-src", null);


                        }
                        if (_a != null)
                        {
                            var url = _a.GetAttributeValue("href", null);
                            _a.SetAttributeValue("href", "javascript:void(0)");
                            _a.SetAttributeValue("data-url", url);
                        }

                    }

                }

                var uls = doc.DocumentNode.SelectNodes("//ul");
                if (uls != null)
                {
                    foreach (var item in uls)
                    {
                        item.AddClass("maintain-ul");

                        var ul_smaller = item.SelectNodes(".//ul");
                        if (ul_smaller != null)
                        {
                            foreach (var it in ul_smaller)
                            {
                                it.RemoveClass("maintain-ul");
                                it.AddClass("maintain-ul-smaller");
                            }
                        }
                    }
                }
                //var ul_parents = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'toc_list')]").FirstOrDefault();
                var p_link = doc.DocumentNode.SelectSingleNode("//p[contains(@class, 'toc_title')]");
                if (p_link != null)
                {
                    var text = p_link.InnerText;
                    p_link.RemoveAllChildren();
                    p_link.Name = "a";
                    p_link.SetAttributeValue("href", "javascript:void(0)");
                    p_link.SetAttributeValue("style", "color: inherit;");
                    p_link.InnerHtml = text;
                    HtmlDocument fa = new HtmlDocument();
                    fa.LoadHtml("<i class=\"fas fa-angle-down\"></i>");
                    var f = fa.DocumentNode.SelectSingleNode("//i");
                    if (f != null)
                    {
                        p_link.ChildNodes.Add(f);
                    }

                }
                return doc.DocumentNode.OuterHtml;
            }
            return "";


        }

        public static string IndexingCss(string body)
        {
            HtmlDocument doc = new HtmlDocument();
            if (!string.IsNullOrEmpty(body))
            {
                doc.LoadHtml(body);

                var ul_parents = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'toc_list')]").FirstOrDefault();
                if (ul_parents != null)
                {
                    var ul_childs = ul_parents.SelectNodes(".//ul");
                    if (ul_childs != null)
                    {
                        foreach (var item in ul_childs)
                            item.AddClass("pl-2");
                    }

                }

                var p_link = doc.DocumentNode.SelectSingleNode("//p");
                if (p_link != null)
                {
                    var text = p_link.InnerText;
                    p_link.RemoveAllChildren();
                    p_link.Name = "a";
                    p_link.SetAttributeValue("href", "javascript:void(0)");
                    p_link.InnerHtml = text;
                    HtmlDocument fa = new HtmlDocument();
                    fa.LoadHtml("<i class=\"fas fa-angle-down\"></i>");
                    var f = fa.DocumentNode.SelectSingleNode("//i");
                    if (f != null)
                    {
                        p_link.ChildNodes.Add(f);
                    }

                }
                return doc.DocumentNode.OuterHtml;
            }
            return "";
        }

        public static string GetFirstImage(string body)
        {
            HtmlDocument doc = new HtmlDocument();
            if (!string.IsNullOrEmpty(body))
            {
                doc.LoadHtml(body);

                var firstImg = doc.DocumentNode.SelectSingleNode("//img");
                if (firstImg != null)
                {
                    var r = firstImg.GetAttributeValue("src", "").ToString();
                    r = r.Replace("https://janhome.vn/wp-content/uploads", "");
                    return r;
                }
            }
            return "";
        }

        public static string EncryptAES256CBC(string plainText, string keyString)
        {
            byte[] cipherData;
            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(keyString);
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

        public static string DecryptAES256CBC(string combinedString, string keyString)
        {
            string plainText;
            byte[] combinedData = Convert.FromBase64String(combinedString);
            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(keyString);
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] cipherText = new byte[combinedData.Length - iv.Length];
            Array.Copy(combinedData, iv, iv.Length);
            Array.Copy(combinedData, iv.Length, cipherText, 0, cipherText.Length);
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform decipher = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, decipher, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        plainText = sr.ReadToEnd();
                    }
                }

                return plainText;
            }
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

        public static List<DetailBanerAds> ConvertSlide(BannerAdsViewModel _object)
        {
            var result = new List<DetailBanerAds>();
            if (_object != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<List<DetailBanerAds>>(_object.MetaData);
                }
                catch (Exception ex)
                {
                    var _r = JsonConvert.DeserializeObject<DetailBanerAds>(_object.MetaData);
                    result.Add(_r);

                }
                result = result.OrderBy(r => r.Order).ToList();
            }
            return result;
        }

        public static string GetCultureText(List<DetailBanerAds> _object, string code, bool isTrimDot = false)
        {
            var _r = "";
            var result = _object.Where(r => r.Title.Equals(code)).FirstOrDefault();
            if (result != null)
            {
                _r = result.Description;
            }
            else
            {
                _r = code;
            }
            _r = _r.Trim();
            if (_r.EndsWith(".") && isTrimDot)
            {
                _r = _r.TrimEnd('.');
            }
            return UIHelper.ClearHtmlTag(_r).Replace("&nbsp;", "").Trim();
        }
        public static string GetCultureHtml(List<DetailBanerAds> _object, string code)
        {
            var _r = "";
            var result = _object.Where(r => r.Title.Equals(code)).FirstOrDefault();
            if (result != null)
            {
                _r = _r.TrimStart();
                _r = result.Description;
            }
            else
            {
                _r = code;
            }
            return _r;
        }

        public static List<DetailBanerAds> ConvertSlideCulture(BannerAdsViewModel _object)
        {
            var result = new List<DetailBanerAds>();
            if (_object != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject<List<DetailBanerAds>>(_object.MetaData);

                }
                catch (Exception ex)
                {
                    var _r = JsonConvert.DeserializeObject<DetailBanerAds>(_object.MetaData);
                    result.Add(_r);

                }

            }
            return result;
        }

        


    }

    public class BannerAdsViewModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string MetaData { get; set; }
        public int Type { get; set; }
        public string LanguageCode { get; set; }
    }

    public class MultipleBannerAdsViewModal
    {
        public string bannerAdsCode { get; set; }
        public List<DetailBanerAds> details { get; set; }
    }

    public class RequestBannerAdsByCodes
    {
        public List<string> codes { get; set; }
        public string cultureCode { get; set; }
    }
}

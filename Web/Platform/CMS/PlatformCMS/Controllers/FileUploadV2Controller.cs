using LazZiya.ImageResize;
using MI.Bo.Bussiness;
using MI.Dal.IDbContext;
using MI.Entity.Common;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using OfficeOpenXml.Style;
using PlatformCMS.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Utils;
using Image = System.Drawing.Image;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadV2Controller : ControllerBase
    {
        FileUploadBCL _fileUploadBCL;
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;
        private readonly IExportUtility _workingFile;
        IHostingEnvironment _hostingEnvironment;

        public FileUploadV2Controller(IHostingEnvironment env, IConfiguration iConfig, IExportUtility workingFile, IHostingEnvironment hostingEnvironment)
        {
            _env = env;
            _config = iConfig;
            _fileUploadBCL = new FileUploadBCL();
            _workingFile = workingFile;
            _hostingEnvironment = hostingEnvironment;
        }

        [Route("UploadImage")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(List<IFormFile> files)
        {

            string serverPath = _config.GetValue<string>("AppSettings:UploadFolder");
            string folder = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            var res = new ResponseData();
            res.Success = false;
            try
            {

                var webRoot = _env.WebRootPath;
                var fileDirectory = Path.Combine(serverPath, folder);
                var pathToSave = Path.Combine(webRoot, fileDirectory);

                if (Request.Form.Files.Count > 0)
                {
                    var uploadResults = new List<FileInfo>();
                    string imageAllowUpload = _config.GetValue<string>("AppSettings:ImageAllowUpload");
                    string documentAllowUpload = _config.GetValue<string>("AppSettings:DocumentAllowUpload");
                    bool fileUploadSubFix = _config.GetValue<bool>("AppSettings:FileUploadSubFix");
                    int fileUploadMaxSize = _config.GetValue<int>("AppSettings:FileUploadMaxSize");
                    int imageScaleWidth = _config.GetValue<int>("AppSettings:ImageScaleWidth");
                    int imageScaleHeight = _config.GetValue<int>("AppSettings:ImageScaleHeight");
                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(fileDirectory + "\\" + file.FileName);

                        var imageAllowFileArray = imageAllowUpload.Split(',');
                        var documentAllowFileArray = documentAllowUpload.Split(',');
                        if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1 || Array.IndexOf(documentAllowFileArray, ext.ToLower()) != -1)
                        {
                            var fileName = Path.GetFileName(fileDirectory + "\\" + file.FileName);
                            if (fileUploadMaxSize >= (file.Length / 1024))
                            {

                                if (fileUploadSubFix)
                                {
                                    string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                                    fileName = Utility.UnicodeToKoDauAndGach(filenameNoExtension) + "-" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ext;
                                }
                                var fullPath = Path.Combine(pathToSave, fileName);
                                var webpPath = Path.Combine(pathToSave, Path.GetFileNameWithoutExtension(fileName) + ".webp");
                                if (!Directory.Exists(pathToSave))
                                {
                                    Directory.CreateDirectory(pathToSave);
                                }

                                if (!_fileUploadBCL.ExistFile(fileName))
                                {
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);

                                        // Convert and save the WebP version
                                        using (var originalStream = file.OpenReadStream())
                                        using (var skiaImage = SKBitmap.Decode(originalStream))
                                        {
                                            using (var webpImage = SKImage.FromBitmap(skiaImage))
                                            using (var data = webpImage.Encode(SKEncodedImageFormat.Webp, 80)) // Adjust quality as needed
                                            {
                                                using (var webpStream = new FileStream(webpPath, FileMode.Create, FileAccess.Write))
                                                {
                                                    data.SaveTo(webpStream);
                                                }
                                            }
                                        }

                                        

                                        uploadResults.Add(new FileInfo
                                        {
                                            name = fileName,
                                            path = "/" + Path.Combine(folder).Replace("\\", "/") + "/" + Path.GetFileNameWithoutExtension(fileName) + ".webp",
                                            ext = ext,
                                            size = file.Length / 1024,
                                            code = 200,
                                            messages = "Upload thành công"
                                        });


                                        var obj = new FileUpload();
                                        obj.Name = fileName;
                                        obj.FilePath = "/" + Path.Combine(folder).Replace("\\", "/") + "/" + Path.GetFileNameWithoutExtension(fileName) + ".webp";
                                        obj.FileExt = ext;
                                        //   obj.Dimensions = dimensions;
                                        obj.FileSize = file.Length / 1024;
                                        obj.Status = 1;
                                        obj.Type = 2;

                                        ResponseData responseData = new ResponseData();
                                        try
                                        {
                                            bool result = _fileUploadBCL.Add(obj);
                                            if (result)
                                            {

                                                responseData.Success = true;
                                                responseData.Message = "Thành công";
                                                FileInfo fileInfo = new FileInfo
                                                {
                                                    name = fileName,
                                                    path = "/" + Path.Combine(folder).Replace("\\", "/") + "/" + Path.GetFileNameWithoutExtension(fileName) + ".webp",
                                                    ext = ext,
                                                    size = file.Length / 1024,//Math.Round((double)file.Length / 1024, 1),
                                                    code = 200,
                                                    messages = "Upload thành công ne"

                                                };
                                                uploadResults.Add(fileInfo);
                                                // Inside your existing function
                                                if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1)
                                                {
                                                    using (var _stream = file.OpenReadStream())
                                                    {
                                                        // Decode the original image using SkiaSharp
                                                        using (var skiaImage = SKBitmap.Decode(_stream))
                                                        {
                                                            // Get the original dimensions and save them to the object
                                                            var dimensions = $"{skiaImage.Width}x{skiaImage.Height}";
                                                            obj.Dimensions = dimensions;
                                                            _fileUploadBCL.Update(obj);

                                                            // Create the thumbnail save path
                                                            var thumbPathToSave = Path.Combine(webRoot, serverPath, "thumb", folder);
                                                            if (!Directory.Exists(thumbPathToSave))
                                                            {
                                                                Directory.CreateDirectory(thumbPathToSave);
                                                            }

                                                            var thumbFileDirectory = Path.Combine(folder);
                                                            var thumbPath = Path.Combine(thumbPathToSave, Path.GetFileNameWithoutExtension(fileName) + ".webp");

                                                            // Calculate new dimensions for the thumbnail (one-third of original)
                                                            int newWidth = skiaImage.Width / 3;
                                                            int newHeight = skiaImage.Height / 3;

                                                            // Resize the image
                                                            using (var resizedBitmap = skiaImage.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
                                                            using (var webpImage = SKImage.FromBitmap(resizedBitmap))
                                                            {
                                                                // Save the thumbnail as a WebP file
                                                                using (var data = webpImage.Encode(SKEncodedImageFormat.Webp, 80)) // 80 is the quality level; adjust if needed
                                                                using (var thumbStream = new FileStream(thumbPath, FileMode.Create, FileAccess.Write))
                                                                {
                                                                    data.SaveTo(thumbStream);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            FileInfo fileInfo = new FileInfo
                                            {
                                                name = Path.GetFileNameWithoutExtension(fileName) + ".webp",
                                                path = "/",
                                                ext = ext,
                                                size = file.Length / 1024, //Math.Round((double)file.Length / 1024, 1),
                                                code = 500,
                                                messages = ex.Message

                                            };
                                            uploadResults.Add(fileInfo);
                                        }


                                    }
                                    write_watermark_text(fullPath);


                                    write_watermark_text(fullPath, false);

                                }
                                else
                                {
                                    FileInfo fileInfo = new FileInfo
                                    {
                                        name = fileName,
                                        path = "/",
                                        ext = ext,
                                        size = file.Length / 1024,
                                        code = 900,
                                        messages = "File đã tồn tại trên hệ thống"

                                    };
                                    uploadResults.Add(fileInfo);
                                }
                            }
                            else
                            {
                                FileInfo fileInfo = new FileInfo
                                {
                                    name = fileName,
                                    path = "/",
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 900,
                                    messages = "Limited file size"

                                };
                                uploadResults.Add(fileInfo);
                            }

                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo
                            {
                                name = file.FileName,
                                path = "/",
                                ext = ext,
                                size = file.Length / 1024,
                                code = 900,
                                messages = "File extension not allowed"

                            };
                            uploadResults.Add(fileInfo);

                        }
                    }
                    res.Data = uploadResults;
                    res.Success = true;
                }
                else
                {
                    res.ErrorCode = -1;
                    res.Success = false;
                    res.Message = "Hãy chọn một file.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                // Define the log file path
                string logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "error_log.txt");

                // Ensure the directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

                // Compose the log message
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error uploading file: {ex.Message}\n{ex.StackTrace}\n";

                // Write the log message to the file
                System.IO.File.AppendAllText(logFilePath, logMessage);

                // Set response data
                res.ErrorCode = -2;
                res.Success = false;
                res.Message = "Lỗi hệ thống, Vui lòng liên hệ quản trị.";
                return Ok(res);
            }


        }
        [Route("UploadImageV2")]

        public IActionResult UploadV2(List<IFormFile> files)
        {
            string serverPath = _config.GetValue<string>("AppSettings:UploadFolder");
            string folder = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            var res = new ResponseData { Success = false };
            try
            {
                var webRoot = _env.WebRootPath;
                var fileDirectory = Path.Combine(serverPath, folder);
                var pathToSave = Path.Combine(webRoot, fileDirectory);

                if (Request.Form.Files.Count > 0)
                {
                    var uploadResults = new List<FileInfo>();
                    string imageAllowUpload = _config.GetValue<string>("AppSettings:ImageAllowUpload");
                    bool fileUploadSubFix = _config.GetValue<bool>("AppSettings:FileUploadSubFix");
                    int fileUploadMaxSize = _config.GetValue<int>("AppSettings:FileUploadMaxSize");
                    int imageScaleWidth = _config.GetValue<int>("AppSettings:ImageScaleWidth");

                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(file.FileName).ToLower();
                        var allowedExtensions = imageAllowUpload.Split(',');

                        if (Array.IndexOf(allowedExtensions, ext) != -1 && file.Length / 1024 <= fileUploadMaxSize)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileUploadSubFix)
                            {
                                fileName = $"{Path.GetFileNameWithoutExtension(fileName)}-{DateTime.Now:HHmmssfff}{ext}";
                            }

                            var fullPath = Path.Combine(pathToSave, fileName);
                            var webpPath = Path.Combine(pathToSave, Path.GetFileNameWithoutExtension(fileName) + ".webp");

                            if (!Directory.Exists(pathToSave))
                            {
                                Directory.CreateDirectory(pathToSave);
                            }

                            if (!_fileUploadBCL.ExistFile(fileName))
                            {
                                // Save original image
                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);
                                }

                                // Convert and save the WebP version
                                using (var originalStream = file.OpenReadStream())
                                using (var skiaImage = SKBitmap.Decode(originalStream))
                                {
                                    using (var webpImage = SKImage.FromBitmap(skiaImage))
                                    using (var data = webpImage.Encode(SKEncodedImageFormat.Webp, 80)) // Adjust quality as needed
                                    {
                                        using (var webpStream = new FileStream(webpPath, FileMode.Create, FileAccess.Write))
                                        {
                                            data.SaveTo(webpStream);
                                        }
                                    }
                                }

                                // Create and save the thumbnail in WebP format
                                var thumbPath = Path.Combine(webRoot, serverPath, "thumb", folder, Path.GetFileNameWithoutExtension(fileName) + ".webp");
                                var thumbDirectory = Path.GetDirectoryName(thumbPath);

                                if (!Directory.Exists(thumbDirectory))
                                {
                                    Directory.CreateDirectory(thumbDirectory);
                                }

                                using (var originalStream = file.OpenReadStream())
                                using (var skiaImage = SKBitmap.Decode(originalStream))
                                {
                                    int newWidth = skiaImage.Width / 3;
                                    int newHeight = skiaImage.Height / 3;

                                    using (var resizedBitmap = skiaImage.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High))
                                    using (var webpThumbnail = SKImage.FromBitmap(resizedBitmap))
                                    using (var data = webpThumbnail.Encode(SKEncodedImageFormat.Webp, 80)) // Adjust quality as needed
                                    {
                                        using (var thumbStream = new FileStream(thumbPath, FileMode.Create, FileAccess.Write))
                                        {
                                            data.SaveTo(thumbStream);
                                        }
                                    }
                                }

                                uploadResults.Add(new FileInfo
                                {
                                    name = fileName,
                                    path = "/" + Path.Combine(folder).Replace("\\", "/") + "/" + fileName,
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 200,
                                    messages = "Upload thành công"
                                });
                            }
                            else
                            {
                                uploadResults.Add(new FileInfo
                                {
                                    name = fileName,
                                    path = "/",
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 900,
                                    messages = "File đã tồn tại trên hệ thống"
                                });
                            }
                        }
                        else
                        {
                            uploadResults.Add(new FileInfo
                            {
                                name = file.FileName,
                                path = "/",
                                ext = ext,
                                size = file.Length / 1024,
                                code = 900,
                                messages = "File extension not allowed or exceeds size limit"
                            });
                        }
                    }
                    res.Data = uploadResults;
                    res.Success = true;
                }
                else
                {
                    res.ErrorCode = -1;
                    res.Success = false;
                    res.Message = "Hãy chọn một file.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                res.ErrorCode = -2;
                res.Success = false;
                res.Message = "Lỗi hệ thống, Vui lòng liên hệ quản trị.";
                return Ok(res);
            }
        }




        [Route("UploadExcel")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadExcel(List<IFormFile> files)
        {
            string serverPath = _config.GetValue<string>("AppSettings:UploadFolder");
            string folder = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            var res = new ResponseData();
            try
            {

                var webRoot = _env.WebRootPath;
                var fileDirectory = Path.Combine(serverPath, folder);
                var pathToSave = Path.Combine(webRoot, fileDirectory);

                if (Request.Form.Files.Count > 0)
                {
                    var uploadResults = new List<FileInfo>();
                    string imageAllowUpload = _config.GetValue<string>("AppSettings:ImageAllowUpload");
                    string documentAllowUpload = _config.GetValue<string>("AppSettings:DocumentAllowUpload");
                    bool fileUploadSubFix = _config.GetValue<bool>("AppSettings:FileUploadSubFix");
                    int fileUploadMaxSize = _config.GetValue<int>("AppSettings:FileUploadMaxSize");
                    int imageScaleWidth = _config.GetValue<int>("AppSettings:ImageScaleWidth");
                    int imageScaleHeight = _config.GetValue<int>("AppSettings:ImageScaleHeight");
                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(fileDirectory + "\\" + file.FileName);

                        var imageAllowFileArray = imageAllowUpload.Split(',');
                        var documentAllowFileArray = documentAllowUpload.Split(',');
                        if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1 || Array.IndexOf(documentAllowFileArray, ext.ToLower()) != -1)
                        {
                            var fileName = Path.GetFileName(fileDirectory + "\\" + file.FileName);
                            if (fileUploadMaxSize >= (file.Length / 1024))
                            {

                                if (fileUploadSubFix)
                                {
                                    string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                                    fileName = Utility.UnicodeToKoDauAndGach(filenameNoExtension) + "-" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ext;
                                }
                                var fullPath = Path.Combine(pathToSave, fileName);
                                if (!Directory.Exists(pathToSave))
                                {
                                    Directory.CreateDirectory(pathToSave);
                                }

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);

                                }
                                //Doc file

                                var r = _workingFile.ImportProductPriceLocationExcel(fullPath);
                            }
                            else
                            {
                                FileInfo fileInfo = new FileInfo
                                {
                                    name = fileName,
                                    path = "/",
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 900,
                                    messages = "Limited file size"

                                };
                                uploadResults.Add(fileInfo);
                            }

                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo
                            {
                                name = file.FileName,
                                path = "/",
                                ext = ext,
                                size = file.Length / 1024,
                                code = 900,
                                messages = "File extension not allowed"

                            };
                            uploadResults.Add(fileInfo);
                        }



                        res.Success = true;

                    }
                    res.Data = uploadResults;
                }
                else
                {
                    res.ErrorCode = -1;
                    res.Success = false;
                    res.Message = "Hãy chọn một file.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {

                res.ErrorCode = -2;
                res.Success = false;
                res.Message = "Lỗi hệ thống, Vui lòng liên hệ quản trị.";
                return Ok(res);
            }
        }


        [Route("UploadExcel_Spectification")]
        [HttpPost]
        public IActionResult UploadExcelMaintainSpectificationInProduct(List<IFormFile> files)
        {
            string serverPath = _config.GetValue<string>("AppSettings:UploadFolder");
            string folder = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            var res = new ResponseData();
            try
            {

                var webRoot = _env.WebRootPath;
                var fileDirectory = Path.Combine(serverPath, folder);
                var pathToSave = Path.Combine(webRoot, fileDirectory);

                if (Request.Form.Files.Count > 0)
                {
                    var uploadResults = new List<FileInfo>();
                    string imageAllowUpload = _config.GetValue<string>("AppSettings:ImageAllowUpload");
                    string documentAllowUpload = _config.GetValue<string>("AppSettings:DocumentAllowUpload");
                    bool fileUploadSubFix = _config.GetValue<bool>("AppSettings:FileUploadSubFix");
                    int fileUploadMaxSize = _config.GetValue<int>("AppSettings:FileUploadMaxSize");
                    int imageScaleWidth = _config.GetValue<int>("AppSettings:ImageScaleWidth");
                    int imageScaleHeight = _config.GetValue<int>("AppSettings:ImageScaleHeight");
                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(fileDirectory + "\\" + file.FileName);

                        var imageAllowFileArray = imageAllowUpload.Split(',');
                        var documentAllowFileArray = documentAllowUpload.Split(',');
                        if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1 || Array.IndexOf(documentAllowFileArray, ext.ToLower()) != -1)
                        {
                            var fileName = Path.GetFileName(fileDirectory + "\\" + file.FileName);
                            if (fileUploadMaxSize >= (file.Length / 1024))
                            {

                                if (fileUploadSubFix)
                                {
                                    string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                                    fileName = Utility.UnicodeToKoDauAndGach(filenameNoExtension) + "-" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ext;
                                }
                                var fullPath = Path.Combine(pathToSave, fileName);
                                if (!Directory.Exists(pathToSave))
                                {
                                    Directory.CreateDirectory(pathToSave);
                                }

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);

                                }
                                //Doc file

                                var r = _workingFile.ImportMMaintainSpectificatinInProduct(fullPath);
                            }
                            else
                            {
                                FileInfo fileInfo = new FileInfo
                                {
                                    name = fileName,
                                    path = "/",
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 900,
                                    messages = "Limited file size"

                                };
                                uploadResults.Add(fileInfo);
                            }

                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo
                            {
                                name = file.FileName,
                                path = "/",
                                ext = ext,
                                size = file.Length / 1024,
                                code = 900,
                                messages = "File extension not allowed"

                            };
                            uploadResults.Add(fileInfo);
                        }



                        res.Success = true;

                    }
                    res.Data = uploadResults;
                }
                else
                {
                    res.ErrorCode = -1;
                    res.Success = false;
                    res.Message = "Hãy chọn một file.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {

                res.ErrorCode = -2;
                res.Success = false;
                res.Message = "Lỗi hệ thống, Vui lòng liên hệ quản trị.";
                return Ok(res);
            }
        }





        [HttpDelete("Delete")]
        public ResponseData Delete(int id)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                bool ads = _fileUploadBCL.Remove(id);
                if (ads)
                {
                    responseData.Success = true;
                    responseData.Message = "Thành công";
                }
            }
            catch (Exception ex)
            {

            }
            return responseData;
        }
        [HttpGet("GetAll")]
        public ResponseData GetAll(int take, int pageIndex, string keyword)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                if (string.IsNullOrEmpty(keyword))
                    keyword = "";
                if (keyword.Equals("undefined"))
                    keyword = "";
                int skip = pageIndex > 1 ? ((take * pageIndex) - take) : 0;
                var totals = 0;
                var objs = _fileUploadBCL.Find(keyword, skip, take, out totals);

                //var objs = new List<FileUpload>();
                //objs = _fileUploadBCL.FindAll();

                //if (!String.IsNullOrEmpty(keyword))
                //{
                //    keyword = keyword.ToLower();
                //    objs = _fileUploadBCL.FindAll(x => x.Name.ToLower() == keyword || x.Name.ToLower().Contains(keyword));
                //}

                //var totals = objs.Count();

                var data = objs.OrderByDescending(it => it.UploadedDate).Skip(skip).Take(take).ToList();

                responseData.TotalRow = totals;
                responseData.Success = true;
                responseData.Message = "Thành công";
                responseData.Data = objs.ToList<object>();

            }
            catch (Exception ex)
            {

            }
            return responseData;
        }

        [Route("ExportProductInBankInstallment")]
        [HttpPost]
        public IActionResult ExportProductInBankInstallment([FromBody] ProductBankRequest request)
        {
            string rootFolder = _hostingEnvironment.WebRootPath;
            if (request.productId > 0 && request.type > 0)
            {
                using (IDbContext _context = new IDbContext())
                {
                    var listBankInstallment = _context.BankInstallment.Where(r => r.Type == request.type).ToList();
                    var listProductInBankInstallment = _context.ProductInBankInstallment.Where(r => r.ProductId == request.productId).ToList();

                    try
                    {
                        var stream = new MemoryStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            foreach (var bank in listBankInstallment)
                            {
                                var sheetName = bank.Name + " {" + bank.Id + "}";
                                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                                workSheet.TabColor = System.Drawing.Color.Black;
                                workSheet.DefaultRowHeight = 12;
                                workSheet.Row(1).Height = 20;
                                workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                workSheet.Row(1).Style.Font.Bold = true;
                                workSheet.Row(1).Style.WrapText = true;



                                //Header

                                workSheet.Cells[1, 1].Value = "Kỳ hạn";
                                workSheet.Cells[1, 2].Value = "Trả trước (Theo số tiền)";
                                workSheet.Cells[1, 3].Value = "Trả trước (Theo phần trăm)";
                                workSheet.Cells[1, 4].Value = "Lãi suất";
                                workSheet.Cells[1, 5].Value = "Phụ phí (Theo số tiền)";
                                workSheet.Cells[1, 6].Value = "Phụ phí (Theo phần trăm)";

                                var productInBank = listProductInBankInstallment.Where(r => r.BankInstallmentId == bank.Id);
                                var start_row = 2;
                                foreach (var prod in productInBank)
                                {
                                    workSheet.Cells[start_row, 1].Value = prod.Period;
                                    workSheet.Cells[start_row, 2].Value = prod.PaymentFirst;
                                    workSheet.Cells[start_row, 3].Value = prod.PaymentFirstPercent;
                                    workSheet.Cells[start_row, 4].Value = prod.InterestPercent;
                                    workSheet.Cells[start_row, 5].Value = prod.OthersFee;
                                    workSheet.Cells[start_row, 6].Value = prod.OthersFeePercent;
                                    start_row++;
                                }
                                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                            }

                            package.Save();
                        }
                        stream.Position = 0;

                        string excelName = "ExportProductId_" + request.productId + ".xlsx";
                        var exportFolder = "Export";
                        var exportPath = Path.Combine(rootFolder, exportFolder, excelName);
                        if (!Directory.Exists(Path.Combine(rootFolder, exportFolder)))
                        {
                            Directory.CreateDirectory(Path.Combine(rootFolder, exportFolder));
                        }

                        if (System.IO.File.Exists(exportPath))
                        {
                            System.IO.File.Delete(exportPath);
                        }
                        using (var fileStream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
                        {
                            stream.CopyTo(fileStream);
                        }
                        return Ok(Path.Combine(exportFolder, excelName));
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return BadRequest();
        }

        [Route("ImportProductInBankInstallment")]
        public IActionResult ImportProductInBankInstallment(List<IFormFile> files)
        {
            string serverPath = _config.GetValue<string>("AppSettings:UploadFolder");
            string folder = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            var res = new ResponseData();
            try
            {

                var webRoot = _env.WebRootPath;
                var fileDirectory = Path.Combine(serverPath, folder);
                var pathToSave = Path.Combine(webRoot, fileDirectory);

                if (Request.Form.Files.Count > 0)
                {
                    var uploadResults = new List<FileInfo>();
                    string imageAllowUpload = _config.GetValue<string>("AppSettings:ImageAllowUpload");
                    string documentAllowUpload = _config.GetValue<string>("AppSettings:DocumentAllowUpload");
                    bool fileUploadSubFix = _config.GetValue<bool>("AppSettings:FileUploadSubFix");
                    int fileUploadMaxSize = _config.GetValue<int>("AppSettings:FileUploadMaxSize");
                    int imageScaleWidth = _config.GetValue<int>("AppSettings:ImageScaleWidth");
                    int imageScaleHeight = _config.GetValue<int>("AppSettings:ImageScaleHeight");
                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(fileDirectory + "\\" + file.FileName);

                        var imageAllowFileArray = imageAllowUpload.Split(',');
                        var documentAllowFileArray = documentAllowUpload.Split(',');
                        if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1 || Array.IndexOf(documentAllowFileArray, ext.ToLower()) != -1)
                        {
                            var fileName = Path.GetFileName(fileDirectory + "\\" + file.FileName);
                            if (fileUploadMaxSize >= (file.Length / 1024))
                            {

                                if (fileUploadSubFix)
                                {
                                    string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                                    fileName = Utility.UnicodeToKoDauAndGach(filenameNoExtension) + "-" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ext;
                                }
                                var fullPath = Path.Combine(pathToSave, fileName);
                                if (!Directory.Exists(pathToSave))
                                {
                                    Directory.CreateDirectory(pathToSave);
                                }

                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);

                                }
                                //Doc file
                                if (Request.Form.Keys.Count > 0)
                                {
                                    var keys = Request.Form.Keys;
                                    var productKey = keys.FirstOrDefault();
                                    if (productKey != null)
                                    {
                                        var productId = int.Parse(Request.Form[productKey]);
                                        var r = _workingFile.ImportProductInBankInstallment(fullPath, productId);
                                    }
                                }

                            }
                            else
                            {
                                FileInfo fileInfo = new FileInfo
                                {
                                    name = fileName,
                                    path = "/",
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 900,
                                    messages = "Limited file size"

                                };
                                uploadResults.Add(fileInfo);
                            }

                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo
                            {
                                name = file.FileName,
                                path = "/",
                                ext = ext,
                                size = file.Length / 1024,
                                code = 900,
                                messages = "File extension not allowed"

                            };
                            uploadResults.Add(fileInfo);
                        }



                        res.Success = true;

                    }
                    res.Data = uploadResults;
                }
                else
                {
                    res.ErrorCode = -1;
                    res.Success = false;
                    res.Message = "Hãy chọn một file.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {

                res.ErrorCode = -2;
                res.Success = false;
                res.Message = "Lỗi hệ thống, Vui lòng liên hệ quản trị.";
                return Ok(res);
            }
        }

        //[HttpPost("import_serial_sim")]
        [Route("import_serial_sim")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadExcelFile()
        {
            try
            {
                var form = Request.Form;
                var file = form.Files.GetFile("file");
                var productId = form["productId"].ToString();

                // Kiểm tra xem file có tồn tại không
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("Không có file được chọn hoặc file trống.");
                }
                // Kiểm tra xem file có tồn tại không
                if (file == null || file.Length <= 0)
                {
                    return BadRequest("Không có file được chọn hoặc file trống.");
                }

                using (var stream = new MemoryStream())
                {
                    // Lưu file vào MemoryStream để xử lý
                    file.CopyTo(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        // Mở workbook và chọn sheet đầu tiên
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        // Lấy số dòng có dữ liệu trong sheet
                        int rowCount = worksheet.Dimension.Rows;
                        var request = new List<ProductSerialNumbers>();

                        // In ra các giá trị trong cột 1 và 2
                        for (int row = 2; row <= rowCount; row++)
                        {
                            // Lấy giá trị trong cột 1 và 2
                            string column1Value = worksheet.Cells[row, 1].Value?.ToString();
                            string column2Value = worksheet.Cells[row, 2].Value?.ToString();
                            var item = new ProductSerialNumbers()
                            {
                                ProductId = int.Parse(productId),
                                SerialNumber = column1Value,
                                PhoneNumber = column2Value,
                                Status = "pending"
                            };
                            request.Add(item);

                        }
                        using (var context = new IDbContext())
                        {
                            context.ProductSerialNumbers.AddRange(request);
                            context.SaveChanges();
                        }

                    }
                }

                return Ok("File đã được xử lý thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        [Route("UploadFile")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadFile(List<IFormFile> files)
        {

            string serverPath = _config.GetValue<string>("AppSettings:UploadFolder");
            string folder = string.Format("{0:yyyy/MM/dd}", DateTime.Now);
            var res = new ResponseData();
            res.Success = false;
            try
            {

                var webRoot = _env.WebRootPath;
                var fileDirectory = Path.Combine(serverPath, folder);
                var pathToSave = Path.Combine(webRoot, fileDirectory);

                if (Request.Form.Files.Count > 0)
                {
                    var uploadResults = new List<FileInfo>();
                    string imageAllowUpload = _config.GetValue<string>("AppSettings:ImageAllowUpload");
                    string documentAllowUpload = _config.GetValue<string>("AppSettings:DocumentAllowUpload");
                    bool fileUploadSubFix = _config.GetValue<bool>("AppSettings:FileUploadSubFix");
                    int fileUploadMaxSize = _config.GetValue<int>("AppSettings:FileUploadMaxSize");
                    int imageScaleWidth = _config.GetValue<int>("AppSettings:ImageScaleWidth");
                    int imageScaleHeight = _config.GetValue<int>("AppSettings:ImageScaleHeight");
                    foreach (var file in Request.Form.Files)
                    {
                        var ext = Path.GetExtension(fileDirectory + "\\" + file.FileName);

                        var imageAllowFileArray = imageAllowUpload.Split(',');
                        var documentAllowFileArray = documentAllowUpload.Split(',');
                        if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1 || Array.IndexOf(documentAllowFileArray, ext.ToLower()) != -1)
                        {
                            var fileName = Path.GetFileName(fileDirectory + "\\" + file.FileName);
                            if (fileUploadMaxSize >= (file.Length / 1024))
                            {

                                if (fileUploadSubFix)
                                {
                                    string filenameNoExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                                    fileName = Utility.UnicodeToKoDauAndGach(filenameNoExtension) + "-" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond + ext;
                                }
                                var fullPath = Path.Combine(pathToSave, fileName);
                                if (!Directory.Exists(pathToSave))
                                {
                                    Directory.CreateDirectory(pathToSave);
                                }


                                using (var stream = new FileStream(fullPath, FileMode.Create))
                                {
                                    file.CopyTo(stream);


                                    var obj = new FileUpload();
                                    obj.Name = fileName;
                                    obj.FilePath = "/" + Path.Combine(folder).Replace("\\", "/") + "/" + fileName;
                                    obj.FileExt = ext;
                                    //   obj.Dimensions = dimensions;
                                    obj.FileSize = file.Length / 1024;
                                    obj.Status = 1;
                                    obj.Type = 2;
                                    //uploadResults.Add(obj);
                                    ResponseData responseData = new ResponseData();
                                    try
                                    {
                                        bool result = true;
                                        if (result)
                                        {

                                            responseData.Success = true;
                                            responseData.Message = "Thành công";
                                            FileInfo fileInfo = new FileInfo
                                            {
                                                name = fileName,
                                                path = "/" + Path.Combine(folder).Replace("\\", "/") + "/" + fileName,
                                                ext = ext,
                                                size = file.Length / 1024,//Math.Round((double)file.Length / 1024, 1),
                                                code = 200,
                                                messages = "Upload thành công"

                                            };
                                            uploadResults.Add(fileInfo);
                                            if (Array.IndexOf(imageAllowFileArray, ext.ToLower()) != -1)
                                            {
                                                using (var _stream = file.OpenReadStream())
                                                {
                                                    var uploadedImage = Image.FromStream(_stream);
                                                    var dimensions =
                                                        uploadedImage.PhysicalDimension.Width + "x" +
                                                        uploadedImage.PhysicalDimension.Height;
                                                    obj.Dimensions = dimensions;
                                                    //_fileUploadBCL.Update(obj);

                                                    var thumbPathToSave = Path.Combine(webRoot, serverPath, "thumb",
                                                        folder);
                                                    if (!Directory.Exists(thumbPathToSave))
                                                    {
                                                        Directory.CreateDirectory(thumbPathToSave);
                                                    }

                                                    var thumbFileDirectory = Path.Combine(folder);
                                                    thumbFileDirectory = thumbFileDirectory.Replace("/", "\\");

                                                    var thumbPath =
                                                        "wwwroot\\" + serverPath + "\\thumb\\" +
                                                        thumbFileDirectory +
                                                        "\\" + fileName;
                                                    if (ext.ToLower().Equals(".png"))
                                                    {


                                                        //  postedFile.SaveAs(fileDirectory);
                                                        Bitmap origBMP = new Bitmap(uploadedImage);
                                                        if (origBMP.Width <= imageScaleWidth)
                                                        {
                                                            origBMP.Save(thumbPath);

                                                        }
                                                        else
                                                        {
                                                            int origWidth = origBMP.Width;
                                                            int origHeight = origBMP.Height;
                                                            int newWidth = imageScaleWidth;
                                                            int newHeight = newWidth * origHeight / origWidth;

                                                            Bitmap newBMP = new Bitmap(origBMP, newWidth, newHeight);
                                                            Graphics objGra = Graphics.FromImage(newBMP);
                                                            objGra.DrawImage(origBMP, new Rectangle(0, 0, newBMP.Width, newBMP.Height), 0, 0, origWidth, origHeight, GraphicsUnit.Pixel);
                                                            objGra.Dispose();
                                                            newBMP.Save(thumbPath);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (imageScaleWidth > 0 && imageScaleHeight > 0)
                                                        {
                                                            var img = ImageResize.Scale(uploadedImage, 200, 100);
                                                            img.SaveAs(@"" + thumbPath + "");
                                                        }

                                                        if (imageScaleWidth > 0 && imageScaleHeight == 0)
                                                        {
                                                            var img = ImageResize.ScaleByWidth(uploadedImage,
                                                                imageScaleWidth);
                                                            img.SaveAs(@"" + thumbPath + "");
                                                        }

                                                    }
                                                }
                                            }


                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        FileInfo fileInfo = new FileInfo
                                        {
                                            name = fileName,
                                            path = "/",
                                            ext = ext,
                                            size = file.Length / 1024, //Math.Round((double)file.Length / 1024, 1),
                                            code = 500,
                                            messages = ex.Message

                                        };
                                        uploadResults.Add(fileInfo);
                                    }


                                }

                            }
                            else
                            {
                                FileInfo fileInfo = new FileInfo
                                {
                                    name = fileName,
                                    path = "/",
                                    ext = ext,
                                    size = file.Length / 1024,
                                    code = 900,
                                    messages = "Limited file size"

                                };
                                uploadResults.Add(fileInfo);
                            }

                        }
                        else
                        {
                            FileInfo fileInfo = new FileInfo
                            {
                                name = file.FileName,
                                path = "/",
                                ext = ext,
                                size = file.Length / 1024,
                                code = 900,
                                messages = "File extension not allowed"

                            };
                            uploadResults.Add(fileInfo);

                        }
                    }
                    res.Data = uploadResults;
                    res.Success = true;
                }
                else
                {
                    res.ErrorCode = -1;
                    res.Success = false;
                    res.Message = "Hãy chọn một file.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {

                res.ErrorCode = -2;
                res.Success = false;
                res.Message = "Lỗi hệ thống, Vui lòng liên hệ quản trị.";
                return Ok(res);
            }

        }
        private void write_watermark_text(string sourceImagePath, bool isThumb = false)
        {
            //try
            //{

            //    Image img = Image.FromFile(sourceImagePath);

            //    int width = img.Width;
            //    int height = img.Height;


            //    Point text_starting_point = new Point(height / 6, width / 4);


            //    Image watermark = Image.FromFile(Path.Combine(_env.WebRootPath, !isThumb ? "watermark.jpg" : "watermark-thumb.jpg"));


            //    float height_wtm = (watermark.Height);
            //    float width_wtm = watermark.Width;

            //    // de watermark o vi tri height = 90% anh goc
            //    float height_wtk_position = ((float)((height - height_wtm) * (0.9)));
            //    Graphics graphics = Graphics.FromImage(img);
            //    if (!isThumb)
            //    {
            //        height_wtm = ((float)(height * (0.1)));
            //        height_wtk_position = ((float)((height - height_wtm) * (0.9)));
            //    }
            //    graphics.DrawImage(watermark, 0, height_wtk_position, width, height_wtm);
            //    //graphics.DrawString(text, text_font, brush, text_starting_point);
            //    graphics.Dispose();

            //    var image_2 = new Bitmap(img);

            //    img.Dispose();

            //    if (System.IO.File.Exists(sourceImagePath))
            //        System.IO.File.Delete(sourceImagePath);

            //    image_2.Save(sourceImagePath);
            //    image_2.Dispose();


            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
        }
    }
    public class FileInfo
    {
        public int code { get; set; }
        public string name { get; set; }
        public string path { get; set; }
        public long size { get; set; }
        public string ext { get; set; }
        public string messages { get; set; }
    }

    public class ProductBankRequest
    {
        public int productId { get; set; }
        public int type { get; set; }
    }
}




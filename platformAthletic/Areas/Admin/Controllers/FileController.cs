using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Tools;
using platformAthletic.Global;
using ImageResizer;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class FileController : AdminController
    {
        protected string DestinationDir = "Media/files/uploads/";

        private static string ImageFolder = "/Media/files/images/";
        private static string BannerFolder = "/Media/files/banners/";
        private static string EquipmentFolder = "/Media/files/equipment/";
        private static string GalleryFolder = "/Media/files/gallery/";
        
        private static string ImageSize = "ImageSize";
        private static string ImagePreviewSize = "ImagePreviewSize";
        private static string EquipmentSize = "EquipmentSize";

        private static string GallerySize = "GallerySize";

        private static MemoryStream GetMemoryStream(Stream inputStream)
        {
            var buffer = new byte[inputStream.Length];
            var ms = new MemoryStream(buffer);
            inputStream.CopyTo(ms);
            return ms;
        }

        public ActionResult Index(int page = 1)
        {
            var list = Repository.Files.OrderByDescending(p => p.AddedDate);

            var data = new PageableData<Model.File>();
            data.Init(list, page, "Index");
            ViewBag.ShowMore = list.Count() > page * 20;
            ViewBag.Page = page + 1;
            return View(data);
        }

        public JsonResult Remove(int id)
        {
            var file = Repository.Files.FirstOrDefault(p => p.ID == id);

            if (file != null)
            {
                var fi = new FileInfo(Server.MapPath(file.Path));
                if (fi.Exists)
                {
                    fi.Delete();
                }

                fi = new FileInfo(Server.MapPath(file.Preview));
                if (fi.Exists)
                {
                    fi.Delete();
                }
                Repository.RemoveFile(id);
                return Json(new { result = "ok" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        [ValidateInput(false)]
        [HttpPost]
        public FineUploaderResult UploadFile(FineUpload upload)
        {

            var uFile = StringExtension.GenerateNewFile() + Path.GetExtension(upload.Filename);
            var filePath = Path.Combine(Path.Combine(Server.MapPath("~"), DestinationDir), uFile);
            try
            {
                ImageBuilder.Current.Build(upload.InputStream, filePath, new ResizeSettings("maxwidth=1600&crop=auto"));

                var img = new Bitmap(filePath);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }
            return new FineUploaderResult(true, new { fileUrl =  "/" + DestinationDir + uFile });
        }

        [HttpPost]
        public JsonResult Upload(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && PreviewCreator.SupportMimeType(mimeType.Name))
                    {
                        var ms = GetMemoryStream(inputStream);
                        var file = new Model.File();
                        file.Path = string.Format("{0}{1}.jpg", ImageFolder, StringExtension.GenerateNewFile());
                        file.Preview = file.Path.GetPreviewPath("_preview");

                        MakeImage(ms, ImageSize, file.Path);
                        MakePreview(ms, ImagePreviewSize, file.Preview);
                        Repository.CreateFile(file);

                        return Json(new
                        {
                            success = true,
                            result = "ok",
                            data = new
                            {
                                ID = file.ID,
                                Path = file.Path,
                                PreviewUrl = file.Preview
                            }
                        }, "text/html");
                    }
                }
            }
            return Json(new { success = true, result = "error" });
        }

        [HttpPost]
        public JsonResult UploadPriceImage(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && mimeType.Name.Contains("image"))
                    {
                        var path = "/Media/files/" + fileName;
                        using (var file = System.IO.File.OpenWrite(Server.MapPath(path)))
                        {
                            inputStream.CopyTo(file);
                        }
                        return Json(new
                        {
                            result = "ok",
                            data = path
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                result = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadEquipment(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && mimeType.Name.Contains("image"))
                    {
                        var path = string.Format("{0}{1}.jpg", EquipmentFolder, StringExtension.GenerateNewFile());
                        MakePreview(inputStream, EquipmentSize, path);
                        return Json(new
                        {
                            result = "ok",
                            data = path
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                result = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadGallery(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && mimeType.Name.Contains("image"))
                    {
                        var path = string.Format("{0}{1}.jpg", GalleryFolder, StringExtension.GenerateNewFile());
                        MakePreview(inputStream, GallerySize, path);
                        return Json(new
                        {
                            result = "ok",
                            data = path
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                result = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadBannerSource(string qqfile)
        {
            string fileName;
            var inputStream = GetInputStream(qqfile, out fileName);
            if (inputStream != null)
            {
                var extension = Path.GetExtension(fileName);
                if (extension != null)
                {
                    extension = extension.ToLower();
                    var mimeType = Config.MimeTypes.FirstOrDefault(p => p.Extension == extension);

                    if (mimeType != null && mimeType.Name.Contains("image"))
                    {
                        var path = BannerFolder + fileName;
                        using (var file = System.IO.File.OpenWrite(Server.MapPath(path)))
                        {
                            inputStream.CopyTo(file);
                        }
                        return Json(new
                        {
                            result = "ok",
                            data = path
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new
            {
                result = "error"
            }, JsonRequestBehavior.AllowGet);
        }

        private void MakePreview(Stream ms, string avatarSize, string fileName)
        {
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSavePreview(ms, previewSize, Server.MapPath(fileName), Brushes.White);
            }
        }

        private void MakeImage(Stream ms, string avatarSize, string fileName)
        {
            var avatarSizes = Config.IconSizes.FirstOrDefault(c => c.Name == avatarSize);
            if (avatarSizes != null)
            {
                var previewSize = new Size(avatarSizes.Width, avatarSizes.Height);
                PreviewCreator.CreateAndSaveImage(ms, previewSize, Server.MapPath(fileName), Brushes.White);
            }
        }
    }
}

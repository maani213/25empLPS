using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Employee.Controllers
{
    public class GalleryController : EmployeeBaseController
    {
        // GET: Employee/Gallery/Albums
        public ActionResult Albums()
        {
            try
            {
                var _albumList = new List<AlbumInfo>();

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _albumList = Repo.GetAlbumList();
                }

                return View(_albumList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "Albums"));
            }
        }

        // GET: Employee/Gallery/Photos
        public ActionResult Photos(string id = "")
        {
            try
            {
                AlbumInfo _album = null;
                int _temp;

                if (!int.TryParse(id, out _temp))
                {
                    return RedirectToAction("Albums", "Gallery");
                }

                using (AlbumRepository Repo = new AlbumRepository())
                {
                    _album = Repo.GetAlbumById(int.Parse(id));
                }

                if(_album == null)
                {
                    return RedirectToAction("Albums", "Gallery");
                }

                _album.PhotoInfoList = new List<PhotoInfo>();
                using (PhotoRepository Repo = new PhotoRepository())
                {
                    _album.PhotoInfoList = Repo.GetPhotoListByAlbumId(int.Parse(id));
                }

                return View(_album);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Gallery", "Albums"));
            }
        }
    }
}
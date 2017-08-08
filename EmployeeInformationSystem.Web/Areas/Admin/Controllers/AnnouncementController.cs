using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class AnnouncementController : AdminBaseController
    {
        // GET: Admin/Announcement/Manage
        public ActionResult Manage()
        {
            try
            {
                var _announcementList = new List<AnnouncementInfo>();

                using (AnnouncementRepository Repo = new AnnouncementRepository())
                {
                    _announcementList = Repo.GetAnnouncementList();
                }

                return View(_announcementList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Announcement", "Manage"));
            }
        }

        // GET: Admin/Announcement/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Announcement", "Create"));
            }
        }

        // POST: Admin/Announcement/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnnouncementInfo announcementInfo)
        {
            try
            {
                announcementInfo.CreatedByAccountId = CurrentUser.AccountId;
                announcementInfo.CreatedOn = DateTime.Now;

                using (AnnouncementRepository Repo = new AnnouncementRepository())
                {
                    Repo.SaveAnnouncement(announcementInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Announcement created successfully.");

                return RedirectToAction("Manage", "Announcement");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Announcement", "Create"));
            }
        }

        // GET: Admin/Announcement/Update
        public ActionResult Update(string id = "")
        {
            try
            {
                int _id;
                AnnouncementInfo _announcement = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Manage", "Announcement");
                }

                using (AnnouncementRepository Repo = new AnnouncementRepository())
                {
                    _announcement = Repo.GetAnnouncementById(_id);
                }

                if (_announcement == null)
                {
                    return RedirectToAction("Manage", "Announcement");
                }

                return View(_announcement);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Announcement", "Update"));
            }
        }

        // POST: Admin/Announcement/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AnnouncementInfo announcementInfo)
        {
            try
            {
                announcementInfo.ModifiedByAccountId = CurrentUser.AccountId;
                announcementInfo.ModifiedOn = DateTime.Now;

                using (AnnouncementRepository Repo = new AnnouncementRepository())
                {
                    Repo.UpdateAnnouncement(announcementInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Announcement updated successfully.");

                return RedirectToAction("Manage", "Announcement");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Announcement", "Update"));
            }
        }

        // POST: Admin/Announcement/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id = "")
        {
            try
            {
                int _id;
                AnnouncementInfo _announcement = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Manage", "Announcement");
                }

                using (AnnouncementRepository Repo = new AnnouncementRepository())
                {
                    _announcement = Repo.GetAnnouncementById(_id);
                }

                if (_announcement == null)
                {
                    return RedirectToAction("Manage", "Announcement");

                }

                using (AnnouncementRepository Repo = new AnnouncementRepository())
                {
                    Repo.DeleteAnnouncement(_id);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Announcement deleted successfully.");

                return RedirectToAction("Manage", "Announcement");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Announcement", "Delete"));
            }
        }
    }
}
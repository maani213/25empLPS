using EmployeeInformationSystem.Business.MedicalCheckoutRepository;
using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Helpers;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Controllers
{
    public class SettingController : AppController
    {
        // GET: Setting/SwitchMode
        public ActionResult SwitchMode()
        {
            try
            {
                var _authManager = HttpContext.GetOwinContext().Authentication;
                var _identity = new ClaimsIdentity(User.Identity);
                string _mode = CurrentUser.Mode;

                if (_mode == "Employee")
                {
                    _identity.RemoveClaim(_identity.FindFirst("Mode"));
                    _identity.AddClaim(new Claim("Mode", "Admin"));

                    _authManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                        new ClaimsPrincipal(_identity),
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                            AllowRefresh = true,
                            IsPersistent = false
                        }
                        );

                    return RedirectToAction("Dashboard", "Home", new { Area = "Admin" });
                }

                else if (_mode == "Admin")
                {
                    _identity.RemoveClaim(_identity.FindFirst("Mode"));
                    _identity.AddClaim(new Claim("Mode", "Employee"));

                    _authManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                        new ClaimsPrincipal(_identity),
                        new AuthenticationProperties
                        {
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                            AllowRefresh = true,
                            IsPersistent = false
                        }
                        );

                    return RedirectToAction("Dashboard", "Home", new { Area = "Employee" });
                }

                else
                {
                    return RedirectToAction("Login", "Auth");
                }

            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Setting", "SwitchMode"));
            }
        }

        [ChildActionOnly]
        public ActionResult GetEmployeeMenu()
        {
            var _menu = new MenuViewModel();
            AnnouncementInfo _announcement = null;
            AlbumInfo _album = null;

            using (AnnouncementRepository Repo = new AnnouncementRepository())
            {
                _announcement = Repo.GetRecentAnnouncement();
            }

            using (AlbumRepository Repo = new AlbumRepository())
            {
                _album = Repo.GetRecentAlbum();
            }

            if (_announcement != null)
            {
                TimeSpan tSpan = (DateTime.Parse(_announcement.CreatedOn.Value.ToString()).AddDays(1)).Subtract(DateTime.Now);

                double diff = tSpan.TotalDays;

                if (diff > 0)
                {
                    _menu.IsNewAnnouncement = true;
                }
            }

            if (_album != null)
            {
                TimeSpan tSpan = (DateTime.Parse(_album.CreatedOn.Value.ToString()).AddDays(1)).Subtract(DateTime.Now);

                double diff = tSpan.TotalDays;

                if (diff > 0)
                {
                    _menu.IsNewAlbum = true;
                }
            }

            return PartialView("_EmployeeMenu", _menu);
        }

        [ChildActionOnly]
        public ActionResult GetAdminMenu()
        {
            var _menu = new MenuViewModel();
            var _leaveRequests = new List<LeaveRequestInfo>();

            using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
            {
                var _medicalCheckouts = Repo.GetPendingMedicalCheckoutsList();

                if (_medicalCheckouts.Count() > 0)
                {
                    _menu.IsNewMedicalRequest = true;
                }
            }

            using (LeaveRequestRepository Repo = new LeaveRequestRepository())
            {
                _leaveRequests = Repo.GetLeaveRequestList();
            }

            if (_leaveRequests.Count() > 0)
            {
                _menu.IsNewLeaveRequest = true;
            }

            return PartialView("_AdminMenu", _menu);
        }
    }
}
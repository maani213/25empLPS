using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Employee.Controllers
{
    public class HomeController : EmployeeBaseController
    {
        // GET: Employee/Home/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                NotificationViewModel _notificationModel = new NotificationViewModel();
                _notificationModel.PaySlipInfo = new PaySlipInfo();
                _notificationModel.EmployeeInfo = new EmployeeInfo();

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    _notificationModel.PaySlipInfo = Repo.GetLastPaySlipByEmployeeId(CurrentUser.EmployeeInfoId);
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _notificationModel.EmployeeInfo = Repo.GetEmployeeInfoById(CurrentUser.EmployeeInfoId);

                    _notificationModel.EmployeesBirthDayList = Repo.GetAllEmployeesExceptSuperAdmin().Where(x => x.DateOfBirth != null && x.IsCheckListCompleted == true && DateTime.Parse(x.DateOfBirth.Value.ToString("dd MMMM")).Subtract(DateTime.Parse(DateTime.Now.ToString("dd MMMM"))).TotalDays < 3 && DateTime.Parse(x.DateOfBirth.Value.ToString("dd MMMM")).Subtract(DateTime.Parse(DateTime.Now.ToString("dd MMMM"))).TotalDays >= 0).ToList();
                }

                return View(_notificationModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Dashboard"));
            }
        }

        // GET: Employee/Home/Announcement
        public ActionResult Announcements()
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
                return View("Error", new HandleErrorInfo(ex, "Home", "Announcements"));
            }
        }

        // GET: Employee/Home/GeneralInfo
        public ActionResult GeneralInfo()
        {
            try
            {
                var _employeeInfo = new EmployeeInfo();
                _employeeInfo.DocumentsList = new List<DocumentInfo>();

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employeeInfo = Repo.GetEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    _employeeInfo.DocumentsList = Repo.GetDocumentListByEmployeeId(CurrentUser.EmployeeInfoId);
                }

                if (_employeeInfo.MaritalStatus == "Married")
                {
                    using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                    {
                        _employeeInfo.FamilyMembersList = Repo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);
                    }
                }

                return View(_employeeInfo);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "GeneralInfo"));
            }
        }

        // POST: Employee/Home/DownloadDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadDocument(string id = "")
        {
            try
            {
                int _id;
                DocumentInfo _document = null;

                if (!int.TryParse(id, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid document id, please provide valid document id.");

                    return RedirectToAction("GeneralInfo", "Home");
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    _document = Repo.GetDocumentById(_id);
                }

                if (_document == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Document does not exist.");

                    return RedirectToAction("GeneralInfo", "Home");
                }

                if (_document.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! please try again later.");

                    return RedirectToAction("GeneralInfo", "Home");
                }

                var filepath = Server.MapPath(_document.DocumentPath);
                return File(filepath, MimeMapping.GetMimeMapping(filepath), _document.FileName);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "DownloadDocument"));
            }
        }
    }
}
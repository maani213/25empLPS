using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class ProfileController : AdminBaseController
    {
        // GET: Admin/Profile/Manage
        public ActionResult Manage(string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int empId;
                var _employee = new EmployeeInfo();
                _employee.DocumentsList = new List<DocumentInfo>();

                if (!int.TryParse(id, out empId))
                {
                    _employee = null;
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(empId);
                }

                if (_employee != null)
                {
                    using (DocumentRepository Repo = new DocumentRepository())
                    {
                        _employee.DocumentsList = Repo.GetDocumentListByEmployeeId(empId);
                    }

                    if (_employee.MaritalStatus == "Married")
                    {
                        using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                        {
                            _employee.FamilyMembersList = Repo.GetFamilyMembersListByEmployeeId(empId);
                        }
                    }
                }

                return View(_employee);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "Manage"));
            }
        }

        // POST: Admin/Profile/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(EmployeeInfo employee, string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int empId;
                var _employee = new EmployeeInfo();

                if (!int.TryParse(id, out empId))
                {
                    _employee = null;

                    return View(_employee);
                }

                return RedirectToAction("Manage", "Profile", new { id = empId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "Manage"));
            }
        }

        // GET: Admin/Profile/Update
        public ActionResult Update(string id = "")
        {
            try
            {
                int empId;
                EmployeeInfo _employee = null;

                if (!int.TryParse(id, out empId))
                {
                    return RedirectToAction("Manage", "Profile");
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(empId);
                }

                if (_employee == null)
                {
                    return RedirectToAction("Manage", "Profile");
                }

                if (_employee.RoleName == "SuperAdmin")
                {
                    return RedirectToAction("Manage", "Profile");
                }

                //int yearsDiff = DateTime.Now.Year - DateTime.Parse(_employee.DateOfJoin.ToString()).Year;
                //int monthsDiff = DateTime.Now.Month - DateTime.Parse(_employee.DateOfJoin.ToString()).Month;

                //int totalYears = yearsDiff + int.Parse(_employee.ExperienceYears);
                //int totalMonths = monthsDiff + int.Parse(_employee.ExperienceMonths);

                //_employee.ExperienceYears = totalYears.ToString();
                //_employee.ExperienceMonths = totalMonths.ToString();
                _employee.DocumentName = "NA";

                return View(_employee);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "Update"));
            }
        }

        // POST: Admin/Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(HttpPostedFileBase file, EmployeeInfo employeeInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                int empId;
                EmployeeInfo _employee = null;

                if (!int.TryParse(employeeInfo.Id.ToString(), out empId))
                {
                    return RedirectToAction("Manage", "Profile");
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(empId);
                }

                if (_employee == null)
                {
                    return RedirectToAction("Manage", "Profile");
                }

                if (_employee.RoleName == "SuperAdmin")
                {
                    return RedirectToAction("Manage", "Profile");
                }

                bool isImgSelected = false;

                if (file != null)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpg"))
                            {
                                isImgSelected = true;
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select 'jpg' format only.");

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

                employeeInfo.ModifiedDate = DateTime.Now;
                employeeInfo.ModifiedByAccountId = CurrentUser.AccountId;

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    Repo.UpdateEmployeeInfo(employeeInfo);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Employee info updated successfully.");
                }

                if (isImgSelected == true)
                {
                    string imgPath = Server.MapPath(Url.Content("~/Content/Employee_pictures/" + employeeInfo.AccountId + ".jpg"));
                    file.SaveAs(imgPath);
                }

                return RedirectToAction("Manage", "Profile", new { id = employeeInfo.Id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "Update"));
            }
        }

        // GET: Admin/Profile/UpdateMyInfo
        public ActionResult UpdateMyInfo()
        {
            try
            {
                if (CurrentUser.Role != "SuperAdmin")
                {
                    return RedirectToAction("Dashboard", "Home");
                }

                AdminInfoViewModel _adminInfoModel = new AdminInfoViewModel();

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _adminInfoModel = Repo.GetAdminInfoById(CurrentUser.EmployeeInfoId);
                }

                return View(_adminInfoModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "UpdateMyInfo"));
            }
        }

        // POST: Admin/Profile/UpdateMyInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateMyInfo(HttpPostedFileBase file, AdminInfoViewModel _adminInfoModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                if (CurrentUser.Role != "SuperAdmin")
                {
                    return RedirectToAction("Dashboard", "Home");
                }

                bool isImgSelected = false;

                if (file != null)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg"))
                            {
                                isImgSelected = true;
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

                _adminInfoModel.EmployeeInfoId = CurrentUser.EmployeeInfoId;
                _adminInfoModel.ModifiedDate = DateTime.Now;
                _adminInfoModel.ModifiedByAccountId = CurrentUser.AccountId;

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    Repo.UpdateAdminIfo(_adminInfoModel);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Information updated successfully.");
                }

                if (isImgSelected == true)
                {
                    string imgPath = Server.MapPath(Url.Content("~/Content/Employee_pictures/" + CurrentUser.AccountId + ".jpg"));
                    file.SaveAs(imgPath);
                }

                return RedirectToAction("UpdateMyInfo", "Profile");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "UpdateMyInfo"));
            }
        }

        // POST: Admin/Profile/UploadDocuments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocuments(HttpPostedFileBase file, EmployeeInfo employeeInfo, string EmployeeId = "")
        {
            try
            {
                int _id;
                var _document = new DocumentInfo();
                EmployeeInfo _employee = null;

                if (!int.TryParse(EmployeeId, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(_id);
                }

                if (_employee == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Employee does not exist.");

                    return RedirectToAction("Manage", "Profile");
                }

                if (string.IsNullOrEmpty(employeeInfo.DocumentName))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Document name is required.");

                    return RedirectToAction("Manage", "Profile");
                }

                if (file != null)
                {
                    if (file.ContentType.Contains("image") || file.ContentType.Contains("application/pdf"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg") || file.FileName.Contains(".pdf") || file.FileName.Contains("*.pdf"))
                            {
                                _document.FileName = employeeInfo.DocumentName + Path.GetExtension(file.FileName); ;
                                _document.UploadDate = DateTime.Now;
                                _document.DocumentType = "Other";
                                _document.EmployeeInfoId = _id;

                                string _dirPath = Server.MapPath(Url.Content("~/Content/Employee_documents"));

                                bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                if (!_isDirectoryExists)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Employee_documents")));
                                }

                                Guid guid = Guid.NewGuid();
                                _document.DocumentPath = Url.Content("~/Content/Employee_documents/" + _document.EmployeeInfoId + "-" + guid + "-" + _document.FileName);
                                string _docPath = Server.MapPath(_document.DocumentPath);

                                file.SaveAs(_docPath);

                                using (DocumentRepository Repo = new DocumentRepository())
                                {
                                    Repo.SaveDocument(_document);
                                }
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select jpeg, jpg, pdf format only.");

                                return RedirectToAction("Manage", "Profile", new { id = _id });
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                            return RedirectToAction("Manage", "Profile", new { id = _id });
                        }
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image and pdf only.");

                        return RedirectToAction("Manage", "Profile", new { id = _id });
                    }
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select document.");

                    return RedirectToAction("Manage", "Profile", new { id = _id });
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Document uploaded successfully.");

                return RedirectToAction("Manage", "Profile", new { id = _id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "UploadDocuments"));
            }
        }

        // POST: Admin/Profile/DownloadDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadDocument(string DocumentId = "")
        {
            try
            {
                int _id;
                DocumentInfo _document = null;

                if (!int.TryParse(DocumentId, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid document id, please provide valid document id.");

                    return RedirectToAction("Manage", "Profile");
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    _document = Repo.GetDocumentById(_id);
                }

                if (_document == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Document does not exist.");

                    return RedirectToAction("Manage", "Profile");
                }

                var filepath = Server.MapPath(_document.DocumentPath);
                return File(filepath, MimeMapping.GetMimeMapping(filepath), _document.FileName);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "DownloadDocument"));
            }
        }

        // POST: Admin/Profile/DeleteDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocument(string DocumentId = "")
        {
            try
            {
                int _id;
                DocumentInfo _document = null;

                if (!int.TryParse(DocumentId, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid document id, please provide valid document id.");

                    return RedirectToAction("Manage", "Profile");
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    _document = Repo.GetDocumentById(_id);

                    if (_document == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Document does not exist.");

                        return RedirectToAction("Manage", "Profile");
                    }

                    string _docPath = Request.MapPath(_document.DocumentPath);

                    if (System.IO.File.Exists(_docPath))
                    {
                        System.IO.File.Delete(_docPath);

                        Repo.DeleteDocument(_document.Id);
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Document does not exist.");

                        return RedirectToAction("Manage", "Profile", new { id = _document.EmployeeInfoId });
                    }
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Document deleted successfully.");

                return RedirectToAction("Manage", "Profile", new { id = _document.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Profile", "DeleteDocument"));
            }
        }

        public SelectList GetEmployeeFullNameList()
        {
            using (EmployeeRepository Repo = new EmployeeRepository())
            {
                return new SelectList(Repo.GetEmployeeInfoList(), "Id", "FullName");
            }
        }
    }
}
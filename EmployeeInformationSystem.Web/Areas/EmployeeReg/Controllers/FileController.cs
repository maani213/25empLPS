using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.EmployeeReg.Controllers
{
    public class FileController : EmployeeRegBaseController
    {
        // GET: EmployeeReg/File/UploadDocuments
        public ActionResult UploadDocuments()
        {
            try
            {
                AuthenticatedUser _authUser;
                var _registration = new RegistrationViewModel();

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _registration.employeeRegistrationInfo = Repo.GetRegisterEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    _registration.documentInfoList = Repo.GetDocumentListByEmployeeId(CurrentUser.EmployeeInfoId);
                }

                using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                {
                    _registration.accountCheckListInfo = Repo.GetAccountCheckListByUserId(CurrentUser.AccountId);

                    if (_registration.accountCheckListInfo.IsPersonalInfoProvided == false)
                    {
                        return RedirectToAction("PersonalInfo", "Profile");
                    }

                    if (_registration.employeeRegistrationInfo.MaritalStatus == "Married")
                    {
                        using (FamilyMemberRepository FamilyRepo = new FamilyMemberRepository())
                        {
                            _registration.familyMembersList = FamilyRepo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);

                            if (_registration.familyMembersList.Count() <= 0)
                            {
                                return RedirectToAction("Details", "Family");
                            }
                        }
                    }

                    int count = (from x in _registration.documentInfoList where x.DocumentType == "CNICFront" || x.DocumentType == "CNICBack" || x.DocumentType == "CV" select x).ToList().Count();

                    if (count < 3)
                    {
                        var _accountCheckList = new AccountCheckListInfo();

                        _accountCheckList.IsDocumentsUploaded = false;
                        _accountCheckList.AccountId = CurrentUser.AccountId;

                        Repo.UpdateIsDocumentsUploaded(_accountCheckList);
                        _registration.accountCheckListInfo = Repo.GetAccountCheckListByUserId(CurrentUser.AccountId);
                    }
                }

                return View(_registration);
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/File/UploadDocuments
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadDocuments(HttpPostedFileBase file, DocumentInfo documentInfo)
        {
            try
            {
                AuthenticatedUser _authUser;
                string _fileName = string.Empty;

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                if (documentInfo.DocumentType != "CV" && documentInfo.DocumentType != "CNICFront" && documentInfo.DocumentType != "CNICBack" && documentInfo.DocumentType != "Other")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid document type.");

                    return RedirectToAction("UploadDocuments", "File");
                }

                if (documentInfo.DocumentType == "Other")
                {
                    if (string.IsNullOrEmpty(documentInfo.FileName))
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Document name is required.");

                        return RedirectToAction("UploadDocuments", "File");
                    }

                    _fileName = documentInfo.FileName;
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    var _docList = Repo.GetDocumentListByEmployeeId(CurrentUser.EmployeeInfoId);

                    if (documentInfo.DocumentType == "CV")
                    {
                        _fileName = "CV";

                        int _cvCount = (from x in _docList where x.DocumentType == "CV" select x).ToList().Count();
                        if (_cvCount > 0)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("CV has already uploaded.");

                            return RedirectToAction("UploadDocuments");
                        }
                    }
                    else if (documentInfo.DocumentType == "CNICFront" || documentInfo.DocumentType == "CNICBack")
                    {
                        _fileName = documentInfo.DocumentType == "CNICFront" ? "CNIC Front" : "CNIC Back";

                        int _cnicCount = (from x in _docList where x.DocumentType == "CNICFront" || x.DocumentType == "CNICBack" select x).ToList().Count();

                        if (_cnicCount == 2)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("CNIC has already uploaded.");

                            return RedirectToAction("UploadDocuments");
                        }
                    }
                }

                if (file != null)
                {
                    if (file.ContentType.Contains("image") || file.ContentType.Contains("application/pdf"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg") || file.FileName.Contains(".pdf") || file.FileName.Contains("*.pdf"))
                            {
                                documentInfo.FileName = _fileName + Path.GetExtension(file.FileName); ;
                                documentInfo.UploadDate = DateTime.Now;
                                documentInfo.EmployeeInfoId = CurrentUser.EmployeeInfoId;

                                string _dirPath = Server.MapPath(Url.Content("~/Content/Employee_documents"));

                                bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                if (!_isDirectoryExists)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Employee_documents")));
                                }

                                Guid guid = Guid.NewGuid();
                                documentInfo.DocumentPath = Url.Content("~/Content/Employee_documents/" + documentInfo.EmployeeInfoId + "-" + guid + "-" + documentInfo.FileName);
                                string _docPath = Server.MapPath(documentInfo.DocumentPath);

                                using (var transaction = new System.Transactions.TransactionScope())
                                {
                                    file.SaveAs(_docPath);

                                    var _documentList = new List<DocumentInfo>();

                                    using (DocumentRepository Repo = new DocumentRepository())
                                    {
                                        Repo.SaveDocument(documentInfo);

                                        _documentList = Repo.GetRequiredDocumentListByEmployeeId(CurrentUser.EmployeeInfoId);
                                    }

                                    if (_documentList.Count() >= 3)
                                    {
                                        using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                                        {
                                            AccountCheckListInfo accountCheckListInfo = new AccountCheckListInfo();

                                            accountCheckListInfo.IsDocumentsUploaded = true;
                                            accountCheckListInfo.AccountId = CurrentUser.AccountId;

                                            Repo.UpdateIsDocumentsUploaded(accountCheckListInfo);
                                        }
                                    }

                                    transaction.Complete();
                                }
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select jpeg, jpg, pdf format only.");

                                return RedirectToAction("UploadDocuments", "File");
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                            return RedirectToAction("UploadDocuments", "File");
                        }
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image and pdf only.");

                        return RedirectToAction("UploadDocuments", "File");
                    }
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select document.");

                    return RedirectToAction("UploadDocuments", "File");
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Document uploaded successfully.");

                return RedirectToAction("UploadDocuments", "File");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/File/DeleteDocument
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDocument(string id)
        {
            try
            {
                int n;
                AuthenticatedUser _authUser;

                bool isNumeric = int.TryParse(id, out n);

                if (!isNumeric)
                {
                    return RedirectToAction("UploadDocuments", "File");
                }

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    var _docInfo = new DocumentInfo();

                    _docInfo = Repo.GetDocumentById(int.Parse(id));

                    if (_docInfo == null)
                    {
                        return RedirectToAction("UploadDocuments", "File");
                    }

                    if (_docInfo.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                    {
                        return RedirectToAction("UploadDocuments", "File");
                    }

                    string _docPath = Request.MapPath(_docInfo.DocumentPath);

                    if (System.IO.File.Exists(_docPath))
                    {
                        System.IO.File.Delete(_docPath);

                        Repo.DeleteDocument(_docInfo.Id);
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Document does not exist physically.");

                        return RedirectToAction("UploadDocuments", "File");
                    }
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Document deleted successfully.");

                return RedirectToAction("UploadDocuments", "File");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // GET: EmployeeReg/File/UploadPicture
        public ActionResult UploadPicture()
        {
            try
            {
                AuthenticatedUser _authUser;
                var _registration = new RegistrationViewModel();

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _registration.employeeRegistrationInfo = Repo.GetRegisterEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                {
                    _registration.accountCheckListInfo = Repo.GetAccountCheckListByUserId(CurrentUser.AccountId);
                }

                if (_registration.accountCheckListInfo.IsDocumentsUploaded == false)
                {
                    return RedirectToAction("UploadDocuments", "File");
                }

                if (_registration.employeeRegistrationInfo.MaritalStatus == "Married")
                {
                    using (FamilyMemberRepository FamilyRepo = new FamilyMemberRepository())
                    {
                        _registration.familyMembersList = FamilyRepo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);

                        if (_registration.familyMembersList.Count() <= 0)
                        {
                            return RedirectToAction("Details", "Family");
                        }
                    }
                }

                return View(_registration);
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/File/UploadPicture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPicture(HttpPostedFileBase file)
        {
            try
            {
                AuthenticatedUser _authUser;
                var _registration = new RegistrationViewModel();

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                if (file != null)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg"))
                            {
                                string _dirPath = Server.MapPath(Url.Content("~/Content/Employee_pictures"));

                                bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                if (!_isDirectoryExists)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Employee_pictures")));
                                }

                                string _imgPath = Server.MapPath(Url.Content("~/Content/Employee_pictures/" + CurrentUser.AccountId + ".jpg"));

                                file.SaveAs(_imgPath);

                                using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                                {
                                    var _accountCheckList = new AccountCheckListInfo();

                                    _accountCheckList.IsPictureUploaded = true;
                                    _accountCheckList.AccountId = CurrentUser.AccountId;

                                    Repo.UpdateIsPictureUploaded(_accountCheckList);
                                }
                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select jpeg, jpg format only.");

                                return RedirectToAction("UploadPicture", "File");
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                            return RedirectToAction("UploadPicture", "File");
                        }
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image only.");

                        return RedirectToAction("UploadPicture", "File");
                    }
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select picture.");

                    return RedirectToAction("UploadPicture", "File");
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Picture uploaded successfully.");

                return RedirectToAction("UploadPicture", "File");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/File/ChangePicture
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePicture()
        {
            try
            {
                AuthenticatedUser _authUser;

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                string _imgPath = Server.MapPath(Url.Content("~/Content/Employee_pictures/" + CurrentUser.AccountId + ".jpg"));

                if (System.IO.File.Exists(_imgPath))
                {
                    System.IO.File.Delete(_imgPath);

                    using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                    {
                        var _accountCheckList = new AccountCheckListInfo();

                        _accountCheckList.IsPictureUploaded = false;
                        _accountCheckList.AccountId = CurrentUser.AccountId;

                        Repo.UpdateIsPictureUploaded(_accountCheckList);
                    }
                }

                return RedirectToAction("UploadPicture", "File");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // GET: EmployeeReg/File/Preview
        public ActionResult ReviewInfo()
        {
            try
            {
                var _registrationViewModel = new RegistrationViewModel();
                _registrationViewModel.documentInfoList = new List<DocumentInfo>();

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _registrationViewModel.employeeRegistrationInfo = Repo.GetRegisterEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (DocumentRepository Repo = new DocumentRepository())
                {
                    _registrationViewModel.documentInfoList = Repo.GetDocumentListByEmployeeId(CurrentUser.EmployeeInfoId);
                }

                if (_registrationViewModel.employeeRegistrationInfo.MaritalStatus == "Married")
                {
                    using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                    {
                        _registrationViewModel.familyMembersList = Repo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);
                    }
                }

                return PartialView("_ReviewInfo", _registrationViewModel);
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/File/CheckListCompleted
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckListCompleted()
        {
            try
            {
                AuthenticatedUser _authUser;
                AccountCheckListInfo _accountCheckList;
                var _employeeRegInfo = new EmployeeRegistrationInfo();

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                {
                    _accountCheckList = Repo.GetAccountCheckListByUserId(CurrentUser.AccountId);
                }

                if (_accountCheckList.IsPictureUploaded == false)
                {
                    return RedirectToAction("UploadPicture", "File");
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employeeRegInfo = Repo.GetRegisterEmployeeInfoById(CurrentUser.EmployeeInfoId);

                    _employeeRegInfo.DateOfJoin = DateTime.Now;

                    Repo.RegisterEmployee(_employeeRegInfo);
                }

                using (AccountRepository Repo = new AccountRepository())
                {
                    Repo.CheckListCompleted(CurrentUser.AccountId);
                }

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                var _authManager = HttpContext.GetOwinContext().Authentication;
                var _identity = new ClaimsIdentity(User.Identity);

                _identity.RemoveClaim(_identity.FindFirst(ClaimTypes.Role));
                _identity.AddClaim(new Claim(ClaimTypes.Role, _authUser.Role));
                _identity.RemoveClaim(_identity.FindFirst(ClaimTypes.Name));
                _identity.AddClaim(new Claim(ClaimTypes.Name, _authUser.FirstName + " " + _authUser.LastName));
                //_identity.RemoveClaim(_identity.FindFirst("AccountNo"));
                //_identity.AddClaim(new Claim("AccountNo", "12345"));

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

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }


    }
}
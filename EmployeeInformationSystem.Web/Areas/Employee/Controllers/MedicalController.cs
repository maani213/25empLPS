using EmployeeInformationSystem.Business.MedicalCheckoutRepository;
using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Employee.Controllers
{
    public class MedicalController : EmployeeBaseController
    {
        // GET: Employee/Medical/Details
        public ActionResult Details()
        {
            try
            {
                var _employeeInfo = new EmployeeInfo();
                var _medicalCheckout = new MedicalCheckoutInfo();
                _medicalCheckout.AvailedMedicalsList = new List<MedicalCheckoutInfo>();
                _medicalCheckout.PendingMedicalsList = new List<MedicalCheckoutInfo>();
                _medicalCheckout.MedicalAllowance = new MedicalAllowanceInfo();
                var _incompleteMedicalChekouts = new List<MedicalCheckoutInfo>();
                string _medicalYear = DateTime.Now.Year.ToString();

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    TempData["FamilyMembersList"] = new SelectList(Repo.GetAllFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId), "Id", "Name");
                }

                using (var transaction = new System.Transactions.TransactionScope())
                {
                    using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                    {
                        ViewBag.MedicalYear = new SelectList(Repo.GetMedicalYearsList(CurrentUser.EmployeeInfoId));

                        _incompleteMedicalChekouts = Repo.GetIncompleteMedicalCheckoutsListByEmployeeId(CurrentUser.EmployeeInfoId);

                        if (_incompleteMedicalChekouts.Count() > 0)
                        {
                            using (MedicalPrescriptionRepository PrescriptionRepo = new MedicalPrescriptionRepository())
                            {
                                foreach (var checkout in _incompleteMedicalChekouts)
                                {
                                    var _prescriptionsList = PrescriptionRepo.GetMedicalPrescriptionsListByMedicalCheckoutId(checkout.Id);

                                    foreach (var prescription in _prescriptionsList)
                                    {
                                        string fullPath = Request.MapPath(prescription.PrescriptionPath);

                                        if (System.IO.File.Exists(fullPath))
                                        {
                                            System.IO.File.Delete(fullPath);
                                        }

                                        PrescriptionRepo.DeleteMedicalPrescription(prescription.Id);
                                    }

                                    Repo.DeleteMedicalCheckout(checkout.Id);
                                }
                            }
                        }

                        _medicalCheckout.PendingMedicalsList = Repo.GetPendingMedicalCheckoutsListByEmployeeId(CurrentUser.EmployeeInfoId);
                        _medicalCheckout.AvailedMedicalsList = Repo.GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(CurrentUser.EmployeeInfoId, _medicalYear);
                    }

                    transaction.Complete();
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employeeInfo = Repo.GetEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    if (_employeeInfo.MaritalStatus == "Single")
                    {
                        _medicalCheckout.MedicalAllowance = Repo.GetMedicalAllowanceByCategory("Single");
                    }
                    else
                    {
                        _medicalCheckout.MedicalAllowance = Repo.GetMedicalAllowanceByCategory("Married");
                    }
                }

                return View(_medicalCheckout);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Details"));
            }
        }

        // POST: Employee/Medical/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string MedicalYear = "")
        {
            try
            {
                var _medicalCheckout = new MedicalCheckoutInfo();
                _medicalCheckout.AvailedMedicalsList = new List<MedicalCheckoutInfo>();

                if (MedicalYear == DateTime.Now.ToString("yyyy"))
                {
                    return RedirectToAction("Details", "Medical");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    TempData["FamilyMembersList"] = new SelectList(Repo.GetAllFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId), "Id", "Name");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    ViewBag.MedicalYear = new SelectList(Repo.GetMedicalYearsList(CurrentUser.EmployeeInfoId));
                    _medicalCheckout.AvailedMedicalsList = Repo.GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(CurrentUser.EmployeeInfoId, MedicalYear);
                }

                if (_medicalCheckout.AvailedMedicalsList.Count() == 0)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Medical");
                }

                _medicalCheckout.MedicalAllowance = null;

                return View(_medicalCheckout);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Details"));
            }
        }

        // POST: Employee/Medical/SaveCheckout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCheckout(MedicalCheckoutInfo medicalCheckoutInfo)
        {
            try
            {
                int _medicalCheckoutId = 0;
                EmployeeInfo _employeeInfo = null;
                MedicalAllowanceInfo _medicalAllowance = null;

                if (!ModelState.IsValid)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid data.");

                    return RedirectToAction("Details", "Medical");
                }

                if (medicalCheckoutInfo.TreatmentDate > DateTime.Now.Date)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Treatment date cannot be earlier than today.");

                    return RedirectToAction("Details", "Medical");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    FamilyMemberInfo _familyMember = null;

                    _familyMember = Repo.GetFamilyMemberById(medicalCheckoutInfo.FamilyMemberId);

                    if (_familyMember == null || _familyMember.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid patient name.");

                        return RedirectToAction("Details", "Medical");
                    }
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employeeInfo = Repo.GetEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    if (_employeeInfo.MaritalStatus == "Single")
                    {
                        _medicalAllowance = Repo.GetMedicalAllowanceByCategory("Single");
                    }
                    else
                    {
                        _medicalAllowance = Repo.GetMedicalAllowanceByCategory("Married");
                    }
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    var _availedMedical = Repo.GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(CurrentUser.EmployeeInfoId, DateTime.Now.ToString("yyyy"));

                    var remainingMedicalAmount = _medicalAllowance.Amount - _availedMedical.Sum(x => x.Amount);

                    if (medicalCheckoutInfo.Amount > remainingMedicalAmount)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Selected amount cannot be greater than remaining medical amount.");

                        return RedirectToAction("Details", "Medical");
                    }

                    medicalCheckoutInfo.EmployeeInfoId = CurrentUser.EmployeeInfoId;
                    medicalCheckoutInfo.Status = "Incomplete";

                    _medicalCheckoutId = Repo.SaveMedicalCheckout(medicalCheckoutInfo);
                }

                return RedirectToAction("Apply", "Medical", new { id = _medicalCheckoutId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "SaveCheckout"));
            }
        }

        // GET: Employee/Medical/Apply
        public ActionResult Apply(string id = "")
        {
            try
            {
                int _id;
                MedicalCheckoutInfo _medicalCheckout = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _medicalCheckout = Repo.GetMedicalCheckoutById(_id);
                }

                if (_medicalCheckout == null || _medicalCheckout.EmployeeInfoId != CurrentUser.EmployeeInfoId || _medicalCheckout.Status == "Approved")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                {
                    _medicalCheckout.MedicalPrescriptions = new List<MedicalPrescriptionInfo>();

                    _medicalCheckout.MedicalPrescriptions = Repo.GetMedicalPrescriptionsListByMedicalCheckoutId(_id);
                }

                return View(_medicalCheckout);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Apply"));
            }
        }

        // GET: Employee/Medical/UpdateCheckout
        public ActionResult UpdateCheckout(string id = "")
        {
            try
            {
                int _id;
                MedicalCheckoutInfo _medicalCheckout = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _medicalCheckout = Repo.GetMedicalCheckoutById(_id);
                }

                if (_medicalCheckout == null || _medicalCheckout.EmployeeInfoId != CurrentUser.EmployeeInfoId || _medicalCheckout.Status == "Approved")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                    return RedirectToAction("Details", "Medical");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    TempData["FamilyMembersList"] = new SelectList(Repo.GetAllFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId), "Id", "Name");
                }

                return View(_medicalCheckout);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "UpdateCheckout"));
            }
        }

        // POST: Employee/Medical/UpdateCheckout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCheckout(MedicalCheckoutInfo medicalCheckoutInfo)
        {
            try
            {
                int _id;
                MedicalCheckoutInfo _medicalCheckout = null;

                if (!ModelState.IsValid)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid data.");

                    return RedirectToAction("Details", "Medical");
                }

                if (!int.TryParse(medicalCheckoutInfo.Id.ToString(), out _id))
                {
                    return RedirectToAction("Details", "Medical");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    FamilyMemberInfo _familyMember = null;

                    _familyMember = Repo.GetFamilyMemberById(medicalCheckoutInfo.FamilyMemberId);

                    if (_familyMember == null || _familyMember.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid patient name.");

                        return RedirectToAction("Details", "Medical");
                    }
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _medicalCheckout = Repo.GetMedicalCheckoutById(_id);

                    if (_medicalCheckout == null || _medicalCheckout.EmployeeInfoId != CurrentUser.EmployeeInfoId || _medicalCheckout.Status == "Approved")
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                        return RedirectToAction("Details", "Medical");
                    }

                    Repo.UpdateMedicalCheckout(medicalCheckoutInfo);
                }

                return RedirectToAction("Apply", "Medical", new { id = medicalCheckoutInfo.Id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "UpdateCheckout"));
            }
        }

        // POST: Employee/Medical/UploadPrescription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadPrescription(HttpPostedFileBase file, MedicalCheckoutInfo medicalCheckoutInfo)
        {
            try
            {
                int id;
                MedicalCheckoutInfo _medicalCheckout = null;
                var _prescription = new MedicalPrescriptionInfo();

                if (!int.TryParse(medicalCheckoutInfo.Id.ToString(), out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong please try again later.");

                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _medicalCheckout = Repo.GetMedicalCheckoutById(id);
                }

                if (_medicalCheckout == null || _medicalCheckout.EmployeeInfoId != CurrentUser.EmployeeInfoId || _medicalCheckout.Status == "Approved")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                    return RedirectToAction("Details", "Medical");
                }

                if (string.IsNullOrEmpty(medicalCheckoutInfo.PrescriptionName))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Prescription name is required.");

                    return RedirectToAction("Details", "Medical");
                }

                if (file != null)
                {
                    if (file.ContentType.Contains("image"))
                    {
                        if (file.ContentLength < 2 * 1024 * 1024)
                        {
                            if (file.FileName.Contains(".jpeg") || file.FileName.Contains(".jpg"))
                            {
                                _prescription.FileName = medicalCheckoutInfo.PrescriptionName + Path.GetExtension(file.FileName);
                                _prescription.UploadDate = DateTime.Now;
                                _prescription.MedicalCheckoutId = medicalCheckoutInfo.Id;

                                string _dirPath = Server.MapPath(Url.Content("~/Content/Employee_medical_documents/" + _medicalCheckout.EmployeeName + _medicalCheckout.EmployeeInfoId + " Prescriptions"));

                                bool _isDirectoryExists = System.IO.Directory.Exists(_dirPath);

                                if (!_isDirectoryExists)
                                {
                                    System.IO.Directory.CreateDirectory(Server.MapPath(Url.Content("~/Content/Employee_medical_documents/" + _medicalCheckout.EmployeeName + _medicalCheckout.EmployeeInfoId + " Prescriptions")));
                                }

                                Guid guid = Guid.NewGuid();
                                _prescription.PrescriptionPath = Url.Content("~/Content/Employee_medical_documents/" + _medicalCheckout.EmployeeName + _medicalCheckout.EmployeeInfoId + " Prescriptions/" + _medicalCheckout.EmployeeInfoId + "-" + guid + "-" + _prescription.FileName);

                                string _prescriptionPath = Server.MapPath(_prescription.PrescriptionPath);

                                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                                {
                                    file.SaveAs(_prescriptionPath);

                                    Repo.SaveMedicalPrescription(_prescription);
                                }

                            }
                            else
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select 'jpeg, jpg' format only.");

                                return RedirectToAction("Apply", "Medical", new { id = _medicalCheckout.Id });
                            }
                        }
                        else
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select size upto 2 MB or smaller.");

                            return RedirectToAction("Apply", "Medical", new { id = _medicalCheckout.Id });
                        }
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid content type, please select image only.");

                        return RedirectToAction("Apply", "Medical", new { id = _medicalCheckout.Id });
                    }
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select prescription.");

                    return RedirectToAction("Apply", "Medical", new { id = _medicalCheckout.Id });
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Prescription uploaded successfully.");

                return RedirectToAction("Apply", "Medical", new { id = _medicalCheckout.Id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "UploadPrescription"));
            }
        }

        // POST: Employee/Medical/DeletePrescription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePrescription(string PrescriptionId)
        {
            try
            {
                int id;
                MedicalPrescriptionInfo _prescription = null;

                if (!int.TryParse(PrescriptionId, out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                {
                    _prescription = Repo.GetMedicalPrescriptionById(id);

                    if (_prescription == null || _prescription.EmployeeInfoId != CurrentUser.EmployeeInfoId || _prescription.CheckoutStatus == "Approved")
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Prescription not found.");

                        return RedirectToAction("Details", "Medical");
                    }

                    string _prescriptionPath = Request.MapPath(_prescription.PrescriptionPath);

                    if (System.IO.File.Exists(_prescriptionPath))
                    {
                        System.IO.File.Delete(_prescriptionPath);

                        Repo.DeleteMedicalPrescription(_prescription.Id);
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Prescription does not exist physically.");
                    }
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Prescription deleted successfully.");

                return RedirectToAction("Apply", "Medical", new { id = _prescription.MedicalCheckoutId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "DeletePrescription"));
            }
        }

        // POST: Employee/Medical/ApplyMedical
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyMedical(string id = "")
        {
            try
            {
                int _id;
                MedicalCheckoutInfo _medicalCheckout = null;

                if (!int.TryParse(id, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong please try again later.");

                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                {
                    var _prescriptions = Repo.GetMedicalPrescriptionsListByMedicalCheckoutId(_id);

                    if (_prescriptions.Count() == 0)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Please upload prescription(s).");

                        return RedirectToAction("Details", "Medical", new { id = _id });
                    }
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _medicalCheckout = Repo.GetMedicalCheckoutById(_id);

                    if (_medicalCheckout == null || _medicalCheckout.EmployeeInfoId != CurrentUser.EmployeeInfoId || _medicalCheckout.Status == "Approved")
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                        return RedirectToAction("Details", "Medical");
                    }

                    _medicalCheckout.RequestDate = DateTime.Now;
                    _medicalCheckout.IsCreatedByAdmin = false;
                    _medicalCheckout.Status = "Pending";

                    Repo.UpdateMedicalCheckout(_medicalCheckout);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("You have applied for medical successfully.");
                }

                return RedirectToAction("Details", "Medical");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "ApplyMedical"));
            }
        }

        // POST: Employee/Medical/DeleteCheckout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelCheckout(string CheckoutId)
        {
            try
            {
                int id;
                MedicalCheckoutInfo _checkout = null;
                var _prescriptionsList = new List<MedicalPrescriptionInfo>();

                if (!int.TryParse(CheckoutId, out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _checkout = Repo.GetMedicalCheckoutById(id);

                    if (_checkout == null || _checkout.EmployeeInfoId != CurrentUser.EmployeeInfoId || _checkout.Status == "Approved")
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                        return RedirectToAction("Details", "Medical");
                    }

                    _checkout.Status = "Incomplete";

                    Repo.UpdateMedicalCheckout(_checkout);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Checkout canceled successfully.");

                return RedirectToAction("Details", "Medical");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "CancelCheckout"));
            }
        }

        // POST: Employee/Medical/DownloadPrescriptions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadPrescriptions(string CheckoutId = "")
        {
            try
            {
                int id;
                var _prescriptionsList = new List<MedicalPrescriptionInfo>();
                MedicalCheckoutInfo _checkout = null;
                var outputStream = new MemoryStream();

                if (!int.TryParse(CheckoutId, out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Medical");
                }

                using (MedicalCheckoutRepository CheckoutRepo = new MedicalCheckoutRepository())
                {
                    _checkout = CheckoutRepo.GetMedicalCheckoutById(id);

                    if (_checkout == null || _checkout.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                        return RedirectToAction("Details", "Medical");
                    }
                }

                var _zipName = _checkout.PatientName + "_Prescriptions_" + _checkout.TreatmentDate.Value.ToString("dd-MM-yyyy");

                using (MedicalPrescriptionRepository PrescriptionRepo = new MedicalPrescriptionRepository())
                {
                    _prescriptionsList = PrescriptionRepo.GetMedicalPrescriptionsListByMedicalCheckoutId(id);

                    using (var zip = new ZipFile())
                    {
                        foreach (var item in _prescriptionsList)
                        {
                            zip.AddFile(Server.MapPath(item.PrescriptionPath), string.Empty).FileName = item.FileName;
                        }

                        zip.Save(outputStream);
                    }
                }

                outputStream.Position = 0;

                return File(outputStream, "application/zip", _zipName);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "DownloadPrescriptions"));
            }
        }
    }
}
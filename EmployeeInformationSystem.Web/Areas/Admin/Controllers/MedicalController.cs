using EmployeeInformationSystem.Business.MedicalCheckoutRepository;
using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class MedicalController : Controller
    {

        // GET: Admin/Medical/Manage
        public ActionResult Manage(string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int empId;
                var _checkout = new MedicalCheckoutInfo();
                var _employeeInfo = new EmployeeInfo();
                string _medicalYear = DateTime.Now.Year.ToString();

                if (!int.TryParse(id, out empId))
                {
                    _checkout = null;

                    return View(_checkout);
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    ViewBag.MedicalYear = new SelectList(Repo.GetMedicalYearsList(empId));

                    _checkout.AvailedMedicalsList = Repo.GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(empId, _medicalYear);
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employeeInfo = Repo.GetEmployeeInfoById(empId);
                }

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    if (_employeeInfo.MaritalStatus == "Single")
                    {
                        _checkout.MedicalAllowance = Repo.GetMedicalAllowanceByCategory("Single");
                    }
                    else
                    {
                        _checkout.MedicalAllowance = Repo.GetMedicalAllowanceByCategory("Married");
                    }
                }

                _checkout.EmployeeInfoId = int.Parse(id);

                return View(_checkout);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Manage"));
            }
        }

        // POST: Admin/Medical/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(string MedicalYear = "", string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int _id;
                int _medicalYear;
                var _medicalCheckout = new MedicalCheckoutInfo();
                _medicalCheckout.AvailedMedicalsList = new List<MedicalCheckoutInfo>();

                if (!int.TryParse(id, out _id) || !int.TryParse(MedicalYear, out _medicalYear))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Medical");
                }

                if (MedicalYear == DateTime.Now.ToString("yyyy"))
                {
                    return RedirectToAction("Manage", "Medical");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    ViewBag.MedicalYear = new SelectList(Repo.GetMedicalYearsList(_id));
                    _medicalCheckout.AvailedMedicalsList = Repo.GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(_id, MedicalYear);
                }

                if (_medicalCheckout.AvailedMedicalsList.Count() == 0)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Medical", new { id = _id });
                }

                _medicalCheckout.EmployeeInfoId = _id;
                _medicalCheckout.MedicalAllowance = null;

                return View(_medicalCheckout);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Manage"));
            }
        }

        // POST: Admin/Medical/EmployeeId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeId(string EmployeeInfoId = "")
        {
            try
            {
                return RedirectToAction("Manage", "Medical", new { id = EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "EmployeeId"));
            }
        }

        // GET: Admin/Medical/Requests
        public ActionResult Requests()
        {
            try
            {
                var _medicalRequestList = new List<MedicalCheckoutInfo>();

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _medicalRequestList = Repo.GetPendingMedicalCheckoutsList();
                }

                return View(_medicalRequestList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Requests"));
            }
        }

        // GET: Admin/Medical/RequestDetails
        public ActionResult RequestDetails(string id = "")
        {
            try
            {
                int _id;
                var _pendingRequestList = new List<MedicalCheckoutInfo>();

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Requests", "Medical");
                }

                using (MedicalCheckoutRepository Repo = new MedicalCheckoutRepository())
                {
                    _pendingRequestList = Repo.GetPendingMedicalCheckoutsListByEmployeeId(_id);
                }

                if (_pendingRequestList.Count() == 0)
                {
                    return RedirectToAction("Requests", "Medical");
                }

                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                {
                    foreach (var item in _pendingRequestList)
                    {
                        item.MedicalPrescriptions = Repo.GetMedicalPrescriptionsListByMedicalCheckoutId(item.Id);
                    }
                }

                return View(_pendingRequestList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "RequestDetails"));
            }
        }

        // GET: Admin/Medical/ViewPrescription
        public ActionResult ViewPrescription(string id = "")
        {
            try
            {
                int _id;
                MedicalPrescriptionInfo _prescription = null;

                if (!int.TryParse(id, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return View(_prescription);
                }

                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                {
                    _prescription = Repo.GetMedicalPrescriptionById(_id);

                    if (_prescription == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Prescription not found.");

                        return View(_prescription);
                    }
                }

                return View(_prescription);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "ViewPrescription"));
            }
        }

        // POST: Admin/Medical/DownloadPrescription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadPrescription(string id = "")
        {
            try
            {
                int _id;
                MedicalPrescriptionInfo _prescription = null;

                if (!int.TryParse(id, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Requests", "Medical");
                }

                using (MedicalPrescriptionRepository Repo = new MedicalPrescriptionRepository())
                {
                    _prescription = Repo.GetMedicalPrescriptionById(_id);

                    if (_prescription == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Prescription not found.");

                        return RedirectToAction("Requests", "Medical");
                    }
                }

                var prescriptionPath = Server.MapPath(_prescription.PrescriptionPath);
                return File(prescriptionPath, MimeMapping.GetMimeMapping(prescriptionPath), _prescription.FileName);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "DownloadPrescription"));
            }
        }

        // POST: Admin/Medical/DownloadPrescriptions
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

                    return RedirectToAction("Manage", "Medical");
                }

                using (MedicalCheckoutRepository CheckoutRepo = new MedicalCheckoutRepository())
                {
                    _checkout = CheckoutRepo.GetMedicalCheckoutById(id);
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

        // POST: Admin/Medical/ProcessRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessRequest(string CheckoutId = "", string ProcessBtn = "")
        {
            try
            {
                int _id;
                MedicalCheckoutInfo _checkout = null;
                EmployeeInfo _employeeInfo = null;
                MedicalAllowanceInfo _medicalAllowance = null;

                if (!int.TryParse(CheckoutId, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Requests", "Medical");
                }

                using (MedicalCheckoutRepository CheckoutRepo = new MedicalCheckoutRepository())
                {
                    _checkout = CheckoutRepo.GetMedicalCheckoutById(_id);

                    if (_checkout == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Checkout not found.");

                        return RedirectToAction("Requests", "Medical");
                    }

                    if (ProcessBtn != "Approve" && ProcessBtn != "Reject")
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("RequestDetails", "Medical", new { id = _checkout.Id });
                    }

                    if (ProcessBtn == "Approve")
                    {
                        using (EmployeeRepository EmployeeRepo = new EmployeeRepository())
                        {
                            _employeeInfo = EmployeeRepo.GetEmployeeInfoById(_checkout.EmployeeInfoId);
                        }

                        using (MedicalAllowanceRepository AllowanceRepo = new MedicalAllowanceRepository())
                        {
                            if (_employeeInfo.MaritalStatus == "Single")
                            {
                                _medicalAllowance = AllowanceRepo.GetMedicalAllowanceByCategory("Single");
                            }
                            else
                            {
                                _medicalAllowance = AllowanceRepo.GetMedicalAllowanceByCategory("Married");
                            }
                        }

                        var _availedMedical = CheckoutRepo.GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(_checkout.EmployeeInfoId, DateTime.Now.ToString("yyyy"));

                        var remainingMedicalAmount = _medicalAllowance.Amount - _availedMedical.Sum(x => x.Amount);

                        if (_checkout.Amount > remainingMedicalAmount)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("This request cannot be process, employee has insufficient medical amount.");

                            return RedirectToAction("RequestDetails", "Medical", new { id = _checkout.EmployeeInfoId });
                        }

                        _checkout.Status = "Approved";

                        TempData["Msg"] = AlertMessageProvider.SuccessMessage("Medical request approved successfully.");
                    }
                    else
                    {
                        _checkout.Status = "Incomplete";

                        TempData["Msg"] = AlertMessageProvider.SuccessMessage("Medical request rejected successfully.");
                    }

                    CheckoutRepo.UpdateMedicalCheckout(_checkout);
                }

                return RedirectToAction("RequestDetails", "Medical", new { id = _checkout.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "ProcessRequest"));
            }
        }

        // GET: Admin/Medical/Allowance
        public ActionResult Allowance()
        {
            try
            {
                var _medicalAllowanceList = new List<MedicalAllowanceInfo>();

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    _medicalAllowanceList = Repo.GetMedicalAllowancesList();
                }

                return View(_medicalAllowanceList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "Allowance"));
            }
        }

        // GET: Admin/Medical/CreateAllowance
        public ActionResult CreateAllowance()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "CreateAllowance"));
            }
        }

        // POST: Admin/Medical/CreateAllowance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAllowance(MedicalAllowanceInfo medicalAllowanceInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    medicalAllowanceInfo.Category = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(medicalAllowanceInfo.Category.ToLower());

                    Repo.SaveMedicalAllowance(medicalAllowanceInfo);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Allowance created successfully.");

                    return RedirectToAction("Allowance", "Medical");
                }
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "CreateAllowance"));
            }
        }

        // GET: Admin/Medical/UpdateAllowance
        public ActionResult UpdateAllowance(string id = "")
        {
            try
            {
                int _id;
                MedicalAllowanceInfo _medicalAllowance = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Allowance", "Medical");
                }

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    _medicalAllowance = Repo.GetMedicalAllowanceById(_id);
                }

                if (_medicalAllowance == null)
                {
                    return RedirectToAction("Allowance", "Medical");
                }

                return View(_medicalAllowance);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "UpdateAllowance"));
            }
        }

        // POST: Admin/Medical/UpdateAllowance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAllowance(MedicalAllowanceInfo medicalAllowanceInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    medicalAllowanceInfo.Category = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(medicalAllowanceInfo.Category.ToLower());

                    Repo.UpdateMedicalAllowance(medicalAllowanceInfo);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Allowance updated successfully.");

                    return RedirectToAction("Allowance", "Medical");
                }
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "UpdateAllowance"));
            }
        }

        // POST: Admin/Medical/DeleteAllowance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAllowance(int id)
        {
            try
            {
                using (MedicalAllowanceRepository Repo = new MedicalAllowanceRepository())
                {
                    Repo.DeleteMedicalAllowance(id);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Allowance deleted successfully.");

                    return RedirectToAction("Allowance", "Medical");
                }
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Medical", "DeleteAllowance"));
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
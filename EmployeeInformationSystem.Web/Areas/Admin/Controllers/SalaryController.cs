using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using EmployeeInformationSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class SalaryController : AdminBaseController
    {
        // GET: Admin/Salary/Manage
        public ActionResult Manage()
        {
            try
            {
                ViewBag.SalaryDate = GetSalaryDateList();
                var _salary = new SalaryViewModel();
                var _idsList = new List<int>();
                _salary.SalaryList = new List<SalaryInfo>();

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    if(Repo.GetEmployeeInfoList().Count() == 0)
                    {
                        _salary = null;

                        return View(_salary);
                    }
                }

                _salary.SalaryDate = DateTime.Now.ToString("MMMM - yyyy");

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    _idsList = Repo.GetEmployeeIdsListIncludeSelectedDate(_salary.SalaryDate);
                }

                using (SalaryRepository Repo = new SalaryRepository())
                {
                    _salary.SalaryList = Repo.GetSalaryList();
                }

                if (_idsList.Count() > 0)
                {
                    _salary.SalaryList = _salary.SalaryList.Where(t => !_idsList.Contains((int)t.EmployeeInfoId)).ToList();
                }

                return View(_salary);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Manage"));
            }
        }

        // POST: Admin/Salary/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(string SalaryDate)
        {
            try
            {
                ViewBag.SalaryDate = GetSalaryDateList();
                var _salary = new SalaryViewModel();
                var _idsList = new List<int>();
                _salary.SalaryList = new List<SalaryInfo>();

                _salary.SalaryDate = SalaryDate;

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    _idsList = Repo.GetEmployeeIdsListIncludeSelectedDate(_salary.SalaryDate);
                }

                using (SalaryRepository Repo = new SalaryRepository())
                {
                    _salary.SalaryList = Repo.GetSalaryList();
                }

                if (_idsList.Count() > 0)
                {
                    _salary.SalaryList = _salary.SalaryList.Where(t => !_idsList.Contains((int)t.EmployeeInfoId)).ToList();
                }

                return View(_salary);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Manage"));
            }
        }

        // GET: Admin/Salary/Update
        public ActionResult Update(string id = "")
        {
            try
            {
                int temp;

                if (!int.TryParse(id, out temp))
                {
                    return RedirectToAction("Manage", "Salary");
                }

                SalaryInfo salary = null;

                using (SalaryRepository Repo = new SalaryRepository())
                {
                    salary = Repo.GetSalaryById(int.Parse(id));
                }

                if (salary == null)
                {
                    return RedirectToAction("Manage", "Salary");
                }

                return View(salary);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Update"));
            }
        }

        // POST: Admin/Salary/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SalaryInfo salaryInfo)
        {
            try
            {
                using (SalaryRepository Repo = new SalaryRepository())
                {
                    int totalSalary = Convert.ToInt32(salaryInfo.TotalSalary);

                    salaryInfo.IncomTax = Math.Round(Convert.ToDecimal(TaxCalculator.CalculateTax(totalSalary)), 2);
                    salaryInfo.BasicSalary = Convert.ToDecimal(totalSalary * 0.65);
                    salaryInfo.MedicalAllowance = Convert.ToDecimal(totalSalary * 0.1);
                    salaryInfo.HouseRent = Convert.ToDecimal(totalSalary * 0.25);

                    salaryInfo.ModifiedByAccountId = CurrentUser.AccountId;
                    salaryInfo.ModifiedDate = DateTime.Now;

                    Repo.UpdateSalary(salaryInfo);
                }

                return RedirectToAction("Update", "Salary", new { id = salaryInfo.Id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Update"));
            }
        }

        // POST: Admin/Salary/CreateSalariesHistory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSalariesHistory(string employeesIdList = "", string SalaryDate = "")
        {
            try
            {
                string previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM - yyyy");
                string currentMonth = DateTime.Now.ToString("MMMM - yyyy");
                string nextMonth = DateTime.Now.AddMonths(1).ToString("MMMM - yyyy");

                if (string.IsNullOrEmpty(employeesIdList) || string.IsNullOrEmpty(SalaryDate))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! please try again later.");

                    return View();
                }

                DateTime dt;

                if (!DateTime.TryParse(SalaryDate, out dt))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid date, please select valid date.");

                    return View();
                }

                if (dt.ToString("MMMM - yyyy") != previousMonth && dt.ToString("MMMM - yyyy") != currentMonth && dt.ToString("MMMM - yyyy") != nextMonth)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Salary cannot be created other than previous, current and next month.");

                    return View();
                }

                var _empIdsList = employeesIdList.Split(',').Select(int.Parse).ToList();
                var _paySlipInfo = new PaySlipInfo();
                bool isSucceed = false;

                _paySlipInfo.CreatedDate = DateTime.Now;
                _paySlipInfo.SalaryDate = dt;
                _paySlipInfo.NumberOfDaysWorked = DateTime.DaysInMonth(dt.Year, dt.Month);
                _paySlipInfo.CreatedByAccountId = CurrentUser.AccountId;

                using (var transaction = new System.Transactions.TransactionScope())
                {
                    using (SalaryRepository SalaryRepo = new SalaryRepository())
                    {
                        using (PaySlipRepository PaySlipRepo = new PaySlipRepository())
                        {
                            foreach (var empId in _empIdsList)
                            {
                                SalaryInfo salary = null;
                                salary = SalaryRepo.GetSalaryByEmployeeId(empId);

                                if (salary != null)
                                {
                                    if (PaySlipRepo.IsPayslipCreated((int)salary.EmployeeInfoId, SalaryDate) == true)
                                    {
                                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Selected employee(s) salary of selected month has already created.");

                                        return RedirectToAction("Manage", "Salary");
                                    }

                                    if (salary.TotalSalary > 0)
                                    {
                                        _paySlipInfo.TotalSalary = salary.TotalSalary;
                                        _paySlipInfo.BasicSalary = salary.BasicSalary;
                                        _paySlipInfo.HouseRent = salary.HouseRent;
                                        _paySlipInfo.MedicalAllowance = salary.MedicalAllowance;
                                        _paySlipInfo.IncomTax = salary.IncomTax;
                                        _paySlipInfo.LoanDeduction = salary.LoanDeduction;
                                        _paySlipInfo.OtherDeductions = salary.OtherDeductions;
                                        _paySlipInfo.EmployeeInfoId = (int)salary.EmployeeInfoId;

                                        PaySlipRepo.SavePayslip(_paySlipInfo);

                                        salary.LoanDeduction = 0;
                                        salary.OtherDeductions = 0;

                                        SalaryRepo.UpdateSalary(salary);
                                        isSucceed = true;
                                    }
                                }
                            }
                        }
                    }

                    transaction.Complete();
                }

                if (isSucceed == true)
                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Salaries created successfully.");
                else
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Selected employee(s) salary amount is zero.");

                return RedirectToAction("Manage", "Salary");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "CreateSalariesHistory"));
            }
        }

        // GET: Admin/Salary/Details
        public ActionResult Details(string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int _id;
                var _empSalriesModel = new EmployeeSalariesViewModel();
                _empSalriesModel.PaySlipInfoList = new List<PaySlipInfo>();

                if (!int.TryParse(id, out _id))
                {
                    _empSalriesModel = null;

                    return View(_empSalriesModel);
                }

                string currentYear = DateTime.Now.ToString("yyyy");

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    ViewBag.SalaryDateYear = new SelectList(Repo.GetSalaryYearList(_id));

                    _empSalriesModel.PaySlipInfoList = Repo.GetPayslipListByEmployeeId(_id, currentYear);
                }

                return View(_empSalriesModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Details"));
            }
        }

        // POST: Admin/Salary/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string SalaryDateYear = "", string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();
                int _id;
                int _salaryDateYear;

                var _empSalariesModel = new EmployeeSalariesViewModel();
                _empSalariesModel.PaySlipInfoList = new List<PaySlipInfo>();

                if (!int.TryParse(id, out _id) || !int.TryParse(SalaryDateYear, out _salaryDateYear))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Salary");
                }

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    ViewBag.SalaryDateYear = new SelectList(Repo.GetSalaryYearList(_id));

                    _empSalariesModel.PaySlipInfoList = Repo.GetPayslipListByEmployeeId(_id, SalaryDateYear);
                }

                _empSalariesModel.Id = _id;

                return View(_empSalariesModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Details"));
            }
        }

        // POST: Admin/Salary/EmployeeId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeId(string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int _id;
                var _empSalriesModel = new EmployeeSalariesViewModel();
                _empSalriesModel.PaySlipInfoList = new List<PaySlipInfo>();

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Details", "Salary");
                }

                return RedirectToAction("Details", "Salary", new { id = _id });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "EmployeeId"));
            }
        }

        // GET: Admin/Salary/PayslipPrintPreview
        public ActionResult PayslipPrintPreview(string id = "")
        {
            try
            {
                PaySlipInfo _paySlip = null;
                int _temp;

                if (!int.TryParse(id, out _temp))
                {
                    return RedirectToAction("Details", "Salary");
                }

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    _paySlip = Repo.GetPayslipById(int.Parse(id));
                }

                if (_paySlip == null)
                {
                    return RedirectToAction("Details", "Salary");
                }

                return PartialView("_PayslipPrintPreview", _paySlip);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "PayslipPrintPreview"));
            }
        }

        // POST: Admin/Salary/DeletePayslip
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePayslip(string PayslipId = "")
        {
            try
            {
                int _id;
                PaySlipInfo _paySlip = null;
                string previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM - yyyy");
                string currentMonth = DateTime.Now.ToString("MMMM - yyyy");
                string nextMonth = DateTime.Now.AddMonths(1).ToString("MMMM - yyyy");

                if (!int.TryParse(PayslipId, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Salary");
                }

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    _paySlip = Repo.GetPayslipById(_id);

                    if (_paySlip == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Payslip does not exist.");

                        return RedirectToAction("Details", "Salary");
                    }

                    if (_paySlip.SalaryDate.ToString("MMMM - yyyy") != previousMonth && _paySlip.SalaryDate.ToString("MMMM - yyyy") != currentMonth && _paySlip.SalaryDate.ToString("MMMM - yyyy") != nextMonth)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Selected payslip cannot be deleted.");

                        return RedirectToAction("Details", "Salary", new { id = _paySlip.EmployeeInfoId });
                    }

                    Repo.DeletePayslip(_id);

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Payslip deleted successfully.");
                }

                return RedirectToAction("Details", "Salary", new { id = _paySlip.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "DeletePayslip"));
            }
        }

        public SelectList GetSalaryDateList()
        {
            var _salaryDateList = new List<string>()
            {
                DateTime.Now.AddMonths(-1).ToString("MMMM - yyyy"),
                DateTime.Now.ToString("MMMM - yyyy"),
                DateTime.Now.AddMonths(1).ToString("MMMM - yyyy")
            };

            return new SelectList(_salaryDateList);
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
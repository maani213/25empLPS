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
    public class SalaryController : EmployeeBaseController
    {
        // GET: Employee/Salary/Details
        public ActionResult Details()
        {
            try
            {
                var _paySlipList = new List<PaySlipInfo>();
                string _currentYear = DateTime.Now.ToString("yyyy");

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    ViewBag.SalaryDateYear = new SelectList(Repo.GetSalaryYearList(CurrentUser.EmployeeInfoId));

                    _paySlipList = Repo.GetPayslipListByEmployeeId(CurrentUser.EmployeeInfoId, _currentYear);
                }

                return View(_paySlipList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Details"));
            }
        }

        // POST: Employee/Salary/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string SalaryDateYear = "")
        {
            try
            {
                int _temp;

                if (!int.TryParse(SalaryDateYear, out _temp))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return View();
                }

                var _paySlipList = new List<PaySlipInfo>();

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    ViewBag.SalaryDateYear = new SelectList(Repo.GetSalaryYearList(CurrentUser.EmployeeInfoId));

                    _paySlipList = Repo.GetPayslipListByEmployeeId(CurrentUser.EmployeeInfoId, SalaryDateYear);
                }

                return View(_paySlipList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Salary", "Details"));
            }
        }

        // GET: Employee/Salary/PayslipPrintPreview
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

                if (_paySlip.EmployeeInfoId != CurrentUser.EmployeeInfoId)
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

    }
}
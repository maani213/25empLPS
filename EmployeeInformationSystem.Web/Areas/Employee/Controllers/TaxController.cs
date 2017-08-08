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
    public class TaxController : EmployeeBaseController
    {
        // GET: Employee/Tax/Details
        public ActionResult Details()
        {
            try
            {
                var _paySlipList = new List<PaySlipInfo>();
                string _TaxYear = DateTime.Now.Year.ToString();

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    ViewBag.TaxYear = new SelectList(Repo.GetSalaryYearList(CurrentUser.EmployeeInfoId));

                    _paySlipList = Repo.GetPayslipListByEmployeeId(CurrentUser.EmployeeInfoId, _TaxYear);
                }

                return View(_paySlipList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tax", "Details"));
            }
        }

        // POST: Employee/Tax/Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(string TaxYear = "")
        {
            try
            {
                int _temp;

                if (!int.TryParse(TaxYear, out _temp))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return View();
                }

                var _paySlipList = new List<PaySlipInfo>();

                using (PaySlipRepository Repo = new PaySlipRepository())
                {
                    ViewBag.TaxYear = new SelectList(Repo.GetSalaryYearList(CurrentUser.EmployeeInfoId));

                    _paySlipList = Repo.GetPayslipListByEmployeeId(CurrentUser.EmployeeInfoId, TaxYear);
                }

                return View(_paySlipList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tax", "Details"));
            }
        }
    }
}
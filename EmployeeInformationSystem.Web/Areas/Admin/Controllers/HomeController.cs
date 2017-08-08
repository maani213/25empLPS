using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        // GET: Admin/Home/Dashboard
        public ActionResult Dashboard()
        {
            try
            {
                NotificationViewModel _notificationModel = new NotificationViewModel();

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _notificationModel.EmployeesProbationCompleteList = Repo.GetAllEmployeesExceptSuperAdmin().Where(x => x.DateOfJoin != null && x.ProbationPeriod != null && ((DateTime.Parse(DateTime.Now.ToString("dd MMMM yyyy")) - DateTime.Parse(x.DateOfJoin.Value.ToString("dd MMMM yyyy")).AddDays(int.Parse(x.ProbationPeriod) * 30)).TotalDays < 0 && (DateTime.Parse(DateTime.Now.ToString("dd MMMM yyyy")) - DateTime.Parse(x.DateOfJoin.Value.ToString("dd MMMM yyyy")).AddDays(int.Parse(x.ProbationPeriod) * 30)).TotalDays >= -7)).ToList();

                    _notificationModel.EmployeesBirthDayList = Repo.GetAllEmployeesExceptSuperAdmin().Where(x => x.DateOfBirth != null && x.IsCheckListCompleted == true && DateTime.Parse(x.DateOfBirth.Value.ToString("dd MMMM")).Subtract(DateTime.Parse(DateTime.Now.ToString("dd MMMM"))).TotalDays < 3 && DateTime.Parse(x.DateOfBirth.Value.ToString("dd MMMM")).Subtract(DateTime.Parse(DateTime.Now.ToString("dd MMMM"))).TotalDays >= 0).ToList();
                }

                return View(_notificationModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Dashboard"));
            }
        }
    }
}
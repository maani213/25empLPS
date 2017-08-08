using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Employee.Controllers
{
    public class AttendanceController : EmployeeBaseController
    {
        // GET: Employee/Attendance/Details
        public ActionResult Details()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Attendance", "Details"));
            }
        }
    }
}
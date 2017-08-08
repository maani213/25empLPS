using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Employee
{
    [Authorize(Roles = "Employee, Administrator")]
    public class EmployeeBaseController : AppController
    {

    }
}
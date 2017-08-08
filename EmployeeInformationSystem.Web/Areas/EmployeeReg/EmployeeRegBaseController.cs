using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.EmployeeReg
{
    [Authorize(Roles = "Anonymous")]
    public class EmployeeRegBaseController : AppController
    {

    }
}
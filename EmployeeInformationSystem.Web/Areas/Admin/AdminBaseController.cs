﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    public class AdminBaseController : AppController
    {

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error/PageNotFound
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        // GET: Error/InternalServerError
        public ViewResult InternalServerError()
        {
            Response.StatusCode = 500;
            return View();
        }
    }
}
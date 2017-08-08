using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.EmployeeReg
{
    public class EmployeeRegAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "EmployeeReg";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EmployeeReg_default",
                "EmployeeReg/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "EmployeeInformationSystem.Web.Areas.EmployeeReg.Controllers" }
            );
        }
    }
}
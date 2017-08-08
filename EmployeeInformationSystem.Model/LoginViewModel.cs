using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Model
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter email address.</li></ul>")]
        [EmailAddress(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid email address.</li></ul>")]
        [StringLength(90, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter email upto 90 characters.</li></ul>")]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter password.</li></ul>")]
        [StringLength(40, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter password upto 40 characters.</li></ul>")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[\w]{0,40}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Illegal characters are not allowed.</li></ul>")]
        public string Password { get; set; }

        [HiddenInput]
        public string ReturnUrl { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter email address.</li></ul>")]
        [EmailAddress(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid email address.</li></ul>")]
        [StringLength(100, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter email upto 100 characters.</li></ul>")]
        public string CompanyEmail { get; set; }
    }
}

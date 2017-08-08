using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class ChangePasswordViewModel
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Current password is required.</li></ul>")]
        [StringLength(40, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Your password cannot exceed 40 characters.</li></ul>")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[\w]{0,40}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Space & special characters are not allowed.</li></ul>")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>New password is required</li></ul>")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Password must have at least 8 characters and contain at least one uppercase letter, one lowercase letter, one number. Space & special characters are not allowed.</li></ul>")]
        [StringLength(40, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Your password cannot exceed 40 characters.</li></ul>")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Confirm password is required.</li></ul>")]
        [Compare("NewPassword", ErrorMessage = "<ul class='parsley-errors-list filled'><li>New password and confirm password do not match.</li></ul>")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

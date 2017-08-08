using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Model
{
    public class AccountInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Company email is required.</li></ul>")]
        [EmailAddress(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid email address.</li></ul>")]
        [StringLength(140, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter email upto 140 characters.</li></ul>")]
        public string CompanyEmail { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please select role.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid role.</li></ul>")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsCheckListCompleted { get; set; }
        public string EmployeeFullName { get; set; }
        public int EmployeeId { get; set; }
        public List<RoleInfo> RolesList { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class AuthenticatedUser
    {
        public int AccountId { get; set; }
        public int EmployeeInfoId { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string LastName { get; set; }
        public string CompanyEmail { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIp { get; set; }
        public bool IsActive { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public bool IsCheckListCompleted { get; set; }
    }
}

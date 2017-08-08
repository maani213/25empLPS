using System.Security.Claims;

namespace EmployeeInformationSystem.Web
{
    public class AppUser : ClaimsPrincipal
    {
        public AppUser(ClaimsPrincipal principal)
            : base(principal)
        {
        }

        public string Name
        {
            get
            {
                return this.FindFirst(ClaimTypes.Name).Value;
            }
        }

        public string Email
        {
            get
            {
                return this.FindFirst(ClaimTypes.Email).Value;
            }
        }

        public string Role
        {
            get
            {
                return this.FindFirst(ClaimTypes.Role).Value;
            }
        }

        public int AccountId
        {
            get
            {
                return int.Parse(this.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
        }

        public int EmployeeInfoId
        {
            get
            {
                return int.Parse(this.FindFirst("EmployeeInfoId").Value);
            }
        }

        public string LastLoginDate
        {
            get
            {
                return (this.FindFirst("LastLoginDate").Value);
            }
        }

        public string LastLoginIp
        {
            get
            {
                return (this.FindFirst("LastLoginIp").Value);
            }
        }

        public string Mode
        {
            get
            {
                return (this.FindFirst("Mode").Value);
            }
        }
    }
}
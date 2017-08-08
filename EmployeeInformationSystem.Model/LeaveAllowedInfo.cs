using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class LeaveAllowedInfo
    {
        public int Id { get; set; }
        public int Casual { get; set; }
        public int Annual { get; set; }
        public int EmployeeInfoId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByAccountId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedByAccountId { get; set; }
        public string EmployeeFullName { get; set; }

        public LeaveAllowedInfo()
        {

        }

        public LeaveAllowedInfo(int casual, int annual, int employeeId, int accountId)
        {
            Casual = casual;
            Annual = annual;
            EmployeeInfoId = employeeId;
            CreatedDate = DateTime.Now;
            CreatedByAccountId = accountId;
        }
    }
}

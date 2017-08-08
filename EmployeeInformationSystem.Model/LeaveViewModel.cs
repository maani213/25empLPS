using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class LeaveViewModel
    {
        public string EmployeeInfoId { get; set; }
        public AvailedLeaveViewModel availedLeaveViewModel { get; set; }
        public LeaveInfo CasualLeave { get; set; }
        public LeaveInfo AnnualLeave { get; set; }
        public List<LeaveRequestInfo> LeaveRequestInfoList { get; set; }
        public LeaveRequestInfo LeaveRequestInfo { get; set; }

        public List<LeaveRequestInfo> CasualLeaveRequestInfoList { get; set; }
        public List<LeaveRequestInfo> AnnualLeaveRequestInfoList { get; set; }
    }
}

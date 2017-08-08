using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class AvailedLeaveViewModel
    {
        public List<LeaveRequestInfo> CasualLeaveRequestInfoList { get; set; }
        public List<LeaveRequestInfo> AnnualLeaveRequestInfoList { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class NotificationViewModel
    {
        public EmployeeInfo EmployeeInfo { get; set; }
        public PaySlipInfo PaySlipInfo { get; set; }
        public List<EmployeeInfo> EmployeesProbationCompleteList { get; set; }
        public List<EmployeeInfo> EmployeesBirthDayList { get; set; }
    }
}

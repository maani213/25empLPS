using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class LeaveInfo
    {
        public int Id { get; set; }
        public int Allowed { get; set; }
        public int Availed { get; set; }
        public int Remaining { get; set; }
        public string LeaveType { get; set; }
        public int EmployeeInfoId { get; set; }
    }
}

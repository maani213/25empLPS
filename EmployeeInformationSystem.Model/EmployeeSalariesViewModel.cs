using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class EmployeeSalariesViewModel
    {
        public int Id { get; set; }
        public List<PaySlipInfo> PaySlipInfoList { get; set; }
    }
}

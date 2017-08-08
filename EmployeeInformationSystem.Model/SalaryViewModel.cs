using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class SalaryViewModel
    {
        public string SalaryDate { get; set; }
        public List<SalaryInfo> SalaryList { get; set; }
    }
}

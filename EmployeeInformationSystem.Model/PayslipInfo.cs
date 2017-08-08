using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class PaySlipInfo
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public int? EmpCode { get; set; }
        public int NumberOfDaysWorked { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? IncomTax { get; set; }
        public decimal? LoanDeduction { get; set; }
        public decimal? OtherDeductions { get; set; }
        public DateTime SalaryDate { get; set; }
        public int EmployeeInfoId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByAccountId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedByAccountId { get; set; }
    }
}

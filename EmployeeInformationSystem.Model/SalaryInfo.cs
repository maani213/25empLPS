using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class SalaryInfo
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public decimal TotalSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public decimal? HouseRent { get; set; }
        public decimal? MedicalAllowance { get; set; }
        public decimal? IncomTax { get; set; }
        public decimal? LoanDeduction { get; set; }
        public decimal? OtherDeductions { get; set; }
        public int? EmployeeInfoId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedByAccountId { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedByAccountId { get; set; }

        public SalaryInfo()
        {
            BasicSalary = 0;
            HouseRent = 0;
            MedicalAllowance = 0;
            IncomTax = 0;
            LoanDeduction = 0;
            OtherDeductions = 0;
            CreatedDate = DateTime.Now;
        }

        public SalaryInfo(int accountId, int employeeInfoId)
        {
            BasicSalary = 0;
            HouseRent = 0;
            MedicalAllowance = 0;
            IncomTax = 0;
            LoanDeduction = 0;
            OtherDeductions = 0;
            EmployeeInfoId = employeeInfoId;
            CreatedDate = DateTime.Now;
            CreatedByAccountId = accountId;
        }
    }
}

using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class PaySlipRepository : IDisposable
    {

        EmployeeInformationEntities _context = null;

        public PaySlipRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public PaySlipRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<PaySlipInfo> GetPayslipList()
        {

            return (from employeeInfo in _context.EmployeeInfoes.ToList()
                    join payslip in _context.PaySlips.ToList()
                    on employeeInfo.Id equals payslip.EmployeeInfoId
                    select new PaySlipInfo
                    {
                        Id = payslip.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        TotalSalary = payslip.TotalSalary,
                        BasicSalary = payslip.BasicSalary,
                        HouseRent = payslip.HouseRent,
                        MedicalAllowance = payslip.MedicalAllowance,
                        IncomTax = payslip.IncomTax,
                        LoanDeduction = payslip.LoanDeduction,
                        OtherDeductions = payslip.OtherDeductions,
                        EmployeeInfoId = payslip.EmployeeInfoId,
                        CreatedDate = payslip.CreatedDate,
                        CreatedByAccountId = payslip.CreatedByAccountId,
                        ModifiedDate = payslip.ModifiedDate,
                        ModifiedByAccountId = payslip.ModifiedByAccountId

                    }).ToList();

        }

        public PaySlipInfo GetLastPaySlipByEmployeeId(int employeeId)
        {
            return (from employeeInfo in _context.EmployeeInfoes.ToList()
                    join payslip in _context.PaySlips.ToList()
                    on employeeInfo.Id equals payslip.EmployeeInfoId
                    where employeeInfo.Id == employeeId
                    select new PaySlipInfo
                    {
                        Id = payslip.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        Position = employeeInfo.Position,
                        EmpCode = employeeInfo.EmpCode,
                        NumberOfDaysWorked = payslip.NumberOfDaysWorked,
                        TotalSalary = payslip.TotalSalary,
                        BasicSalary = payslip.BasicSalary,
                        HouseRent = payslip.HouseRent,
                        MedicalAllowance = payslip.MedicalAllowance,
                        IncomTax = payslip.IncomTax,
                        LoanDeduction = payslip.LoanDeduction,
                        OtherDeductions = payslip.OtherDeductions,
                        SalaryDate = payslip.SalaryDate,
                        EmployeeInfoId = payslip.EmployeeInfoId,
                        CreatedDate = payslip.CreatedDate,
                        CreatedByAccountId = payslip.CreatedByAccountId,
                        ModifiedDate = payslip.ModifiedDate,
                        ModifiedByAccountId = payslip.ModifiedByAccountId

                    }).LastOrDefault();
        }

        public List<int> GetSalaryYearList(int employeeId)
        {
            return (from payslip in _context.PaySlips.ToList()
                    where payslip.EmployeeInfoId == employeeId
                    orderby payslip.SalaryDate descending
                    group payslip by payslip.SalaryDate.ToString("yyyy") into grp
                    select DateTime.ParseExact(grp.Key, "yyyy", CultureInfo.CurrentCulture).Year
                    ).ToList();
        }

        public List<string> GetSalaryDateList()
        {
            return (from payslip in _context.PaySlips.ToList()
                    orderby payslip.SalaryDate descending
                    group payslip by payslip.SalaryDate.ToString("MMMM - yyyy") into grp
                    select grp.Key
                    ).ToList();
        }

        public bool IsPayslipCreated(int empId, string SalaryDate)
        {
            int count = _context.PaySlips.ToList().Where(x => x.EmployeeInfoId == empId && x.SalaryDate.ToString("MMMM - yyyy") == SalaryDate).Count();

            return count > 0 ? true : false;
        }

        public List<int> GetEmployeeIdsListIncludeSelectedDate(string salaryDate)
        {
            return (from payslip in _context.PaySlips.ToList()
                    group payslip by payslip.EmployeeInfoId into grp
                    where grp.Any(x => x.SalaryDate.ToString("MMMM - yyyy") == salaryDate)
                    select grp.Key
                    ).ToList();
        }

        public List<PaySlipInfo> GetPayslipListByEmployeeId(int employeeId, string year)
        {
            return (from employeeInfo in _context.EmployeeInfoes.ToList()
                    join payslip in _context.PaySlips.ToList()
                    on employeeInfo.Id equals payslip.EmployeeInfoId
                    where payslip.EmployeeInfoId == employeeId && payslip.SalaryDate.ToString("yyyy") == year
                    orderby payslip.SalaryDate ascending
                    select new PaySlipInfo
                    {
                        Id = payslip.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        Position = employeeInfo.Position,
                        EmpCode = employeeInfo.EmpCode,
                        NumberOfDaysWorked = payslip.NumberOfDaysWorked,
                        TotalSalary = payslip.TotalSalary,
                        BasicSalary = payslip.BasicSalary,
                        HouseRent = payslip.HouseRent,
                        MedicalAllowance = payslip.MedicalAllowance,
                        IncomTax = payslip.IncomTax,
                        LoanDeduction = payslip.LoanDeduction,
                        OtherDeductions = payslip.OtherDeductions,
                        SalaryDate = payslip.SalaryDate,
                        EmployeeInfoId = payslip.EmployeeInfoId,
                        CreatedDate = payslip.CreatedDate,
                        CreatedByAccountId = payslip.CreatedByAccountId,
                        ModifiedDate = payslip.ModifiedDate,
                        ModifiedByAccountId = payslip.ModifiedByAccountId

                    }).ToList();
        }


        public PaySlipInfo GetPayslipById(int id)
        {
            return (from employeeInfo in _context.EmployeeInfoes.ToList()
                    join payslip in _context.PaySlips.ToList()
                    on employeeInfo.Id equals payslip.EmployeeInfoId
                    where payslip.Id == id
                    select new PaySlipInfo
                    {
                        Id = payslip.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        Position = employeeInfo.Position,
                        EmpCode = employeeInfo.EmpCode,
                        NumberOfDaysWorked = payslip.NumberOfDaysWorked,
                        TotalSalary = payslip.TotalSalary,
                        BasicSalary = payslip.BasicSalary,
                        HouseRent = payslip.HouseRent,
                        MedicalAllowance = payslip.MedicalAllowance,
                        IncomTax = payslip.IncomTax,
                        LoanDeduction = payslip.LoanDeduction,
                        OtherDeductions = payslip.OtherDeductions,
                        SalaryDate = payslip.SalaryDate,
                        EmployeeInfoId = payslip.EmployeeInfoId,
                        CreatedDate = payslip.CreatedDate,
                        CreatedByAccountId = payslip.CreatedByAccountId,
                        ModifiedDate = payslip.ModifiedDate,
                        ModifiedByAccountId = payslip.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public void SavePayslip(PaySlipInfo paySlipInfo)
        {
            Data.PaySlip payslip = ConvertToDb(paySlipInfo);

            _context.PaySlips.Add(payslip);
            _context.SaveChanges();

        }

        public void DeletePayslip(int id)
        {
            Data.PaySlip payslip = _context.PaySlips.Find(id);

            if (payslip != null)
            {
                _context.PaySlips.Remove(payslip);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public PaySlipInfo ConvertToFacade(Data.PaySlip paySlip)
        {
            return new PaySlipInfo
            {
                Id = paySlip.Id,
                NumberOfDaysWorked = paySlip.NumberOfDaysWorked,
                TotalSalary = paySlip.TotalSalary,
                BasicSalary = paySlip.BasicSalary,
                HouseRent = paySlip.HouseRent,
                MedicalAllowance = paySlip.MedicalAllowance,
                IncomTax = paySlip.IncomTax,
                LoanDeduction = paySlip.LoanDeduction,
                OtherDeductions = paySlip.OtherDeductions,
                SalaryDate = paySlip.SalaryDate,
                EmployeeInfoId = paySlip.EmployeeInfoId,
                CreatedDate = paySlip.CreatedDate,
                CreatedByAccountId = paySlip.CreatedByAccountId,
                ModifiedDate = paySlip.ModifiedDate,
                ModifiedByAccountId = paySlip.ModifiedByAccountId

            };
        }

        public Data.PaySlip ConvertToDb(PaySlipInfo paySlipInfo)
        {
            return new Data.PaySlip
            {
                Id = paySlipInfo.Id,
                NumberOfDaysWorked = paySlipInfo.NumberOfDaysWorked,
                TotalSalary = paySlipInfo.TotalSalary,
                BasicSalary = paySlipInfo.BasicSalary,
                HouseRent = paySlipInfo.HouseRent,
                MedicalAllowance = paySlipInfo.MedicalAllowance,
                IncomTax = paySlipInfo.IncomTax,
                LoanDeduction = paySlipInfo.LoanDeduction,
                OtherDeductions = paySlipInfo.OtherDeductions,
                SalaryDate = paySlipInfo.SalaryDate,
                EmployeeInfoId = paySlipInfo.EmployeeInfoId,
                CreatedDate = paySlipInfo.CreatedDate,
                CreatedByAccountId = paySlipInfo.CreatedByAccountId,
                ModifiedDate = paySlipInfo.ModifiedDate,
                ModifiedByAccountId = paySlipInfo.ModifiedByAccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

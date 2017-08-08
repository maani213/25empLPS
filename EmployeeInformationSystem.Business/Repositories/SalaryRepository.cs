using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class SalaryRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public SalaryRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public SalaryRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<SalaryInfo> GetSalaryList()
        {
            //return (from employeeInfo in _context.EmployeeInfoes.ToList()
            //        join salary in _context.Salaries.ToList()
            //        on employeeInfo.Id equals salary.EmployeeInfoId

            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    join salary in _context.Salaries.ToList()
                    on employeeInfo.Id equals salary.EmployeeInfoId
                    where role.RoleName != "SuperAdmin" && account.IsActive == true
                    select new SalaryInfo
                    {
                        Id = salary.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        TotalSalary = salary.TotalSalary,
                        BasicSalary = salary.BasicSalary,
                        HouseRent = salary.HouseRent,
                        MedicalAllowance = salary.MedicalAllowance,
                        IncomTax = salary.IncomTax,
                        LoanDeduction = salary.LoanDeduction,
                        OtherDeductions = salary.OtherDeductions,
                        EmployeeInfoId = salary.EmployeeInfoId,
                        CreatedDate = salary.CreatedDate,
                        CreatedByAccountId = salary.CreatedByAccountId,
                        ModifiedDate = salary.ModifiedDate,
                        ModifiedByAccountId = salary.ModifiedByAccountId

                    }).ToList();

        }

        public SalaryInfo GetSalaryById(int id)
        {
            return (from employeeInfo in _context.EmployeeInfoes.ToList()
                    join salary in _context.Salaries.ToList()
                    on employeeInfo.Id equals salary.EmployeeInfoId
                    where salary.Id == id
                    select new SalaryInfo
                    {
                        Id = salary.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        TotalSalary = salary.TotalSalary,
                        BasicSalary = salary.BasicSalary,
                        HouseRent = salary.HouseRent,
                        MedicalAllowance = salary.MedicalAllowance,
                        IncomTax = salary.IncomTax,
                        LoanDeduction = salary.LoanDeduction,
                        OtherDeductions = salary.OtherDeductions,
                        EmployeeInfoId = salary.EmployeeInfoId,
                        CreatedDate = salary.CreatedDate,
                        CreatedByAccountId = salary.CreatedByAccountId,
                        ModifiedDate = salary.ModifiedDate,
                        ModifiedByAccountId = salary.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public SalaryInfo GetSalaryByEmployeeId(int id)
        {
            return (from employeeInfo in _context.EmployeeInfoes.ToList()
                    join salary in _context.Salaries.ToList()
                    on employeeInfo.Id equals salary.EmployeeInfoId
                    where salary.EmployeeInfoId == id
                    select new SalaryInfo
                    {
                        Id = salary.Id,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        TotalSalary = salary.TotalSalary,
                        BasicSalary = salary.BasicSalary,
                        HouseRent = salary.HouseRent,
                        MedicalAllowance = salary.MedicalAllowance,
                        IncomTax = salary.IncomTax,
                        LoanDeduction = salary.LoanDeduction,
                        OtherDeductions = salary.OtherDeductions,
                        EmployeeInfoId = salary.EmployeeInfoId,
                        CreatedDate = salary.CreatedDate,
                        CreatedByAccountId = salary.CreatedByAccountId,
                        ModifiedDate = salary.ModifiedDate,
                        ModifiedByAccountId = salary.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public void SaveSalary(SalaryInfo salaryInfo)
        {
            Data.Salary salary = ConvertToDb(salaryInfo);

            _context.Salaries.Add(salary);
            _context.SaveChanges();

        }

        public void UpdateSalary(SalaryInfo salaryInfo)
        {
            Data.Salary salary = _context.Salaries.Find(salaryInfo.Id);

            if (salary != null)
            {
                salary.TotalSalary = salaryInfo.TotalSalary;
                salary.BasicSalary = salaryInfo.BasicSalary;
                salary.HouseRent = salaryInfo.HouseRent;
                salary.MedicalAllowance = salaryInfo.MedicalAllowance;
                salary.IncomTax = salaryInfo.IncomTax;
                salary.LoanDeduction = salaryInfo.LoanDeduction;
                salary.OtherDeductions = salaryInfo.OtherDeductions;
                salary.ModifiedDate = salaryInfo.ModifiedDate;
                salary.ModifiedByAccountId = salaryInfo.ModifiedByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public bool IsDeleteSalary(int employeeId)
        {
            Data.Salary salary = _context.Salaries.Find(employeeId);

            _context.Salaries.Remove(salary);

            var result = _context.SaveChanges();

            if (result > 0)
                return true;
            else
                return false;
        }

        public SalaryInfo ConvertToFacade(Data.Salary salary)
        {
            return new SalaryInfo
            {
                Id = salary.Id,
                TotalSalary = salary.TotalSalary,
                BasicSalary = salary.BasicSalary,
                HouseRent = salary.HouseRent,
                MedicalAllowance = salary.MedicalAllowance,
                IncomTax = salary.IncomTax,
                LoanDeduction = salary.LoanDeduction,
                OtherDeductions = salary.OtherDeductions,
                EmployeeInfoId = salary.EmployeeInfoId,
                CreatedDate = salary.CreatedDate,
                CreatedByAccountId = salary.CreatedByAccountId,
                ModifiedDate = salary.ModifiedDate,
                ModifiedByAccountId = salary.ModifiedByAccountId

            };
        }

        public Data.Salary ConvertToDb(SalaryInfo salaryInfo)
        {
            return new Data.Salary
            {
                Id = salaryInfo.Id,
                TotalSalary = salaryInfo.TotalSalary,
                BasicSalary = salaryInfo.BasicSalary,
                HouseRent = salaryInfo.HouseRent,
                MedicalAllowance = salaryInfo.MedicalAllowance,
                IncomTax = salaryInfo.IncomTax,
                LoanDeduction = salaryInfo.LoanDeduction,
                OtherDeductions = salaryInfo.OtherDeductions,
                EmployeeInfoId = salaryInfo.EmployeeInfoId,
                CreatedDate = salaryInfo.CreatedDate,
                CreatedByAccountId = salaryInfo.CreatedByAccountId,
                ModifiedDate = salaryInfo.ModifiedDate,
                ModifiedByAccountId = salaryInfo.ModifiedByAccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

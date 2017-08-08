using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class AuthRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public AuthRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public AuthRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public AuthenticatedUser IsAccountExist(LoginViewModel loginInfo)
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where account.CompanyEmail == loginInfo.CompanyEmail
                    select new AuthenticatedUser
                    {
                        AccountId = account.Id,
                        EmployeeInfoId = employeeInfo.Id,
                        Role = role.RoleName,
                        PasswordHash = account.PasswordHash,
                        Salt = account.Salt,
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        CompanyEmail = account.CompanyEmail,
                        LastLoginDate = account.LastLoginDate,
                        LastLoginIp = account.LastLoginIp,
                        IsActive = account.IsActive,
                        IsFirstTimeLogin = account.IsFirstTimeLogin,
                        IsCheckListCompleted = account.IsCheckListCompleted

                    }).FirstOrDefault();

        }

        public AuthenticatedUser GetAuthenticatedUserById(int employeeId)
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where employeeInfo.Id == employeeId
                    select new AuthenticatedUser
                    {
                        AccountId = account.Id,
                        EmployeeInfoId = employeeInfo.Id,
                        Role = role.RoleName,
                        PasswordHash = account.PasswordHash,
                        Salt = account.Salt,
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        CompanyEmail = account.CompanyEmail,
                        LastLoginDate = account.LastLoginDate,
                        LastLoginIp = account.LastLoginIp,
                        IsActive = account.IsActive,
                        IsFirstTimeLogin = account.IsFirstTimeLogin,
                        IsCheckListCompleted = account.IsCheckListCompleted

                    }).FirstOrDefault();

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class AccountRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public AccountRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public AccountRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<AccountInfo> GetEmployeesAccountList()
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    orderby account.IsActive descending
                    select new AccountInfo
                    {
                        Id = account.Id,
                        CompanyEmail = account.CompanyEmail,
                        PasswordHash = account.PasswordHash,
                        Salt = account.Salt,
                        RoleId = account.RoleId,
                        RoleName = role.RoleName,
                        CreatedDate = employeeInfo.CreatedDate,
                        LastLoginDate = account.LastLoginDate,
                        LastLoginIp = account.LastLoginIp,
                        IsActive = account.IsActive,
                        IsFirstTimeLogin = account.IsFirstTimeLogin,
                        IsCheckListCompleted = account.IsCheckListCompleted

                    }).ToList();
        }

        public AccountInfo GetEmployeeAccountById(int accountId)
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where account.Id == accountId
                    select new AccountInfo
                    {
                        Id = account.Id,
                        CompanyEmail = account.CompanyEmail,
                        PasswordHash = account.PasswordHash,
                        Salt = account.Salt,
                        RoleId = account.RoleId,
                        RoleName = role.RoleName,
                        CreatedDate = employeeInfo.CreatedDate,
                        LastLoginDate = account.LastLoginDate,
                        LastLoginIp = account.LastLoginIp,
                        IsActive = account.IsActive,
                        IsFirstTimeLogin = account.IsFirstTimeLogin,
                        IsCheckListCompleted = account.IsCheckListCompleted,
                        EmployeeId = employeeInfo.Id

                    }).FirstOrDefault();
        }

        public AccountInfo GetEmployeeAccountByCompanyEmail(string email)
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where account.CompanyEmail == email
                    select new AccountInfo
                    {
                        Id = account.Id,
                        CompanyEmail = account.CompanyEmail,
                        PasswordHash = account.PasswordHash,
                        Salt = account.Salt,
                        RoleId = account.RoleId,
                        RoleName = role.RoleName,
                        CreatedDate = employeeInfo.CreatedDate,
                        LastLoginDate = account.LastLoginDate,
                        LastLoginIp = account.LastLoginIp,
                        IsActive = account.IsActive,
                        IsFirstTimeLogin = account.IsFirstTimeLogin,
                        IsCheckListCompleted = account.IsCheckListCompleted,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).FirstOrDefault();
        }

        public List<RoleInfo> GetRoleList()
        {
            return (from r in _context.Roles.ToList()
                    where r.RoleName != "Anonymous" && r.RoleName != "SuperAdmin"
                    select new RoleInfo()
                    {
                        Id = r.Id,
                        RoleName = r.RoleName

                    }).ToList();
        }

        public AccountInfo GetAccountByid(int id)
        {
            Data.Account account = _context.Accounts.Find(id);

            if (account != null)
            {
                return ConvertToFacade(account);
            }
            else
            {
                AccountInfo accountInfo = new AccountInfo();
                accountInfo = null;

                return accountInfo;
            }
        }

        public void ChangeNewPassword(AccountInfo accountInfo)
        {
            Data.Account account = _context.Accounts.Find(accountInfo.Id);

            if (account != null)
            {
                account.PasswordHash = accountInfo.PasswordHash;
                account.Salt = accountInfo.Salt;
                account.IsFirstTimeLogin = accountInfo.IsFirstTimeLogin;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public int GetRoleIdByName(string roleName)
        {
            int _roleId = 0;

            _roleId = (from a in _context.Roles.ToList() where a.RoleName == roleName select a.Id).FirstOrDefault();
            return _roleId;
        }

        public bool IsEmailExist(string email)
        {
            int count = _context.Accounts.ToList().Where(x => x.CompanyEmail == email.ToLower()).Count();
            
            if(count > 0)
                return true;
            else
                return false;

        }

        public List<AccountInfo> GetAccounts()
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    where account.IsCheckListCompleted == true && account.IsActive == true
                    orderby account.IsActive descending
                    select new AccountInfo
                    {
                        Id = account.Id,
                        CompanyEmail = account.CompanyEmail,
                        PasswordHash = account.PasswordHash,
                        Salt = account.Salt,
                        RoleName = role.RoleName,

                    }).ToList();
        }

        public void CheckListCompleted(int accountId)
        {
            int _roleId = (from a in _context.Roles.ToList() where a.RoleName == "Employee" select a.Id).FirstOrDefault();

            Data.Account account = _context.Accounts.Find(accountId);
            if (account != null)
            {
                account.RoleId = _roleId;
                account.IsCheckListCompleted = true;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void UpdateAccountOnLogin(AccountInfo accountInfo)
        {
            Data.Account account = _context.Accounts.Find(accountInfo.Id);

            if (account != null)
            {
                account.LastLoginDate = accountInfo.LastLoginDate;
                account.LastLoginIp = accountInfo.LastLoginIp;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public int CreateAccount(AccountInfo accountInfo)
        {
            Data.Account account = ConvertToDb(accountInfo);

            _context.Accounts.Add(account);
            _context.SaveChanges();

            return account.Id;
        }

        public void UpdateAccount(AccountInfo accountInfo)
        {
            Data.Account account = _context.Accounts.Find(accountInfo.Id);

            if (account != null)
            {
                account.RoleId = accountInfo.RoleId;
                account.IsActive = accountInfo.IsActive;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public bool IsDeletedUnActivatedAccount(int id)
        {
            Data.Account account = _context.Accounts.Find(id);

            _context.Accounts.Remove(account);

            var result = _context.SaveChanges();

            if (result > 0)
                return true;
            else
                return false;
        }

        public string GetUserRoleByEmail(string companyEmail)
        {
            string role = (from r in _context.Roles.ToList()
                           join account in _context.Accounts.ToList()
                           on r.Id equals account.RoleId
                           where account.CompanyEmail == companyEmail
                           select r.RoleName).FirstOrDefault().ToString();

            return role;
        }

        public AccountInfo ConvertToFacade(Data.Account account)
        {
            return new AccountInfo
            {
                Id = account.Id,
                CompanyEmail = account.CompanyEmail,
                PasswordHash = account.PasswordHash,
                Salt = account.Salt,
                RoleId = account.RoleId,
                LastLoginDate = account.LastLoginDate,
                LastLoginIp = account.LastLoginIp,
                IsActive = account.IsActive,
                IsFirstTimeLogin = account.IsFirstTimeLogin,
                IsCheckListCompleted = account.IsCheckListCompleted

            };
        }

        public Data.Account ConvertToDb(AccountInfo accountInfo)
        {
            return new Data.Account
            {
                Id = accountInfo.Id,
                CompanyEmail = accountInfo.CompanyEmail,
                PasswordHash = accountInfo.PasswordHash,
                Salt = accountInfo.Salt,
                RoleId = accountInfo.RoleId,
                LastLoginDate = accountInfo.LastLoginDate,
                LastLoginIp = accountInfo.LastLoginIp,
                IsActive = accountInfo.IsActive,
                IsFirstTimeLogin = accountInfo.IsFirstTimeLogin,
                IsCheckListCompleted = accountInfo.IsCheckListCompleted

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

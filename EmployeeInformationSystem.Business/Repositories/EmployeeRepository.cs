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
    public class EmployeeRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public EmployeeRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public EmployeeRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public Model.EmployeeInfo GetEmployeeInfoByAccountId(int accountId)
        {
            return (from account in _context.Accounts.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where account.Id == accountId
                    select new Model.EmployeeInfo
                    {
                        Id = employeeInfo.Id,
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        FatherName = employeeInfo.FatherName,
                        DateOfBirth = employeeInfo.DateOfBirth,
                        CellNumber = employeeInfo.CellNumber,
                        DateOfLeave = employeeInfo.DateOfLeave,
                        MaritalStatus = employeeInfo.MaritalStatus,
                        ProbationPeriod = employeeInfo.ProbationPeriod,
                        ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber,
                        PersonalEmail = employeeInfo.PersonalEmail,
                        CNIC = employeeInfo.CNIC,
                        PermanentAddress = employeeInfo.PermanentAddress,
                        PresentAddress = employeeInfo.PresentAddress,
                        ExperienceYears = employeeInfo.TotalExperience.Split(',').First(),
                        ExperienceMonths = employeeInfo.TotalExperience.Split(',').Last(),
                        DateOfJoin = employeeInfo.DateOfJoin,
                        Position = employeeInfo.Position,
                        EmpCode = employeeInfo.EmpCode,
                        Team = employeeInfo.Team,
                        CompanyEmail = account.CompanyEmail,
                        AccountId = employeeInfo.AccountId,
                        CreatedDate = employeeInfo.CreatedDate,
                        CreatedByAccountId = employeeInfo.CreatedByAccountId,
                        ModifiedDate = employeeInfo.ModifiedDate,
                        ModifiedByAccountId = employeeInfo.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public Model.EmployeeInfo GetEmployeeInfoById(int employeeId)
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where employeeInfo.Id == employeeId
                    select new Model.EmployeeInfo
                    {
                        Id = employeeInfo.Id,
                        RoleName = role.RoleName,
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        FatherName = employeeInfo.FatherName,
                        DateOfBirth = employeeInfo.DateOfBirth,
                        CellNumber = employeeInfo.CellNumber,
                        DateOfLeave = employeeInfo.DateOfLeave,
                        MaritalStatus = employeeInfo.MaritalStatus,
                        ProbationPeriod = employeeInfo.ProbationPeriod,
                        ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber,
                        PersonalEmail = employeeInfo.PersonalEmail,
                        CNIC = employeeInfo.CNIC,
                        PermanentAddress = employeeInfo.PermanentAddress,
                        PresentAddress = employeeInfo.PresentAddress,
                        ExperienceYears = employeeInfo.TotalExperience.Split(',').First(),
                        ExperienceMonths = employeeInfo.TotalExperience.Split(',').Last(),
                        DateOfJoin = employeeInfo.DateOfJoin,
                        Position = employeeInfo.Position,
                        EmpCode = employeeInfo.EmpCode,
                        Team = employeeInfo.Team,
                        CompanyEmail = account.CompanyEmail,
                        AccountId = employeeInfo.AccountId,
                        CreatedDate = employeeInfo.CreatedDate,
                        CreatedByAccountId = employeeInfo.CreatedByAccountId,
                        ModifiedDate = employeeInfo.ModifiedDate,
                        ModifiedByAccountId = employeeInfo.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public List<Model.EmployeeInfo> GetAllEmployeesExceptSuperAdmin()
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where role.RoleName != "SuperAdmin" && account.IsActive == true
                    select new Model.EmployeeInfo
                    {
                        Id = employeeInfo.Id,
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        FatherName = employeeInfo.FatherName,
                        DateOfBirth = employeeInfo.DateOfBirth,
                        CellNumber = employeeInfo.CellNumber,
                        DateOfLeave = employeeInfo.DateOfLeave,
                        MaritalStatus = employeeInfo.MaritalStatus,
                        ProbationPeriod = employeeInfo.ProbationPeriod,
                        ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber,
                        PersonalEmail = employeeInfo.PersonalEmail,
                        CNIC = employeeInfo.CNIC,
                        PermanentAddress = employeeInfo.PermanentAddress,
                        PresentAddress = employeeInfo.PresentAddress,
                        ExperienceYears = employeeInfo.TotalExperience.Split(',').First(),
                        ExperienceMonths = employeeInfo.TotalExperience.Split(',').Last(),
                        DateOfJoin = employeeInfo.DateOfJoin,
                        Position = employeeInfo.Position,
                        EmpCode = employeeInfo.EmpCode,
                        Team = employeeInfo.Team,
                        CompanyEmail = account.CompanyEmail,
                        AccountId = employeeInfo.AccountId,
                        IsCheckListCompleted = account.IsCheckListCompleted,
                        CreatedDate = employeeInfo.CreatedDate,
                        CreatedByAccountId = employeeInfo.CreatedByAccountId,
                        ModifiedDate = employeeInfo.ModifiedDate,
                        ModifiedByAccountId = employeeInfo.ModifiedByAccountId

                    }).ToList();
        }

        public EmployeeRegistrationInfo GetRegisterEmployeeInfoById(int employeeId)
        {
            return (from account in _context.Accounts.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where employeeInfo.Id == employeeId && account.IsActive == true
                    select new EmployeeRegistrationInfo
                    {
                        AccountId = account.Id,
                        EmployeeInfoId = employeeInfo.Id,
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        FatherName = employeeInfo.FatherName,
                        DateOfBirth = employeeInfo.DateOfBirth,
                        CellNumber = employeeInfo.CellNumber,
                        ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber,
                        MaritalStatus = employeeInfo.MaritalStatus,
                        PersonalEmail = employeeInfo.PersonalEmail,
                        CNIC = employeeInfo.CNIC,
                        PermanentAddress = employeeInfo.PermanentAddress,
                        PresentAddress = employeeInfo.PresentAddress,
                        ExperienceYears = employeeInfo.TotalExperience.Split(',').First(),
                        ExperienceMonths = employeeInfo.TotalExperience.Split(',').Last()

                    }).FirstOrDefault();
        }

        public AdminInfoViewModel GetAdminInfoById(int employeeId)
        {
            return (from account in _context.Accounts.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where employeeInfo.Id == employeeId && account.IsActive == true
                    select new AdminInfoViewModel
                    {
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        DateOfBirth = employeeInfo.DateOfBirth,
                        CellNumber = employeeInfo.CellNumber,
                        ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber,
                        MaritalStatus = employeeInfo.MaritalStatus,
                        PersonalEmail = employeeInfo.PersonalEmail,
                        CNIC = employeeInfo.CNIC,
                        PermanentAddress = employeeInfo.PermanentAddress,
                        PresentAddress = employeeInfo.PresentAddress,
                        ExperienceYears = employeeInfo.TotalExperience.Split(',').First(),
                        ExperienceMonths = employeeInfo.TotalExperience.Split(',').Last()

                    }).FirstOrDefault();
        }

        public List<Model.EmployeeInfo> GetEmployeeInfoList()
        {
            return (from role in _context.Roles.ToList()
                    join account in _context.Accounts.ToList()
                    on role.Id equals account.RoleId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    where role.RoleName == "Employee" || role.RoleName == "Administrator" && account.IsCheckListCompleted == true && account.IsActive == true
                    select new Model.EmployeeInfo
                    {
                        Id = employeeInfo.Id,
                        FullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public int SaveEmployeeInfo(Model.EmployeeInfo employeeInfo)
        {
            Data.EmployeeInfo empInfo = ConvertToDb(employeeInfo);

            _context.EmployeeInfoes.Add(empInfo);
            _context.SaveChanges();

            return empInfo.Id;
        }

        public void RegisterEmployee(EmployeeRegistrationInfo _employeeRegInfo)
        {
            Data.EmployeeInfo _employee = _context.EmployeeInfoes.Find(_employeeRegInfo.EmployeeInfoId);

            if (_employee != null)
            {
                _employee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_employeeRegInfo.FirstName.ToLower());
                _employee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_employeeRegInfo.LastName.ToLower());
                _employee.FatherName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_employeeRegInfo.FatherName.ToLower());
                _employee.DateOfBirth = _employeeRegInfo.DateOfBirth;
                _employee.CellNumber = _employeeRegInfo.CellNumber;
                _employee.ResidencePhoneNumber = _employeeRegInfo.ResidencePhoneNumber;
                _employee.MaritalStatus = _employeeRegInfo.MaritalStatus;
                _employee.PersonalEmail = _employeeRegInfo.PersonalEmail.ToLower();
                _employee.CNIC = _employeeRegInfo.CNIC;
                _employee.PermanentAddress = _employeeRegInfo.PermanentAddress;
                _employee.PresentAddress = _employeeRegInfo.PresentAddress;
                _employee.TotalExperience = _employeeRegInfo.ExperienceYears + "," + _employeeRegInfo.ExperienceMonths;
                _employee.DateOfJoin = _employeeRegInfo.DateOfJoin;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void UpdateAdminIfo(AdminInfoViewModel _adminInfo)
        {
            Data.EmployeeInfo _employee = _context.EmployeeInfoes.Find(_adminInfo.EmployeeInfoId);

            if (_employee != null)
            {
                _employee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_adminInfo.FirstName.ToLower());
                _employee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_adminInfo.LastName.ToLower());
                _employee.DateOfBirth = _adminInfo.DateOfBirth;
                _employee.CellNumber = _adminInfo.CellNumber;
                _employee.ResidencePhoneNumber = _adminInfo.ResidencePhoneNumber;
                _employee.MaritalStatus = _adminInfo.MaritalStatus;
                _employee.PersonalEmail = _adminInfo.PersonalEmail.ToLower();
                _employee.CNIC = _adminInfo.CNIC;
                _employee.PermanentAddress = _adminInfo.PermanentAddress;
                _employee.PresentAddress = _adminInfo.PresentAddress;
                _employee.TotalExperience = _adminInfo.ExperienceYears + "," + _adminInfo.ExperienceMonths;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void UpdateEmployeeInfo(Model.EmployeeInfo employeeInfo)
        {
            Data.EmployeeInfo employee = _context.EmployeeInfoes.Find(employeeInfo.Id);

            if (employee != null)
            {
                employee.FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employeeInfo.FirstName.ToLower());
                employee.LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employeeInfo.LastName.ToLower());
                employee.FatherName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employeeInfo.FatherName.ToLower());
                employee.DateOfBirth = employeeInfo.DateOfBirth;
                employee.CellNumber = employeeInfo.CellNumber;
                employee.DateOfLeave = employeeInfo.DateOfLeave;
                employee.MaritalStatus = employeeInfo.MaritalStatus;
                employee.ProbationPeriod = employeeInfo.ProbationPeriod;
                employee.ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber;
                employee.PersonalEmail = employeeInfo.PersonalEmail.ToLower();
                employee.CNIC = employeeInfo.CNIC;
                employee.PermanentAddress = employeeInfo.PermanentAddress;
                employee.PresentAddress = employeeInfo.PresentAddress;
                employee.TotalExperience = employeeInfo.ExperienceYears + "," + employeeInfo.ExperienceMonths;
                employee.Position = employeeInfo.Position;
                employee.Team = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(employeeInfo.Team.ToLower());
                employee.EmpCode = employeeInfo.EmpCode;
                employee.DateOfJoin = employeeInfo.DateOfJoin;
                employee.ModifiedDate = employeeInfo.ModifiedDate;
                employee.ModifiedByAccountId = employeeInfo.ModifiedByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }


        public Model.EmployeeInfo ConvertToFacade(Data.EmployeeInfo employee)
        {
            return new Model.EmployeeInfo
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FatherName = employee.FatherName,
                DateOfBirth = employee.DateOfBirth,
                CellNumber = employee.CellNumber,
                DateOfLeave = employee.DateOfLeave,
                MaritalStatus = employee.MaritalStatus,
                ProbationPeriod = employee.ProbationPeriod,
                ResidencePhoneNumber = employee.ResidencePhoneNumber,
                PersonalEmail = employee.PersonalEmail,
                CNIC = employee.CNIC,
                PermanentAddress = employee.PermanentAddress,
                PresentAddress = employee.PresentAddress,
                ExperienceYears = employee.TotalExperience.Split(',').First(),
                ExperienceMonths = employee.TotalExperience.Split(',').Last(),
                DateOfJoin = employee.DateOfJoin,
                Position = employee.Position,
                EmpCode = employee.EmpCode,
                Team = employee.Team,
                AccountId = employee.AccountId,
                CreatedDate = employee.CreatedDate,
                CreatedByAccountId = employee.CreatedByAccountId,
                ModifiedDate = employee.ModifiedDate,
                ModifiedByAccountId = employee.ModifiedByAccountId

            };
        }

        public Data.EmployeeInfo ConvertToDb(Model.EmployeeInfo employeeInfo)
        {
            return new Data.EmployeeInfo
            {
                Id = employeeInfo.Id,
                FirstName = employeeInfo.FirstName,
                LastName = employeeInfo.LastName,
                FatherName = employeeInfo.FatherName,
                DateOfBirth = employeeInfo.DateOfBirth,
                CellNumber = employeeInfo.CellNumber,
                DateOfLeave = employeeInfo.DateOfLeave,
                MaritalStatus = employeeInfo.MaritalStatus,
                ProbationPeriod = employeeInfo.ProbationPeriod,
                ResidencePhoneNumber = employeeInfo.ResidencePhoneNumber,
                PersonalEmail = employeeInfo.PersonalEmail,
                CNIC = employeeInfo.CNIC,
                PermanentAddress = employeeInfo.PermanentAddress,
                PresentAddress = employeeInfo.PresentAddress,
                TotalExperience = employeeInfo.ExperienceYears + "," + employeeInfo.ExperienceMonths,
                DateOfJoin = employeeInfo.DateOfJoin,
                Position = employeeInfo.Position,
                EmpCode = employeeInfo.EmpCode,
                Team = employeeInfo.Team,
                AccountId = employeeInfo.AccountId,
                CreatedDate = employeeInfo.CreatedDate,
                CreatedByAccountId = employeeInfo.CreatedByAccountId,
                ModifiedDate = employeeInfo.ModifiedDate,
                ModifiedByAccountId = employeeInfo.ModifiedByAccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

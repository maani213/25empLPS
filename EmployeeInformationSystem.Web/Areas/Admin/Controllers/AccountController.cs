using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using EmployeeInformationSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class AccountController : AdminBaseController
    {
        // GET: Admin/Account/Manage
        public ActionResult Manage()
        {
            try
            {
                var _accountList = new List<AccountInfo>();

                using (AccountRepository Repo = new AccountRepository())
                {
                    _accountList = Repo.GetEmployeesAccountList();
                }

                if (CurrentUser.Role == "SuperAdmin")
                {
                    _accountList = (from x in _accountList where x.Id != CurrentUser.AccountId select x).ToList();
                }

                return View(_accountList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Manage"));
            }
        }

        // GET: Admin/Account/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Create"));
            }
        }

        // POST: Admin/Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AccountInfo accountInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var _employeeInfo = new Model.EmployeeInfo();
                    _employeeInfo.CreatedByAccountId = CurrentUser.AccountId;
                    _employeeInfo.CreatedDate = DateTime.Now;

                    int employeeInfoId;
                    int accountId;

                    string saltValue = RandomPassword.Generate(18, 20);
                    string password = RijndaelCrypt.EncryptPassword(RandomPassword.Generate(), saltValue);

                    accountInfo.CompanyEmail = accountInfo.CompanyEmail.ToLower();
                    accountInfo.PasswordHash = password;
                    accountInfo.Salt = saltValue;
                    accountInfo.LastLoginDate = DateTime.Now;
                    accountInfo.LastLoginIp = "";
                    accountInfo.IsActive = true;
                    accountInfo.IsFirstTimeLogin = true;

                    using (var transaction = new System.Transactions.TransactionScope())
                    {
                        using (AccountRepository Repo = new AccountRepository())
                        {
                            if (Repo.IsEmailExist(accountInfo.CompanyEmail) == true)
                            {
                                TempData["Msg"] = AlertMessageProvider.FailureMessage("Email already exist.");

                                return View();
                            }

                            int roleId = Repo.GetRoleIdByName("Anonymous");
                            accountInfo.RoleId = roleId;

                            accountId = Repo.CreateAccount(accountInfo);
                        }

                        using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                        {
                            var _accountCheckList = new AccountCheckListInfo(accountId);

                            Repo.SaveAccountCheckList(_accountCheckList);
                        }

                        using (EmployeeRepository Repo = new EmployeeRepository())
                        {
                            _employeeInfo.AccountId = accountId;

                            employeeInfoId = Repo.SaveEmployeeInfo(_employeeInfo);
                        }

                        using (LeaveAllowedRepository Repo = new LeaveAllowedRepository())
                        {
                            LeaveAllowedInfo _leaveAllowed = new LeaveAllowedInfo(0, 0, employeeInfoId, CurrentUser.AccountId);

                            Repo.SaveLeaveAllowed(_leaveAllowed);
                        }

                        using (SalaryRepository Repo = new SalaryRepository())
                        {
                            var _salaryInfo = new SalaryInfo(CurrentUser.AccountId, employeeInfoId);

                            Repo.SaveSalary(_salaryInfo);
                        }

                        using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                        {
                            var _familyMember = new FamilyMemberInfo
                            {
                                Name = "Self",
                                Relation = "Selef",
                                EmployeeInfoId = employeeInfoId
                            };

                            Repo.SaveFamilyMember(_familyMember);
                        }

                        transaction.Complete();
                    }

                    List<string> To = new List<string>() { accountInfo.CompanyEmail };
                    string Subject = "LPS Online Account Information";
                    var LoginUrl = Url.Action("Login", "Auth", new { Area = "" }, protocol: Request.Url.Scheme);

                    string Body = "Dear Employee, <br/><br/>" +
                            "Your account has been created.<br/>" +
                            "Please ensure to save the username and password written below:<br/><br/>" +
                            "Username: &nbsp; <b>" + accountInfo.CompanyEmail + "</b><br/>" +
                            "Password: &nbsp; <b>" + RijndaelCrypt.DecryptPassword(accountInfo.PasswordHash, accountInfo.Salt) + "</b><br/><br/>" +
                            "<a href='" + LoginUrl + "' target='_blank'>" + LoginUrl + "</a><br/>" +
                            "You can login to your account to use LPS online services.<br/><br/>" +
                            "Thanks,<br/>" +
                            "<b>Logic Powered Solutions</b>";

                    bool result = EmailSender.Send(Subject, Body, To);

                    if (result)
                    {
                        TempData["Msg"] = AlertMessageProvider.SuccessMessage("Account created, email has been sent to employee successfully.");
                    }
                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! email not sent, please try again later.");
                    }

                    return RedirectToAction("Manage", "Account");
                }

                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Create"));
            }
        }

        // GET: Admin/Account/Update
        public ActionResult Update(string id = "")
        {
            try
            {
                int _id;
                AccountInfo _accountInfo = null;

                if (!int.TryParse(id, out _id))
                {
                    return RedirectToAction("Manage", "Account");
                }

                using (AccountRepository Repo = new AccountRepository())
                {
                    _accountInfo = Repo.GetEmployeeAccountById(int.Parse(id));

                    if (_accountInfo == null)
                    {
                        return RedirectToAction("Manage", "Account");
                    }

                    _accountInfo.RolesList = Repo.GetRoleList();
                }

                if (CurrentUser.Role == "Administrator" && _accountInfo.RoleName == "Administrator" || _accountInfo.RoleName == "SuperAdmin")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("You don't have permission for this process.");

                    return RedirectToAction("Manage", "Account");
                }

                return View(_accountInfo);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Update"));
            }
        }

        // POST: Admin/Account/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(AccountInfo accountInfo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AccountInfo _accountInfo = null;

                    using (AccountRepository Repo = new AccountRepository())
                    {
                        _accountInfo = Repo.GetEmployeeAccountById(accountInfo.Id);
                    }

                    if (_accountInfo == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Manage", "Account");
                    }

                    if (CurrentUser.AccountId == accountInfo.Id)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("You can't update your own account.");

                        return RedirectToAction("Manage", "Account");
                    }

                    if (CurrentUser.Role == "Administrator" && _accountInfo.RoleName == "Administrator" || _accountInfo.RoleName == "SuperAdmin")
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("You don't have permission for this process.");

                        return RedirectToAction("Manage", "Account");
                    }

                    if (CurrentUser.Role != "SuperAdmin")
                    {
                        accountInfo.RoleId = _accountInfo.RoleId;
                    }

                    using (var transaction = new System.Transactions.TransactionScope())
                    {
                        using (EmployeeRepository Repo = new EmployeeRepository())
                        {
                            var _employee = new Model.EmployeeInfo();

                            _employee = Repo.GetEmployeeInfoByAccountId(accountInfo.Id);

                            if (accountInfo.IsActive == false)
                            {
                                _employee.DateOfLeave = DateTime.Now;
                            }
                            else
                            {
                                _employee.DateOfLeave = null;
                            }

                            _employee.ModifiedByAccountId = CurrentUser.AccountId;
                            _employee.ModifiedDate = DateTime.Now;

                            Repo.UpdateEmployeeInfo(_employee);
                        }

                        using (AccountRepository Repo = new AccountRepository())
                        {
                            Repo.UpdateAccount(accountInfo);
                        }

                        transaction.Complete();
                    }

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Account updated successfully.");

                    return RedirectToAction("Manage", "Account");
                }

                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Update"));
            }
        }

        // POST: Admin/Account/ResendEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResendEmail(int id)
        {
            try
            {
                AccountInfo _accountInfo = null;

                using (AccountRepository Repo = new AccountRepository())
                {
                    _accountInfo = Repo.GetAccountByid(id);
                }

                if (_accountInfo == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! email not sent, please try again later.");

                    return RedirectToAction("Manage", "Account");
                }

                if (_accountInfo.IsCheckListCompleted == true)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("This account has been already activated.");

                    return RedirectToAction("Manage", "Account");
                }

                List<string> To = new List<string>() { _accountInfo.CompanyEmail };
                string Subject = "LPS Online Account Information";
                var LoginUrl = Url.Action("Login", "Auth", new { Area = "" }, protocol: Request.Url.Scheme);

                string Body = "Dear Employee, <br/><br/>" +
                        "Your account has been created.<br/>" +
                        "Please ensure to save the username and password written below:<br/><br/>" +
                        "Username: &nbsp; <b>" + _accountInfo.CompanyEmail + "</b><br/>" +
                        "Password: &nbsp; <b>" + RijndaelCrypt.DecryptPassword(_accountInfo.PasswordHash, _accountInfo.Salt) + "</b><br/><br/>" +
                        "<a href='" + LoginUrl + "' target='_blank'>" + LoginUrl + "</a><br/>" +
                        "You can login to your account to use LPS online services.<br/><br/>" +
                        "Thanks,<br/>" +
                        "<b>Logic Powered Solutions</b>";

                bool result = EmailSender.Send(Subject, Body, To);

                if (result)
                {
                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("Email has been sent to employee successfully.");
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! email not sent, please try again later.");
                }

                return RedirectToAction("Manage");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "ResendEmail"));
            }
        }

        // POST: Admin/Account/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                AccountInfo _accountInfo = null;

                using (AccountRepository Repo = new AccountRepository())
                {
                    _accountInfo = Repo.GetEmployeeAccountById(id);

                    if (_accountInfo == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! please try again later.");

                        return RedirectToAction("Manage");
                    }

                    if (_accountInfo.IsCheckListCompleted == true)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Unable to delete activated account.");

                        return RedirectToAction("Manage", "Account");
                    }

                    var result = Repo.IsDeletedUnActivatedAccount(id);

                    if(result == true)
                        TempData["Msg"] = AlertMessageProvider.SuccessMessage("Account has been deleted successfully.");
                    else
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! please try again later.");

                }

                return RedirectToAction("Manage", "Account");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Account", "Delete"));
            }
        }
    }
}
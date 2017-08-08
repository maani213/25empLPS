using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.EmployeeReg.Controllers
{
    public class ProfileController : EmployeeRegBaseController
    {
        // GET: EmployeeReg/Profile/PersonalInfo
        public ActionResult PersonalInfo()
        {
            try
            {
                AuthenticatedUser _authUser;
                var _registration = new RegistrationViewModel();

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsFirstTimeLogin == true)
                {
                    return RedirectToAction("ResetPassword", "Account");
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _registration.employeeRegistrationInfo = Repo.GetRegisterEmployeeInfoById(CurrentUser.EmployeeInfoId);
                }

                using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                {
                    _registration.accountCheckListInfo = Repo.GetAccountCheckListByUserId(CurrentUser.AccountId);
                }

                if(_registration.employeeRegistrationInfo.MaritalStatus == "Married")
                {
                    using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                    {
                        _registration.familyMembersList = Repo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);
                    }
                }

                return View(_registration);
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/Profile/PersonalInfo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalInfo(EmployeeRegistrationInfo _employeeRegInfo)
        {
            try
            {
                AuthenticatedUser _authUser;

                if (!ModelState.IsValid)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("PersonalInfo", "Profile");
                }

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsFirstTimeLogin == true)
                {
                    return RedirectToAction("ResetPassword", "Account");
                }

                if (_authUser.IsCheckListCompleted == true)
                {
                    return RedirectToAction("Logout", "Auth", new { area = "" });
                }

                _employeeRegInfo.EmployeeInfoId = CurrentUser.EmployeeInfoId;

                using (var transaction = new System.Transactions.TransactionScope())
                {
                    using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                    {
                        var _familyMembersList = Repo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);

                        if (_employeeRegInfo.MaritalStatus == "Single" && _familyMembersList.Count() > 0)
                        {
                            foreach(var item in _familyMembersList)
                            {
                                Repo.DeleteFamilyMember(item.Id);
                            }
                        }
                    }

                    using (EmployeeRepository Repo = new EmployeeRepository())
                    {
                        Repo.RegisterEmployee(_employeeRegInfo);
                    }

                    using (AccountCheckListRepository Repo = new AccountCheckListRepository())
                    {
                        var _accountCheckList = new AccountCheckListInfo();

                        _accountCheckList.IsPersonalInfoProvided = true;
                        _accountCheckList.AccountId = CurrentUser.AccountId;

                        Repo.UpdateIsPersonalInfoProvided(_accountCheckList);
                    }

                    transaction.Complete();
                }

                if(_employeeRegInfo.MaritalStatus == "Married")
                    return RedirectToAction("Details", "Family");
                else
                    return RedirectToAction("UploadDocuments", "File");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }
    }
}
using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using EmployeeInformationSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.EmployeeReg.Controllers
{
    public class AccountController : EmployeeRegBaseController
    {
        // GET: EmployeeReg/Account/ResetPassword
        public ActionResult ResetPassword()
        {
            try
            {
                AuthenticatedUser _authUser;

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsFirstTimeLogin == false)
                {
                    return RedirectToAction("PersonalInfo", "Profile");
                }

                return View();
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetNewPasswordViewModel _passwordInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                AuthenticatedUser _authUser;

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
                }

                if (_authUser.IsFirstTimeLogin == false)
                {
                    return RedirectToAction("GeneralInfo", "Profile");
                }

                if (RijndaelCrypt.DecryptPassword(_authUser.PasswordHash, _authUser.Salt) == _passwordInfo.NewPassword)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("New password should not be same as current password.");

                    return View();
                }

                var _accountInfo = new AccountInfo();
                _accountInfo.Id = CurrentUser.AccountId;
                _accountInfo.Salt = RandomPassword.Generate(18, 20);
                _accountInfo.PasswordHash = RijndaelCrypt.EncryptPassword(_passwordInfo.NewPassword, _accountInfo.Salt);
                _accountInfo.IsFirstTimeLogin = false;

                using (AccountRepository Repo = new AccountRepository())
                {
                    Repo.ChangeNewPassword(_accountInfo);
                }

                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut("ApplicationCookie");

                TempData["Msg"] = "<span style='color:green; text-align:center;'>Password has been reset successfully.</span>";
                return RedirectToAction("Login", "Auth", new { area = "" });
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }
    }
}
using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using EmployeeInformationSystem.Web.Helpers;

namespace EmployeeInformationSystem.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : AppController
    {
        // GET: Auth/Login
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                loginInfo.CompanyEmail = loginInfo.CompanyEmail.ToLower();
                AuthenticatedUser _authUser;

                using (AuthRepository Repo = new AuthRepository())
                {
                    _authUser = Repo.IsAccountExist(loginInfo);
                }

                if (_authUser != null)
                {
                    if (RijndaelCrypt.DecryptPassword(_authUser.PasswordHash, _authUser.Salt) == loginInfo.Password)
                    {
                        if (_authUser.IsActive == true)
                        {
                            using (AccountRepository Repo = new AccountRepository())
                            {
                                var _userAccount = new AccountInfo();

                                _userAccount.Id = _authUser.AccountId;
                                _userAccount.LastLoginIp = Request.UserHostAddress.ToString();
                                _userAccount.LastLoginDate = DateTime.Now;

                                Repo.UpdateAccountOnLogin(_userAccount);
                            }

                            string _mode = _authUser.Role == "SuperAdmin" ? "Admin" : "Employee";

                            var _identity = new ClaimsIdentity(new[] {
                                new Claim(ClaimTypes.Name, _authUser.FirstName + " " + _authUser.LastName),
                                new Claim(ClaimTypes.Email, _authUser.CompanyEmail),
                                new Claim(ClaimTypes.Role, _authUser.Role),
                                new Claim(ClaimTypes.NameIdentifier, _authUser.AccountId.ToString()),
                                new Claim("EmployeeInfoId", _authUser.EmployeeInfoId.ToString()),
                                new Claim("LastLoginDate", _authUser.LastLoginDate.ToString()),
                                new Claim("LastLoginIp", _authUser.LastLoginIp),
                                new Claim("Mode", _mode)

                            },
                            "ApplicationCookie");

                            var ctx = Request.GetOwinContext();
                            var authManager = ctx.Authentication;

                            authManager.SignIn(new AuthenticationProperties()
                            {
                                ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                                AllowRefresh = true,
                                IsPersistent = false

                            }, _identity);

                            if (_authUser.IsCheckListCompleted == false)
                            {
                                if (_authUser.IsFirstTimeLogin == true)
                                    return RedirectToAction("ResetPassword", "Account", new { Area = "EmployeeReg" });

                                return RedirectToAction("PersonalInfo", "Profile", new { Area = "EmployeeReg" });
                            }

                            return Redirect(GetRedirectUrl(loginInfo.ReturnUrl, _authUser.Role));
                        }
                        else
                        {
                            TempData["Msg"] = "<span style='color:red; text-align:center;'>Your account is deactive, you can't access online services.</span>";
                            return View();
                        }
                    }
                }

                TempData["Msg"] = "<span style='color:red; text-align:center;'>Invalid Username/Password.</span>";

                return View();

            }
            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        // GET: Auth/Logout
        public ActionResult Logout()
        {
            try
            {
                var ctx = Request.GetOwinContext();
                var authManager = ctx.Authentication;
                authManager.SignOut("ApplicationCookie");

                TempData["Msg"] = "<span style='color:green; text-align:center;'>You are logged off successfully.</span>";
                return RedirectToAction("Login", "Auth");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        // GET: Auth/ChangePassword
        public ActionResult ChangePassword()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Auth", "ChangePassword"));
            }
        }

        // POST: Auth/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel passwordInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var _account = new AccountInfo();

                using (AccountRepository Repo = new AccountRepository())
                {
                    _account = Repo.GetEmployeeAccountById(CurrentUser.AccountId);
                }

                string decryptedPassword = RijndaelCrypt.DecryptPassword(_account.PasswordHash, _account.Salt);

                if (decryptedPassword != passwordInfo.CurrentPassword)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Current password is invalid.");

                    return View();
                }

                if (decryptedPassword == passwordInfo.NewPassword)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("New password should not be same as current password.");

                    return View();
                }

                _account.Salt = RandomPassword.Generate(18, 20);
                _account.PasswordHash = RijndaelCrypt.EncryptPassword(passwordInfo.NewPassword, _account.Salt);

                using (AccountRepository Repo = new AccountRepository())
                {
                    Repo.ChangeNewPassword(_account);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Password changed successfully.");

                return View();
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Auth", "ChangePassword"));
            }
        }

        // GET: Auth/ForgotPassword
        public ActionResult ForgotPassword()
        {
            try
            {
                return View();
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        // POST: Auth/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgotInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                AccountInfo _account = null;
                string _token;

                using (AccountRepository Repo = new AccountRepository())
                {
                    _account = Repo.GetEmployeeAccountByCompanyEmail(forgotInfo.CompanyEmail);
                }

                if (_account == null)
                {
                    TempData["Msg"] = "<span style='color:red; text-align:center;'>Account does not associate with this email.</span>";

                    return RedirectToAction("ForgotPassword", "Auth");
                }

                if (_account.IsFirstTimeLogin == true)
                {
                    TempData["Msg"] = "<span style='color:red; text-align:center;'>You cannot reset password right now, please check your account creation email.</span>";

                    return RedirectToAction("ForgotPassword", "Auth");
                }

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();

                _token = Convert.ToBase64String(time.Concat(key).ToArray());

                using (AccountConfirmationRepository Repo = new AccountConfirmationRepository())
                {
                    AccountConfirmationInfo _accountConfirmation = null;

                    _accountConfirmation = Repo.GetAccountConfirmationByAccountId(_account.Id);

                    if (_accountConfirmation != null)
                    {
                        Repo.DeleteAccountConfirmation(_accountConfirmation.Id);
                    }

                    _accountConfirmation = new AccountConfirmationInfo();
                    _accountConfirmation.Token = _token;
                    _accountConfirmation.AccountId = _account.Id;

                    Repo.SaveAccountConfirmation(_accountConfirmation);
                }

                List<string> To = new List<string>() { _account.CompanyEmail };
                string Subject = "Password Reset Link";
                var resetPasswordUrl = Url.Action("Verify", "Auth", new { t = RijndaelCrypt.EncryptString(_token) }, protocol: Request.Url.Scheme);
                var forgotPasswordUrl = Url.Action("ForgotPassword", "Auth", null, protocol: Request.Url.Scheme);

                string Body = "Dear " + _account.EmployeeFullName + ", <br/><br/>" +
                        "We heard that you lost your LPS online account password. Sorry about that! <br/><br/>" +
                        "But don’t worry! You can use the following link within the next day to reset your password: <br/><br/>" +
                        "<a href='" + resetPasswordUrl + "' target='_blank'>" + resetPasswordUrl + "</a> <br/><br/>" +
                        "If you don’t use this link within 24 hours, it will expire. To get a new password reset link, visit<br/>" +
                        "<a href='" + forgotPasswordUrl + "' target='_blank'>" + forgotPasswordUrl + " </a> <br/><br/>" +
                        "Thanks,<br/>" +
                        "Logic Powered Solutions";

                bool result = EmailSender.Send(Subject, Body, To);

                if (result)
                {
                    TempData["Msg"] = "<span style='color:green; text-align:center;'>Request launched, for further processing please check your email.</span>";
                }
                else
                {
                    TempData["Msg"] = "<span style='color:red; text-align:center;'>Something went wrong! email not sent, please try again later.</span>";
                }

                return RedirectToAction("ForgotPassword", "Auth");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        // GET: Auth/Verify
        public ActionResult Verify(string t)
        {
            try
            {
                AccountConfirmationInfo _accountConfirmation = null;

                string _token = RijndaelCrypt.DecryptString(t);

                using (AccountConfirmationRepository Repo = new AccountConfirmationRepository())
                {
                    _accountConfirmation = Repo.GetAccountConfirmationByToken(_token);
                }

                if (_accountConfirmation == null)
                {
                    TempData["Msg"] = "Link has been already used or invalid.";

                    return View();
                    // invalid token
                }

                byte[] data = Convert.FromBase64String(_token);
                DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));

                if (when < DateTime.UtcNow.AddHours(-24))
                {
                    TempData["Msg"] = "Link has been expired.";

                    return View();
                    // expired token
                }
                else
                {
                    TempData["AccountId"] = _accountConfirmation.AccountId;
                    TempData["IsVerified"] = true;

                    return RedirectToAction("ResetPassword");
                    // valid token
                }
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        // GET: Auth/Verification
        public ActionResult ResetPassword()
        {
            try
            {
                //string str = TempData.Peek("AccountId").ToString();

                if (TempData["IsVerified"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                //TempData["AccountId"] = (int)TempData["AccountId"];

                return View();
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        // GET: Auth/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetNewPasswordViewModel passwordInfo)
        {
            try
            {
                if (TempData["AccountId"] == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!ModelState.IsValid)
                {
                    return View();
                }

                int _accountId = (int)TempData["AccountId"];
                var _accountInfo = new AccountInfo();

                _accountInfo.Id = _accountId;

                _accountInfo.Salt = RandomPassword.Generate(18, 20);
                _accountInfo.PasswordHash = RijndaelCrypt.EncryptPassword(passwordInfo.NewPassword, _accountInfo.Salt);

                _accountInfo.IsFirstTimeLogin = false;

                using (AccountRepository Repo = new AccountRepository())
                {
                    Repo.ChangeNewPassword(_accountInfo);
                }

                using (AccountConfirmationRepository Repo = new AccountConfirmationRepository())
                {
                    AccountConfirmationInfo _accountConfirmation = null;

                    _accountConfirmation = Repo.GetAccountConfirmationByAccountId(_accountId);

                    if (_accountConfirmation != null)
                    {
                        Repo.DeleteAccountConfirmation(_accountConfirmation.Id);
                    }
                }

                TempData["Msg"] = "<span style='color:green; text-align:center;'>Password has been reset successfully.</span>";

                return RedirectToAction("Login", "Auth");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = "<span style='color:red; text-align:center;'>" + ex.Message.ToString() + ".</span>";
                return View();
            }
        }

        private string GetRedirectUrl(string returnUrl, string role)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
            {
                return role == "Employee" || role == "Administrator" ? Url.Action("Dashboard", "Home", new { Area = "Employee" }) : Url.Action("Dashboard", "Home", new { Area = "Admin" });
            }

            return returnUrl;
        }
    }
}
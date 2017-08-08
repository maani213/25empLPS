using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.EmployeeReg.Controllers
{
    public class FamilyController : EmployeeRegBaseController
    {
        // GET: EmployeeReg/Family/Details
        public ActionResult Details()
        {
            AuthenticatedUser _authUser;
            var _registration = new RegistrationViewModel();
            _registration.familyMembersList = new List<FamilyMemberInfo>();

            using (AuthRepository Repo = new AuthRepository())
            {
                _authUser = Repo.GetAuthenticatedUserById(CurrentUser.EmployeeInfoId);
            }

            if (_authUser.IsCheckListCompleted == true)
            {
                return RedirectToAction("Logout", "Auth", new { area = "" });
            }

            using (AccountCheckListRepository Repo = new AccountCheckListRepository())
            {
                _registration.accountCheckListInfo = Repo.GetAccountCheckListByUserId(CurrentUser.AccountId);
            }

            if (_registration.accountCheckListInfo.IsPersonalInfoProvided == false)
            {
                return RedirectToAction("PersonalInfo", "Profile");
            }

            using (EmployeeRepository Repo = new EmployeeRepository())
            {
                _registration.employeeRegistrationInfo = Repo.GetRegisterEmployeeInfoById(CurrentUser.EmployeeInfoId);
            }

            if (_registration.employeeRegistrationInfo.MaritalStatus != "Married")
            {
                return RedirectToAction("UploadDocuments", "File");
            }

            using (FamilyMemberRepository Repo = new FamilyMemberRepository())
            {
                _registration.familyMembersList = Repo.GetFamilyMembersListByEmployeeId(CurrentUser.EmployeeInfoId);
            }

            return View(_registration);
        }

        // GET: EmployeeReg/Family/Add
        public ActionResult Add()
        {
            return View();
        }

        // POST: EmployeeReg/Family/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FamilyMemberInfo familyMemberInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Details", "Family");
                }

                if (familyMemberInfo.Relation != "Wife" && familyMemberInfo.Relation != "Son" && familyMemberInfo.Relation != "Daughter")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Family");
                }

                familyMemberInfo.EmployeeInfoId = CurrentUser.EmployeeInfoId;
                familyMemberInfo.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(familyMemberInfo.Name.ToLower());

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    Repo.SaveFamilyMember(familyMemberInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Family member added successfully.");

                return RedirectToAction("Details", "Family");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // GET: EmployeeReg/Family/Update
        public ActionResult Update(string id = "")
        {
            int _id;
            FamilyMemberInfo _familyMember = null;
            
            if (!int.TryParse(id, out _id))
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                return RedirectToAction("Details", "Family");
            }

            using (FamilyMemberRepository Repo = new FamilyMemberRepository())
            {
                _familyMember = Repo.GetFamilyMemberById(int.Parse(id));
            }

            if (_familyMember == null || _familyMember.EmployeeInfoId != CurrentUser.EmployeeInfoId)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                return RedirectToAction("Details", "Family");
            }

            return View(_familyMember);
        }

        // POST: EmployeeReg/Family/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FamilyMemberInfo familyMemberInfo)
        {
            try
            {
                int id;
                FamilyMemberInfo _familyMember = null;

                if (!int.TryParse(familyMemberInfo.Id.ToString(), out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Family");
                }

                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Details", "Family");
                }

                if (familyMemberInfo.Relation != "Wife" && familyMemberInfo.Relation != "Son" && familyMemberInfo.Relation != "Daughter")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Family");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    _familyMember = Repo.GetFamilyMemberById(familyMemberInfo.Id);

                    if (_familyMember == null || _familyMember.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Details", "Family");
                    }

                    Repo.UpdateFamilyMember(familyMemberInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Family member updated successfully.");

                return RedirectToAction("Details", "Family");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }

        // POST: EmployeeReg/Family/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string FamilyMemberId = "")
        {
            try
            {
                int id;
                FamilyMemberInfo _familyMember = null;

                if (!int.TryParse(FamilyMemberId, out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Details", "Family");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    _familyMember = Repo.GetFamilyMemberById(int.Parse(FamilyMemberId));

                    if (_familyMember == null || _familyMember.EmployeeInfoId != CurrentUser.EmployeeInfoId)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Details", "Family");
                    }

                    Repo.DeleteFamilyMember(int.Parse(FamilyMemberId));
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Family member deleted successfully.");

                return RedirectToAction("Details", "Family");
            }

            catch (Exception ex)
            {
                TempData["Msg"] = AlertMessageProvider.FailureMessage(ex.ToString());

                return View();
            }
        }
    }
}
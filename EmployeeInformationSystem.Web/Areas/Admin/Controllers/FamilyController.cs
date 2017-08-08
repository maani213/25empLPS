using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class FamilyController : Controller
    {
        // GET: Admin/Family/Add
        public ActionResult Add(string id = "")
        {
            try
            {
                int _id;
                var _familyMember = new FamilyMemberInfo();

                if (!int.TryParse(id, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                _familyMember.EmployeeInfoId = _id;

                return View(_familyMember);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Family", "Add"));
            }
        }

        // POST: Admin/Family/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(FamilyMemberInfo familyMemberInfo)
        {
            try
            {
                EmployeeInfo _employee = null;

                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Manage", "Profile");
                }

                if (familyMemberInfo.Relation != "Wife" && familyMemberInfo.Relation != "Son" && familyMemberInfo.Relation != "Daughter")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                familyMemberInfo.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(familyMemberInfo.Name.ToLower());

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(familyMemberInfo.EmployeeInfoId);

                    if (_employee == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Manage", "Profile");
                    }
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    Repo.SaveFamilyMember(familyMemberInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Family member added successfully.");

                return RedirectToAction("Manage", "Profile", new { id = familyMemberInfo.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Family", "Add"));
            }
        }

        // GET: Admin/Family/Update
        public ActionResult Update(string id = "")
        {
            try
            {
                int _id;
                FamilyMemberInfo _familyMember = null;

                if (!int.TryParse(id, out _id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    _familyMember = Repo.GetFamilyMemberById(int.Parse(id));
                }

                if (_familyMember == null)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                return View(_familyMember);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Family", "Update"));
            }
        }

        // POST: Admin/Family/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FamilyMemberInfo familyMemberInfo)
        {
            try
            {
                int id;
                FamilyMemberInfo _familyMember = null;

                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Manage", "Profile");
                }

                if (!int.TryParse(familyMemberInfo.Id.ToString(), out id))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                if (familyMemberInfo.Relation != "Wife" && familyMemberInfo.Relation != "Son" && familyMemberInfo.Relation != "Daughter")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Manage", "Profile");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    _familyMember = Repo.GetFamilyMemberById(familyMemberInfo.Id);

                    if (_familyMember == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Manage", "Profile");
                    }

                    Repo.UpdateFamilyMember(familyMemberInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Family member updated successfully.");

                return RedirectToAction("Manage", "Profile", new { id = familyMemberInfo.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Family", "Update"));
            }
        }

        // POST: Admin/Family/Delete
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

                    return RedirectToAction("Manage", "Profile");
                }

                using (FamilyMemberRepository Repo = new FamilyMemberRepository())
                {
                    _familyMember = Repo.GetFamilyMemberById(int.Parse(FamilyMemberId));

                    if (_familyMember == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Manage", "Profile");
                    }

                    Repo.DeleteFamilyMember(int.Parse(FamilyMemberId));
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Family member deleted successfully.");

                return RedirectToAction("Manage", "Profile", new { id = _familyMember.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Family", "Delete"));
            }
        }
    }
}
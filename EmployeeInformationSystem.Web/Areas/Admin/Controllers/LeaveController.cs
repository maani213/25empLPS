using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using EmployeeInformationSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Admin.Controllers
{
    public class LeaveController : AdminBaseController
    {
        // GET: Admin/Leave/Requests
        public ActionResult Requests()
        {
            try
            {
                var _leaveRequestList = new List<LeaveRequestInfo>();

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    _leaveRequestList = Repo.GetLeaveRequestList();
                }

                return View(_leaveRequestList);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Requests"));
            }
        }

        // GET: Admin/Leave/RequestDetails
        public ActionResult RequestDetails(string id = "")
        {
            try
            {
                int temp;

                if (!int.TryParse(id, out temp))
                {
                    return RedirectToAction("Requests", "Leave");
                }

                LeaveRequestInfo leaveRequest = null;

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    leaveRequest = Repo.GetLeaveRequestById(int.Parse(id));
                }

                if (leaveRequest == null)
                {
                    return RedirectToAction("Requests", "Leave");
                }

                return View(leaveRequest);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "RequestDetails"));
            }
        }

        // POST: Admin/Leave/RequestDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RequestDetails(LeaveRequestInfo leaveRequestInfo, string ProceedBtn = "")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("RequestDetails", "Leave", new { id = leaveRequestInfo.Id });
                }

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    LeaveRequestInfo leaveRequest = null;

                    leaveRequest = Repo.GetLeaveRequestById(leaveRequestInfo.Id);

                    if (leaveRequest == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! please try again later.");

                        return RedirectToAction("Requests", "Leave");
                    }

                    leaveRequest.Remark = leaveRequestInfo.Remark;
                    leaveRequest.RequestProcessByAccountId = CurrentUser.AccountId;
                    leaveRequest.RequestProcessDate = DateTime.Now;

                    bool isApproved = false;

                    if (ProceedBtn == "Approve")
                    {
                        leaveRequest.Status = "Approved";
                        Repo.ApproveLeaveRequest(leaveRequest);
                        isApproved = true;
                    }

                    else if (ProceedBtn == "Reject")
                    {
                        leaveRequest.Status = "Disapproved";
                        Repo.DeleteLeaveRequest(leaveRequest.Id);
                    }

                    else
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong! please try again later.");

                        return RedirectToAction("RequestDetails", "Leave", new { id = leaveRequest.Id });
                    }

                    string Subject = "Leave(s) Request Response";

                    string Body = "<b>Status: </b>" + leaveRequest.Status + ".<br/><br/>" +
                                   "<b>Remark: </b>" + leaveRequest.Remark + ".<br/><br/>" +
                                   "Thanks.<br/>";

                    var To = new List<string>() { leaveRequest.EmployeeCompanyEmail };

                    var status = EmailSender.Send(Subject, Body, To);

                    if (isApproved == true)
                        TempData["Msg"] = AlertMessageProvider.SuccessMessage("Leave request approved successfully.");
                    else
                        TempData["Msg"] = AlertMessageProvider.SuccessMessage("Leave request rejected successfully.");

                    return RedirectToAction("Requests", "Leave");
                }
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "RequestDetails"));
            }
        }

        // GET: Admin/Leave/Manage
        public ActionResult Manage(string id = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int empId;
                var _leaveViewModel = new LeaveViewModel();
                _leaveViewModel.LeaveRequestInfoList = new List<LeaveRequestInfo>();
                _leaveViewModel.LeaveRequestInfo = new LeaveRequestInfo();
                _leaveViewModel.CasualLeave = new LeaveInfo();
                _leaveViewModel.AnnualLeave = new LeaveInfo();

                var _leaveAllowed = new LeaveAllowedInfo();

                if (!int.TryParse(id, out empId))
                {
                    _leaveViewModel = null;

                    return View(_leaveViewModel);
                }

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    _leaveViewModel.CasualLeaveRequestInfoList = Repo.GetCasualLeaveRequestListByEmployeeId(empId);
                    _leaveViewModel.AnnualLeaveRequestInfoList = Repo.GetAnnualLeaveRequestListByEmployeeId(empId);

                    _leaveViewModel.LeaveRequestInfoList = Repo.GetLeaveRequestListByEmployeeId(empId);
                    _leaveViewModel.LeaveRequestInfo = Repo.GetLeaveRequestLastRecordByEmployeeId(empId);
                }

                using (LeaveAllowedRepository Repo = new LeaveAllowedRepository())
                {
                    _leaveAllowed = Repo.GetLeaveAllowedByEmployeeId(empId);
                }

                _leaveViewModel.CasualLeave.Allowed = _leaveAllowed.Casual;
                _leaveViewModel.CasualLeave.Availed = LeavesCounter.GetAvailedLeaves(_leaveViewModel.CasualLeaveRequestInfoList);
                _leaveViewModel.CasualLeave.Remaining = _leaveViewModel.CasualLeave.Allowed - _leaveViewModel.CasualLeave.Availed;

                _leaveViewModel.AnnualLeave.Allowed = _leaveAllowed.Annual;
                _leaveViewModel.AnnualLeave.Availed = LeavesCounter.GetAvailedLeaves(_leaveViewModel.AnnualLeaveRequestInfoList);
                _leaveViewModel.AnnualLeave.Remaining = _leaveViewModel.AnnualLeave.Allowed - _leaveViewModel.AnnualLeave.Availed;

                _leaveViewModel.EmployeeInfoId = id.ToString();

                return View(_leaveViewModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Manage"));
            }
        }

        // POST: Admin/Leave/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LeaveViewModel leave, string EmployeeInfoId = "")
        {
            try
            {
                ViewBag.EmployeeId = GetEmployeeFullNameList();

                int empId;
                var _leaveViewModel = new LeaveViewModel();
                _leaveViewModel.LeaveRequestInfoList = new List<LeaveRequestInfo>();

                if (!int.TryParse(EmployeeInfoId, out empId))
                {
                    _leaveViewModel = null;

                    return View(_leaveViewModel);
                }

                return RedirectToAction("Manage", "Leave", new { id = empId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Manage"));
            }
        }

        // GET: Admin/Leave/Add
        public ActionResult Add(string id = "")
        {
            try
            {
                int empId;
                var _leave = new LeaveRequestInfo();
                EmployeeInfo _employee = null;

                if (!int.TryParse(id, out empId))
                {
                    return RedirectToAction("Manage", "Leave");
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(empId);
                }

                if (_employee == null)
                {
                    return RedirectToAction("Manage", "Leave");
                }

                _leave.EmployeeInfoId = _employee.Id;
                _leave.EmployeeFullName = _employee.FirstName + " " + _employee.LastName;

                return View(_leave);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Add"));
            }
        }

        // POST: Admin/Leave/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(LeaveRequestInfo leaveRequestInfo, string LeaveDate = "")
        {
            try
            {
                string[] _dateRange = LeaveDate.Split(new string[] { " - " }, StringSplitOptions.None);
                DateTime _leaveStartDate;
                DateTime _leaveEndDate;

                if (!DateTime.TryParse(_dateRange[0], out _leaveStartDate) || !DateTime.TryParse(_dateRange[1], out _leaveEndDate))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid leave date, please select valid date.");

                    return RedirectToAction("Add", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
                }

                if (leaveRequestInfo.LeaveType != "Annual" && leaveRequestInfo.LeaveType != "Casual")
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid leave type, please select valid leave type.");

                    return RedirectToAction("Add", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
                }

                //if (_leaveStartDate.Date < DateTime.Now.Date)
                //{
                //    TempData["Msg"] = AlertMessageProvider.FailureMessage("Start date cannot be later than today.");

                //    return RedirectToAction("Add", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
                //}

                if (_leaveEndDate.Date < _leaveStartDate.Date)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Start date cannot be later than end date.");

                    return RedirectToAction("Add", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
                }

                int _totalSelectedLeaves = LeavesCounter.CountLeavesWithoutWeekend(_leaveStartDate, _leaveEndDate);

                if (_totalSelectedLeaves == 0)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Leave cannot add to saturday and sunday.");

                    return RedirectToAction("Add", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
                }

                leaveRequestInfo.RequestDate = DateTime.Now;
                leaveRequestInfo.StartDate = _leaveStartDate;
                leaveRequestInfo.EndDate = _leaveEndDate;
                leaveRequestInfo.Reason = "N/A";
                leaveRequestInfo.Status = "Approved";
                leaveRequestInfo.IsCreatedByAdmin = true;
                leaveRequestInfo.RequestProcessDate = DateTime.Now;
                leaveRequestInfo.RequestProcessByAccountId = CurrentUser.AccountId;

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    Repo.SaveLeaveRequest(leaveRequestInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Leave(s) add successfully.");

                return RedirectToAction("Manage", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Add"));
            }
        }

        // GET: Admin/Leave/Update
        public ActionResult Update(string id = "")
        {
            try
            {
                int temp;
                LeaveRequestInfo _leave = null;

                if (!int.TryParse(id, out temp))
                {
                    return RedirectToAction("Manage", "Leave");
                }

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    _leave = Repo.GetLeaveRequestById(int.Parse(id));
                }

                if (_leave == null)
                {
                    return RedirectToAction("Manage", "Leave");
                }

                _leave.Remark = "";

                return View(_leave);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Update"));
            }
        }

        // POST: Admin/Leave/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(LeaveRequestInfo leaveRequestInfo)
        {
            try
            {
                leaveRequestInfo.RequestProcessDate = DateTime.Now;
                leaveRequestInfo.RequestProcessByAccountId = CurrentUser.AccountId;

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    Repo.UpdateLeaveRequest(leaveRequestInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Leave(s) updated successfully.");

                return RedirectToAction("Manage", "Leave", new { id = leaveRequestInfo.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Update"));
            }
        }

        // POST: Admin/Leave/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string LeaveId = "", string EmployeeInfoId = "")
        {
            try
            {
                int _leaveId;
                int _employeeInfoId;
                LeaveRequestInfo _leave = null;

                if (!int.TryParse(LeaveId, out _leaveId) || !int.TryParse(EmployeeInfoId, out _employeeInfoId))
                {
                    return RedirectToAction("Manage", "Leave");
                }

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    _leave = Repo.GetLeaveRequestById(_leaveId);

                    if (_leave == null)
                    {
                        return RedirectToAction("Manage", "Leave");
                    }

                    Repo.DeleteLeaveRequest(_leaveId);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Leave(s) deleted successfully.");

                return RedirectToAction("Manage", "Leave", new { id = EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Delete"));
            }
        }

        // GET: Admin/Leave/Reset
        public ActionResult Reset(string id = "")
        {
            try
            {
                int empId;
                var _leavesAllowed = new LeaveAllowedInfo();

                EmployeeInfo _employee = null;

                //if (CurrentUser.Role != "SuperAdmin")
                //{
                //    TempData["Msg"] = AlertMessageProvider.FailureMessage("You don't have permission to access this feature.");

                //    return RedirectToAction("Manage", "Leave");
                //}

                if (!int.TryParse(id, out empId))
                {
                    return RedirectToAction("Manage", "Leave");
                }

                using (EmployeeRepository Repo = new EmployeeRepository())
                {
                    _employee = Repo.GetEmployeeInfoById(empId);
                }

                if (_employee == null)
                {
                    return RedirectToAction("Manage", "Leave");
                }

                using (LeaveAllowedRepository Repo = new LeaveAllowedRepository())
                {
                    _leavesAllowed = Repo.GetLeaveAllowedByEmployeeId(empId);
                }

                _leavesAllowed.EmployeeFullName = _employee.FirstName + " " + _employee.LastName;

                return View(_leavesAllowed);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Delete"));
            }
        }

        // POST: Admin/Leave/Reset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reset(LeaveAllowedInfo leaveAllowedInfo)
        {
            try
            {
                //if (CurrentUser.Role != "SuperAdmin")
                //{
                //    TempData["Msg"] = AlertMessageProvider.FailureMessage("You don't have permission to access this feature.");

                //    return RedirectToAction("Manage", "Leave");
                //}

                if (!ModelState.IsValid)
                {
                    return View();

                    //return RedirectToAction("Reset", "Leave", new { id = leaveAllowedInfo.EmployeeInfoId });
                }

                LeaveAllowedInfo leavesAllowed = null;

                using (LeaveAllowedRepository Repo = new LeaveAllowedRepository())
                {
                    leavesAllowed = Repo.GetLeaveAllowedByEmployeeId(leaveAllowedInfo.EmployeeInfoId);

                    if (leavesAllowed == null)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                        return RedirectToAction("Manage", "Leave");
                    }

                    if (leaveAllowedInfo.Casual > 5)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Maximum 15 casual leaves allowed.");

                        return RedirectToAction("Reset", "Leave", new { id = leaveAllowedInfo.EmployeeInfoId });
                    }

                    if (leaveAllowedInfo.Annual > 15)
                    {
                        TempData["Msg"] = AlertMessageProvider.FailureMessage("Maximum 30 annual leaves allowed.");

                        return RedirectToAction("Reset", "Leave", new { id = leaveAllowedInfo.EmployeeInfoId });
                    }

                    leaveAllowedInfo.Id = leavesAllowed.Id;
                    leaveAllowedInfo.ModifiedDate = DateTime.Now;
                    leaveAllowedInfo.ModifiedByAccountId = CurrentUser.AccountId;

                    Repo.UpdateLeaveAllowed(leaveAllowedInfo);
                }

                TempData["Msg"] = AlertMessageProvider.SuccessMessage("Leave(s) has been reset successfully.");

                return RedirectToAction("Manage", "Leave", new { id = leaveAllowedInfo.EmployeeInfoId });
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Update"));
            }
        }

        public SelectList GetEmployeeFullNameList()
        {
            using (EmployeeRepository Repo = new EmployeeRepository())
            {
                return new SelectList(Repo.GetEmployeeInfoList(), "Id", "FullName");
            }
        }

    }
}
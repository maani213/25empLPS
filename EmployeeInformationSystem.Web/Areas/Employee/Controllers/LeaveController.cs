using EmployeeInformationSystem.Business.Repositories;
using EmployeeInformationSystem.Model;
using EmployeeInformationSystem.Web.Common;
using EmployeeInformationSystem.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeInformationSystem.Web.Areas.Employee.Controllers
{
    public class LeaveController : EmployeeBaseController
    {
        // GET: Employee/Leave/Status
        public ActionResult Details()
        {
            try
            {
                var _leaveViewModel = new LeaveViewModel();
                _leaveViewModel.availedLeaveViewModel = new AvailedLeaveViewModel();
                _leaveViewModel.LeaveRequestInfo = new LeaveRequestInfo();
                _leaveViewModel.CasualLeave = new LeaveInfo();
                _leaveViewModel.AnnualLeave = new LeaveInfo();

                var _leaveAllowed = new LeaveAllowedInfo();

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    _leaveViewModel.CasualLeaveRequestInfoList = Repo.GetCasualLeaveRequestListByEmployeeId(CurrentUser.EmployeeInfoId);
                    _leaveViewModel.AnnualLeaveRequestInfoList = Repo.GetAnnualLeaveRequestListByEmployeeId(CurrentUser.EmployeeInfoId);

                    _leaveViewModel.LeaveRequestInfo = Repo.GetLeaveRequestLastRecordByEmployeeId(CurrentUser.EmployeeInfoId);

                    if (_leaveViewModel.LeaveRequestInfo == null)
                        _leaveViewModel.LeaveRequestInfo = new LeaveRequestInfo();

                    _leaveViewModel.availedLeaveViewModel.CasualLeaveRequestInfoList = _leaveViewModel.CasualLeaveRequestInfoList;
                    _leaveViewModel.availedLeaveViewModel.AnnualLeaveRequestInfoList = _leaveViewModel.AnnualLeaveRequestInfoList;
                }

                using (LeaveAllowedRepository Repo = new LeaveAllowedRepository())
                {
                    _leaveAllowed = Repo.GetLeaveAllowedByEmployeeId(CurrentUser.EmployeeInfoId);
                }

                _leaveViewModel.CasualLeave.Allowed = _leaveAllowed.Casual;
                _leaveViewModel.CasualLeave.Availed = LeavesCounter.GetAvailedLeaves(_leaveViewModel.CasualLeaveRequestInfoList);
                _leaveViewModel.CasualLeave.Remaining = _leaveViewModel.CasualLeave.Allowed - _leaveViewModel.CasualLeave.Availed;

                _leaveViewModel.AnnualLeave.Allowed = _leaveAllowed.Annual;
                _leaveViewModel.AnnualLeave.Availed = LeavesCounter.GetAvailedLeaves(_leaveViewModel.AnnualLeaveRequestInfoList);
                _leaveViewModel.AnnualLeave.Remaining = _leaveViewModel.AnnualLeave.Allowed - _leaveViewModel.AnnualLeave.Availed;

                if (_leaveViewModel.CasualLeave.Availed >= _leaveAllowed.Casual)
                {
                    _leaveViewModel.LeaveRequestInfo.IsCasualLeaveAvailed = true;
                }

                if (_leaveViewModel.AnnualLeave.Availed >= _leaveAllowed.Annual)
                {
                    _leaveViewModel.LeaveRequestInfo.IsAnnualLeaveAvailed = true;
                }

                return View(_leaveViewModel);
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "Details"));
            }
        }

        // POST: Employee/Leave/ApplyLeaves
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyLeaves(LeaveRequestInfo leaveRequestInfo)
        {
            try
            {
                if (leaveRequestInfo.LeaveType != "Casual" && leaveRequestInfo.LeaveType != "Annual" || string.IsNullOrEmpty(leaveRequestInfo.LeaveDate))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Something went wrong, please try again later.");

                    return RedirectToAction("Status", "Leave");
                }

                string[] _dateRange = leaveRequestInfo.LeaveDate.Split(new string[] { " - " }, StringSplitOptions.None);
                DateTime _leaveStartDate;
                DateTime _leaveEndDate;
                var _leaveAllowed = new LeaveAllowedInfo();

                if (!DateTime.TryParse(_dateRange[0], out _leaveStartDate) || !DateTime.TryParse(_dateRange[1], out _leaveEndDate))
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Invalid leave date, please select valid date.");

                    return RedirectToAction("Details", "Leave");
                }

                _leaveStartDate = DateTime.Parse(_dateRange[0]);
                _leaveEndDate = DateTime.Parse(_dateRange[1]);

                int _totalSelectedLeaves = LeavesCounter.CountLeavesWithoutWeekend(_leaveStartDate, _leaveEndDate);  //(_leaveStartDate - _leaveEndDate).Days + 1;

                if (_leaveStartDate.Date < DateTime.Now.Date)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Start date cannot be later than today.");

                    return RedirectToAction("Details", "Leave");
                }

                int _monthsDiff = ((_leaveStartDate.Year - DateTime.Now.Year) * 12) + _leaveStartDate.Month - DateTime.Now.Month;

                if (_monthsDiff > 1)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("You cannot apply for leaves earlier than 1 month.");

                    return RedirectToAction("Details", "Leave");
                }

                if (_leaveEndDate.Date < _leaveStartDate.Date)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Start date cannot be later than end date.");

                    return RedirectToAction("Details", "Leave");
                }

                if (_totalSelectedLeaves == 0)
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("You can't apply for saturday and sunday.");

                    return RedirectToAction("Details", "Leave");
                }

                using (LeaveAllowedRepository Repo = new LeaveAllowedRepository())
                {
                    _leaveAllowed = Repo.GetLeaveAllowedByEmployeeId(CurrentUser.EmployeeInfoId);
                }

                using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                {
                    LeaveRequestInfo _leaveRequest = null;
                    _leaveRequest = Repo.GetLeaveRequestLastRecordByEmployeeId(CurrentUser.EmployeeInfoId);

                    if (_leaveRequest != null)
                    {
                        if (_leaveRequest.EndDate.Date == _leaveStartDate.Date)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("You have already a leave today.");

                            return RedirectToAction("Details", "Leave");
                        }

                        if (_leaveRequest.Status == "Pending" || _leaveRequest.Status == "Approved" && _leaveRequest.EndDate.Date > DateTime.Now.Date)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("You have already applied for leaves.");

                            return RedirectToAction("Details", "Leave");
                        }
                    }

                    if (leaveRequestInfo.LeaveType == "Casual")
                    {


                        var _leavesList = Repo.GetCasualLeaveRequestListByEmployeeId(CurrentUser.EmployeeInfoId);
                        int _totalAvailedCasualLeaves = LeavesCounter.GetRemainingLeaves(_leavesList, _leaveAllowed.Casual);

                        if (_totalSelectedLeaves > _totalAvailedCasualLeaves)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select upto remaining casual leaves.");

                            return RedirectToAction("Details", "Leave");
                        }
                    }
                    else if (leaveRequestInfo.LeaveType == "Annual")
                    {
                        var _leavesList = Repo.GetAnnualLeaveRequestListByEmployeeId(CurrentUser.EmployeeInfoId);
                        int _totalAvailedAnnualLeaves = LeavesCounter.GetRemainingLeaves(_leavesList, _leaveAllowed.Annual);

                        if (_totalSelectedLeaves > _totalAvailedAnnualLeaves)
                        {
                            TempData["Msg"] = AlertMessageProvider.FailureMessage("Please select upto remaining annual leaves.");

                            return RedirectToAction("Details", "Leave");
                        }
                    }
                }

                leaveRequestInfo.RequestDate = DateTime.Now;
                leaveRequestInfo.StartDate = _leaveStartDate;
                leaveRequestInfo.EndDate = _leaveEndDate;
                leaveRequestInfo.Status = "Pending";
                leaveRequestInfo.Remark = "";
                leaveRequestInfo.IsCreatedByAdmin = false;
                leaveRequestInfo.EmployeeInfoId = CurrentUser.EmployeeInfoId;

                string Subject = "Leave(s) Request";
                string Body = "<b>Employee: </b>" + CurrentUser.Name + ".<br/>" +
                        "<b>Total Leaves: </b>" + _totalSelectedLeaves + ".<br/>" +
                        "<b>Leave Date: </b>" + leaveRequestInfo.LeaveDate + ".<br/><br/>" +
                        "<b>Leave Reason: </b><br/>" + leaveRequestInfo.Reason + "<br/><br/>" +
                        "Thanks.<br/>";

                var status = EmailSender.Send(Subject, Body);
                if (status)
                {
                    using (LeaveRequestRepository Repo = new LeaveRequestRepository())
                    {
                        Repo.SaveLeaveRequest(leaveRequestInfo);
                    }

                    TempData["Msg"] = AlertMessageProvider.SuccessMessage("You have applied for leaves successfully, please wait for approval.");
                }
                else
                {
                    TempData["Msg"] = AlertMessageProvider.FailureMessage("Email sending failed! please try again later.");
                }

                return RedirectToAction("Details", "Leave");
            }

            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Leave", "ApplyLeaves"));
            }
        }
    }
}
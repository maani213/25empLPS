using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class LeaveRequestRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public LeaveRequestRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public LeaveRequestRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<LeaveRequestInfo> GetLeaveRequestListByEmployeeId(int employeeId)
        {
            return (from leaveRequest in _context.LeaveRequests.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on leaveRequest.EmployeeInfoId equals employeeInfo.Id
                    where leaveRequest.EmployeeInfoId == employeeId && leaveRequest.Status == "Approved"
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<LeaveRequestInfo> GetCasualLeaveRequestListByEmployeeId(int employeeId)
        {
            return (from leaveRequest in _context.LeaveRequests.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on leaveRequest.EmployeeInfoId equals employeeInfo.Id
                    where leaveRequest.EmployeeInfoId == employeeId && leaveRequest.Status == "Approved" && leaveRequest.LeaveType == "Casual"
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<LeaveRequestInfo> GetAnnualLeaveRequestListByEmployeeId(int employeeId)
        {
            return (from leaveRequest in _context.LeaveRequests.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on leaveRequest.EmployeeInfoId equals employeeInfo.Id
                    where leaveRequest.EmployeeInfoId == employeeId && leaveRequest.Status == "Approved" && leaveRequest.LeaveType == "Annual"
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<LeaveRequestInfo> GetLeaveRequestList()
        {
            return (from leaveRequest in _context.LeaveRequests.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on leaveRequest.EmployeeInfoId equals employeeInfo.Id
                    where leaveRequest.Status == "Pending"
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public LeaveRequestInfo GetLeaveRequestById(int id)
        {
            return (from account in _context.Accounts.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on account.Id equals employeeInfo.AccountId
                    join leaveRequest in _context.LeaveRequests.ToList()
                    on employeeInfo.Id equals leaveRequest.EmployeeInfoId
                    where leaveRequest.Id == id
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName,
                        EmployeeCompanyEmail = account.CompanyEmail

                    }).FirstOrDefault();
        }

        public LeaveRequestInfo GetLeaveRequestByEmployeeId(int employeeId)
        {
            return (from leaveRequest in _context.LeaveRequests.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on leaveRequest.EmployeeInfoId equals employeeInfo.Id
                    where leaveRequest.EmployeeInfoId == employeeId
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).LastOrDefault();
        }

        public LeaveRequestInfo GetLeaveRequestLastRecordByEmployeeId(int employeeId)
        {
            return (from leaveRequest in _context.LeaveRequests.ToList()
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on leaveRequest.EmployeeInfoId equals employeeInfo.Id
                    where leaveRequest.EmployeeInfoId == employeeId && leaveRequest.IsCreatedByAdmin == false
                    select new LeaveRequestInfo
                    {
                        Id = leaveRequest.Id,
                        RequestDate = leaveRequest.RequestDate,
                        StartDate = leaveRequest.StartDate,
                        EndDate = leaveRequest.EndDate,
                        LeaveType = leaveRequest.LeaveType,
                        Reason = leaveRequest.Reason,
                        Status = leaveRequest.Status,
                        Remark = leaveRequest.Remark,
                        IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                        RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                        RequestProcessDate = leaveRequest.RequestProcessDate,
                        EmployeeInfoId = leaveRequest.EmployeeInfoId,
                        EmployeeFullName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).LastOrDefault();
        }

        public void SaveLeaveRequest(LeaveRequestInfo leaveRequestInfo)
        {
            Data.LeaveRequest leaveRequest = ConvertToDb(leaveRequestInfo);

            _context.LeaveRequests.Add(leaveRequest);
            _context.SaveChanges();

        }

        public void ApproveLeaveRequest(LeaveRequestInfo leaveRequestInfo)
        {
            Data.LeaveRequest leaveRequest = _context.LeaveRequests.Find(leaveRequestInfo.Id);

            if (leaveRequest != null)
            {
                leaveRequest.Status = leaveRequestInfo.Status;
                leaveRequest.RequestProcessDate = leaveRequestInfo.RequestProcessDate;
                leaveRequest.RequestProcessByAccountId = leaveRequestInfo.RequestProcessByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void UpdateLeaveRequest(LeaveRequestInfo leaveRequestInfo)
        {
            Data.LeaveRequest leaveRequest = _context.LeaveRequests.Find(leaveRequestInfo.Id);

            if (leaveRequest != null)
            {
                leaveRequest.EndDate = leaveRequestInfo.EndDate;
                leaveRequest.Remark = leaveRequestInfo.Remark;
                leaveRequest.RequestProcessDate = leaveRequestInfo.RequestProcessDate;
                leaveRequest.RequestProcessByAccountId = leaveRequestInfo.RequestProcessByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void DeleteLeaveRequest(int id)
        {
            Data.LeaveRequest leaveRequest = _context.LeaveRequests.Find(id);

            if (leaveRequest != null)
            {
                _context.LeaveRequests.Remove(leaveRequest);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public LeaveRequestInfo ConvertToFacade(Data.LeaveRequest leaveRequest)
        {
            return new LeaveRequestInfo
            {
                Id = leaveRequest.Id,
                RequestDate = leaveRequest.RequestDate,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                LeaveType = leaveRequest.LeaveType,
                Reason = leaveRequest.Reason,
                Status = leaveRequest.Status,
                Remark = leaveRequest.Remark,
                IsCreatedByAdmin = leaveRequest.IsCreatedByAdmin,
                RequestProcessDate = leaveRequest.RequestProcessDate,
                RequestProcessByAccountId = leaveRequest.RequestProcessByAccountId,
                EmployeeInfoId = leaveRequest.EmployeeInfoId

            };
        }

        public Data.LeaveRequest ConvertToDb(LeaveRequestInfo leaveRequestInfo)
        {
            return new Data.LeaveRequest
            {
                Id = leaveRequestInfo.Id,
                RequestDate = leaveRequestInfo.RequestDate,
                StartDate = leaveRequestInfo.StartDate,
                EndDate = leaveRequestInfo.EndDate,
                LeaveType = leaveRequestInfo.LeaveType,
                Reason = leaveRequestInfo.Reason,
                Status = leaveRequestInfo.Status,
                Remark = leaveRequestInfo.Remark,
                IsCreatedByAdmin = leaveRequestInfo.IsCreatedByAdmin,
                RequestProcessDate = leaveRequestInfo.RequestProcessDate,
                RequestProcessByAccountId = leaveRequestInfo.RequestProcessByAccountId,
                EmployeeInfoId = leaveRequestInfo.EmployeeInfoId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

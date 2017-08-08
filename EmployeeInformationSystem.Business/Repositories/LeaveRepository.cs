using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class LeaveRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public LeaveRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public LeaveRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public LeaveInfo GetCasualLeavesByEmployeeId(int employeeId)
        {
            return (from leave in _context.Leaves.ToList()
                    where leave.EmployeeInfoId == employeeId && leave.LeaveType == "Casual"
                    select new LeaveInfo
                    {
                        Id = leave.Id,
                        Allowed = leave.Allowed,
                        Availed = leave.Availed,
                        LeaveType = leave.LeaveType,
                        EmployeeInfoId = leave.EmployeeInfoId

                    }).FirstOrDefault();
        }

        public LeaveInfo GetAnnualLeavesByEmployeeId(int employeeId)
        {
            return (from leave in _context.Leaves.ToList()
                    where leave.EmployeeInfoId == employeeId && leave.LeaveType == "Annual"
                    select new LeaveInfo
                    {
                        Id = leave.Id,
                        Allowed = leave.Allowed,
                        Availed = leave.Availed,
                        LeaveType = leave.LeaveType,
                        EmployeeInfoId = leave.EmployeeInfoId

                    }).FirstOrDefault();
        }

        public void SaveLeave(LeaveInfo leaveInfo)
        {
            Data.Leave leave = ConvertToDb(leaveInfo);

            _context.Leaves.Add(leave);
            _context.SaveChanges();
        }

        public void UpdateLeave(LeaveInfo leaveInfo)
        {
            Data.Leave leave = _context.Leaves.Find(leaveInfo.Id);

            if (leave != null)
            {
                ConvertToDb(leaveInfo);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void AvailLeaves(LeaveInfo leaveInfo)
        {
            Data.Leave casualLeaves = (from l in _context.Leaves.ToList() where l.EmployeeInfoId == leaveInfo.EmployeeInfoId && l.LeaveType == "Casual" select l).FirstOrDefault();
            Data.Leave annualLeaves = (from l in _context.Leaves.ToList() where l.EmployeeInfoId == leaveInfo.EmployeeInfoId && l.LeaveType == "Annual" select l).FirstOrDefault();

            int remainingCasualLeaves = casualLeaves.Allowed - casualLeaves.Availed;

            if (remainingCasualLeaves == 0)
            {
                int availedAnnualLeaves = annualLeaves.Availed + leaveInfo.Availed;

                annualLeaves.Availed = availedAnnualLeaves;
                _context.SaveChanges();
            }

            else
            {
                if (remainingCasualLeaves > leaveInfo.Availed || remainingCasualLeaves == leaveInfo.Availed)
                {
                    int availedCasualLeaves = casualLeaves.Availed + leaveInfo.Availed;

                    casualLeaves.Availed = availedCasualLeaves;
                    _context.SaveChanges();
                }

                else if (remainingCasualLeaves < leaveInfo.Availed)
                {
                    int availedCasualLeaves = casualLeaves.Availed + remainingCasualLeaves;

                    casualLeaves.Availed = availedCasualLeaves;
                    _context.SaveChanges();

                    annualLeaves.Availed = annualLeaves.Availed + (leaveInfo.Availed - remainingCasualLeaves);
                    _context.SaveChanges();
                }
            }
        }

        public LeaveInfo ConvertToFacade(Data.Leave leave)
        {
            return new LeaveInfo
            {
                Id = leave.Id,
                Allowed = leave.Allowed,
                Availed = leave.Availed,
                LeaveType = leave.LeaveType,
                EmployeeInfoId = leave.EmployeeInfoId

            };
        }

        public Data.Leave ConvertToDb(LeaveInfo leaveInfo)
        {
            return new Data.Leave
            {
                Id = leaveInfo.Id,
                Allowed = leaveInfo.Allowed,
                Availed = leaveInfo.Availed,
                LeaveType = leaveInfo.LeaveType,
                EmployeeInfoId = leaveInfo.EmployeeInfoId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

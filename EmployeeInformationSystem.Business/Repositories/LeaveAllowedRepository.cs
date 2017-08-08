using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class LeaveAllowedRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public LeaveAllowedRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public LeaveAllowedRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }


        public LeaveAllowedInfo GetLeaveAllowedByEmployeeId(int id)
        {
            return (from leaveallowed in _context.LeaveAlloweds.ToList()
                    where leaveallowed.EmployeeInfoId == id
                    select new LeaveAllowedInfo
                    {
                        Id = leaveallowed.Id,
                        Casual = leaveallowed.Casual,
                        Annual = leaveallowed.Annual,
                        EmployeeInfoId = leaveallowed.EmployeeInfoId,
                        CreatedDate = leaveallowed.CreatedDate,
                        CreatedByAccountId = leaveallowed.CreatedByAccountId,
                        ModifiedDate = leaveallowed.ModifiedDate,
                        ModifiedByAccountId = leaveallowed.ModifiedByAccountId,

                    }).FirstOrDefault();
        }

        public void SaveLeaveAllowed(LeaveAllowedInfo leaveAllowedInfo)
        {
            Data.LeaveAllowed leaveAllowed = ConvertToDb(leaveAllowedInfo);

            _context.LeaveAlloweds.Add(leaveAllowed);
            _context.SaveChanges();

        }

        public void UpdateLeaveAllowed(LeaveAllowedInfo leaveAllowedInfo)
        {
            Data.LeaveAllowed leaveAllowed = _context.LeaveAlloweds.Find(leaveAllowedInfo.Id);

            if (leaveAllowed != null)
            {
                leaveAllowed.Casual = leaveAllowedInfo.Casual;
                leaveAllowed.Annual = leaveAllowedInfo.Annual;
                leaveAllowed.ModifiedDate = leaveAllowedInfo.ModifiedDate;
                leaveAllowed.ModifiedByAccountId = leaveAllowedInfo.ModifiedByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public bool IsDeleteLeaveAllowed(int employeeId)
        {
            Data.LeaveAllowed leaveAllowed = _context.LeaveAlloweds.Find(employeeId);

            _context.LeaveAlloweds.Remove(leaveAllowed);

            var result = _context.SaveChanges();

            if(result > 0)
                return true;
            else
                return false;
        }

        public LeaveAllowedInfo ConvertToFacade(Data.LeaveAllowed leaveAllowed)
        {
            return new LeaveAllowedInfo
            {
                Id = leaveAllowed.Id,
                Casual = leaveAllowed.Casual,
                Annual = leaveAllowed.Annual,
                EmployeeInfoId = leaveAllowed.EmployeeInfoId,
                CreatedDate = leaveAllowed.CreatedDate,
                CreatedByAccountId = leaveAllowed.CreatedByAccountId,
                ModifiedDate = leaveAllowed.ModifiedDate,
                ModifiedByAccountId = leaveAllowed.ModifiedByAccountId,

            };
        }

        public Data.LeaveAllowed ConvertToDb(LeaveAllowedInfo leaveAllowedInfo)
        {
            return new Data.LeaveAllowed
            {
                Id = leaveAllowedInfo.Id,
                Casual = leaveAllowedInfo.Casual,
                Annual = leaveAllowedInfo.Annual,
                EmployeeInfoId = leaveAllowedInfo.EmployeeInfoId,
                CreatedDate = leaveAllowedInfo.CreatedDate,
                CreatedByAccountId = leaveAllowedInfo.CreatedByAccountId,
                ModifiedDate = leaveAllowedInfo.ModifiedDate,
                ModifiedByAccountId = leaveAllowedInfo.ModifiedByAccountId,

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

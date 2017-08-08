using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeInformationSystem.Web.Common
{
    public class LeavesCounter
    {
        public static int CountLeavesWithoutWeekend(DateTime startDate, DateTime endDate)
        {
            int count = 0;

            TimeSpan diff = endDate - startDate;
            int days = diff.Days;
            for (var i = 0; i <= days; i++)
            {
                var testDate = startDate.AddDays(i);
                switch (testDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                        count--;
                        break;
                }
                count++;
            }

            return count;
        }

        public static int GetRemainingLeaves(List<LeaveRequestInfo> leavesList, int totalLeavesAllowed)
        {
            int _totalLeavesRemaining = 0;
            int _totalLeavesAvailed = 0;

            foreach (var item in leavesList)
            {
                _totalLeavesAvailed += CountLeavesWithoutWeekend(item.StartDate, item.EndDate);
            }

            _totalLeavesRemaining = (totalLeavesAllowed - _totalLeavesAvailed);

            return _totalLeavesRemaining;
        }

        public static int GetAvailedLeaves(List<LeaveRequestInfo> leavesList)
        {
            int _totalLeavesAvailed = 0;

            foreach (var item in leavesList)
            {
                _totalLeavesAvailed += CountLeavesWithoutWeekend(item.StartDate, item.EndDate);
            }

            return _totalLeavesAvailed;
        }
    }
}
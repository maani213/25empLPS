using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class LeaveRequestInfo
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.DateTime, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid date.</li></ul>")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.DateTime, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid date.</li></ul>")]
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public string LeaveDate { get; set; }
        public DateTime? RequestProcessDate { get; set; }
        public int? RequestProcessByAccountId { get; set; }
        public string EmployeeCompanyEmail { get; set; }
        public int EmployeeInfoId { get; set; }
        public string EmployeeFullName { get; set; }
        public bool IsCasualLeaveAvailed { get; set; }
        public bool IsAnnualLeaveAvailed { get; set; }
    }
}

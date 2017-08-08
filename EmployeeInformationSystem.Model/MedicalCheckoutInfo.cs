using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class MedicalCheckoutInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Treatement date is required.</li></ul>")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [DataType(DataType.DateTime, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid date.</li></ul>")]
        public DateTime? TreatmentDate { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Patient name is required.</li></ul>")]
        public int FamilyMemberId { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Amount is required.</li></ul>")]
        [Range(0, 100000, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Enter amount between 0 to 100000.</li></ul>")]
        [RegularExpression(@"^[0-9\.]*$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Amount must be a number.</li></ul>")]
        public decimal? Amount { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Description is required.</li></ul>")]
        [StringLength(400, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter description upto 400 characters.</li></ul>")]
        //[RegularExpression(@"^[\w\s.,!?€¥£¢$@#-]{0,290}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Illegal characters are not allowed.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid description.</li></ul>")]
        public string Description { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Prescriptions name is required.</li></ul>")]
        [StringLength(190, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter address upto 190 characters.</li></ul>")]
        //[RegularExpression(@"^[\w\s.,!?€¥£¢$@#-]{0,290}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Illegal characters are not allowed.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter file name.</li></ul>")]
        public string PrescriptionName { get; set; }

        public DateTime? RequestDate { get; set; }
        public string Status { get; set; }
        public bool? IsCreatedByAdmin { get; set; }
        public DateTime? RequestProcessDate { get; set; }
        public int? RequestProcessByAccountId { get; set; }
        public int EmployeeInfoId { get; set; }
        public string PatientName { get; set; }
        public string EmployeeName { get; set; }
        public List<MedicalCheckoutInfo> PendingMedicalsList { get; set; }
        public List<MedicalCheckoutInfo> AvailedMedicalsList { get; set; }
        public List<MedicalPrescriptionInfo> MedicalPrescriptions { get; set; }
        public MedicalAllowanceInfo MedicalAllowance { get; set; }
    }
}

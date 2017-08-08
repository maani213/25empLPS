using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class MedicalPrescriptionInfo
    {
        public int Id { get; set; }
        public string PrescriptionPath { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public int MedicalCheckoutId { get; set; }
        public int EmployeeInfoId { get; set; }
        public string CheckoutStatus { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EmployeeInformationSystem.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class MedicalCheckout
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MedicalCheckout()
        {
            this.MedicalPrescriptions = new HashSet<MedicalPrescription>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<System.DateTime> TreatmentDate { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Status { get; set; }
        public Nullable<bool> IsCreatedByAdmin { get; set; }
        public Nullable<System.DateTime> RequestProcessDate { get; set; }
        public Nullable<int> RequestProcessByAccountId { get; set; }
        public int EmployeeInfoId { get; set; }
        public int FamilyMemberId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalPrescription> MedicalPrescriptions { get; set; }
        public virtual Account Account { get; set; }
        public virtual EmployeeInfo EmployeeInfo { get; set; }
        public virtual FamilyMember FamilyMember { get; set; }
    }
}

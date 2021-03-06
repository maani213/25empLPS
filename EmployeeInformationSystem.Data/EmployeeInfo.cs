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
    
    public partial class EmployeeInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmployeeInfo()
        {
            this.Documents = new HashSet<Document>();
            this.Salaries = new HashSet<Salary>();
            this.FamilyMembers = new HashSet<FamilyMember>();
            this.LeaveAlloweds = new HashSet<LeaveAllowed>();
            this.LeaveRequests = new HashSet<LeaveRequest>();
            this.MedicalCheckouts = new HashSet<MedicalCheckout>();
            this.PaySlips = new HashSet<PaySlip>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string CellNumber { get; set; }
        public string ResidencePhoneNumber { get; set; }
        public string PersonalEmail { get; set; }
        public string CNIC { get; set; }
        public string MaritalStatus { get; set; }
        public string PermanentAddress { get; set; }
        public string PresentAddress { get; set; }
        public string TotalExperience { get; set; }
        public string ProbationPeriod { get; set; }
        public string Position { get; set; }
        public Nullable<int> EmpCode { get; set; }
        public string Team { get; set; }
        public Nullable<System.DateTime> DateOfJoin { get; set; }
        public Nullable<System.DateTime> DateOfLeave { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedByAccountId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedByAccountId { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
        public virtual Account Account2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Document> Documents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Salary> Salaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveAllowed> LeaveAlloweds { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MedicalCheckout> MedicalCheckouts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaySlip> PaySlips { get; set; }
    }
}

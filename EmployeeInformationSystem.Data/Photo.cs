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
    
    public partial class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int AlbumId { get; set; }
        public Nullable<System.DateTime> UploadOn { get; set; }
        public int UploadByAccountId { get; set; }
    
        public virtual Album Album { get; set; }
    }
}

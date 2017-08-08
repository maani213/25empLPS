using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class FamilyMemberInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Name is required.</li></ul>")]
        [StringLength(190, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter name upto 190 characters.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid name.</li></ul>")]
        public string Name { get; set; }

        [StringLength(13, MinimumLength = 13, ErrorMessage = "<ul class='parsley-errors-list filled'><li>CNIC must be 13 digits.</li></ul>")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter digits only.</li></ul>")]
        public string CNIC { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Select relation.</li></ul>")]
        [StringLength(90, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter relation upto 90 characters.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid relation.</li></ul>")]
        public string Relation { get; set; }

        public int EmployeeInfoId { get; set; }
    }
}

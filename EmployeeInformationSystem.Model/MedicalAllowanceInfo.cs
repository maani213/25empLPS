using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class MedicalAllowanceInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Category is required.</li></ul>")]
        [StringLength(290, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter category upto 290 characters.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter valid category.</li></ul>")]
        public string Category { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Amount is required.</li></ul>")]
        [Range(0, 100000, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Enter amount between 0 to 100000.</li></ul>")]
        [RegularExpression(@"^[0-9\.]*$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Amount must be a number.</li></ul>")]
        public decimal Amount { get; set; }
    }
}

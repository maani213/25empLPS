using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class DocumentInfo
    {
        public int Id { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Document name is required.</li></ul>")]
        [StringLength(190, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter address upto 190 characters.</li></ul>")]
        //[RegularExpression(@"^[\w\s.,!?€¥£¢$@#-]{0,290}$", ErrorMessage = "<ul class='parsley-errors-list filled'><li>Illegal characters are not allowed.</li></ul>")]
        [DataType(DataType.Text, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter file name.</li></ul>")]
        public string FileName { get; set; }

        public DateTime UploadDate { get; set; }
        public int EmployeeInfoId { get; set; }
    }
}

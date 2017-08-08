using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class AlbumInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "<ul class='parsley-errors-list filled'><li>Title is required.</li></ul>")]
        [StringLength(4000, ErrorMessage = "<ul class='parsley-errors-list filled'><li>Please enter name upto 400 characters.</li></ul>")]
        public string Title { get; set; }

        public string Description { get; set; }
        public string CoverPhotoPath { get; set; }
        public int NumberOfPhotos { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedByAccountId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedByAccountId { get; set; }
        public List<PhotoInfo> PhotoInfoList { get; set; }
    }
}

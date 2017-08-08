using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class PhotoInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public DateTime? UploadOn { get; set; }
        public int UploadByAccountId { get; set; }
    }
}

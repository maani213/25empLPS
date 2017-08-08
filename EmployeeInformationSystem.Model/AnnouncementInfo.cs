using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class AnnouncementInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedByAccountId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedByAccountId { get; set; }
    }
}

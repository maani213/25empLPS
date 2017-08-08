using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class EmployeeDocumentInfo
    {
        public int Id { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentType { get; set; }
        public int EmployeeInfoId { get; set; }
    }
}

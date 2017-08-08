using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class RegistrationViewModel
    {
        public AccountCheckListInfo accountCheckListInfo { get; set; }
        public EmployeeRegistrationInfo employeeRegistrationInfo { get; set; }
        public List<DocumentInfo> documentInfoList { get; set; }
        public List<FamilyMemberInfo> familyMembersList { get; set; }
    }
}

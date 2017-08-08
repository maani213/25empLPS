using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Model
{
    public class AccountCheckListInfo
    {
        public int Id { get; set; }
        public bool IsPersonalInfoProvided { get; set; }
        public bool IsFamilyDetailsProvided { get; set; }
        public bool IsDocumentsUploaded { get; set; }
        public bool IsPictureUploaded { get; set; }
        public int? AccountId { get; set; }

        public AccountCheckListInfo()
        {
            IsPersonalInfoProvided = false;
            IsDocumentsUploaded = false;
            IsPictureUploaded = false;
        }

        public AccountCheckListInfo(int? accountId)
        {
            IsPersonalInfoProvided = false;
            IsDocumentsUploaded = false;
            IsPictureUploaded = false;
            AccountId = accountId;
        }
    }
}

using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class AccountCheckListRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public AccountCheckListRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public AccountCheckListRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public AccountCheckListInfo GetAccountCheckListByUserId(int? id)
        {
         
            return (from ac in _context.AccountCheckLists.ToList()
                    where ac.AccountId == id
                    select new AccountCheckListInfo
                    {
                        AccountId=ac.AccountId,
                        IsPersonalInfoProvided = ac.IsPersonalInfoProvided,
                        IsDocumentsUploaded = ac.IsDocumentsUploaded,
                        IsPictureUploaded = ac.IsPictureUploaded

                    }).FirstOrDefault();
        }

        public int SaveAccountCheckList(AccountCheckListInfo accountCheckListInfo)
        {
            Data.AccountCheckList accountCheck = ConvertToDb(accountCheckListInfo);

            _context.AccountCheckLists.Add(accountCheck);
            _context.SaveChanges();

            return accountCheck.Id;
        }

        public void UpdateIsPersonalInfoProvided(AccountCheckListInfo accountCheckListInfo)
        {
            Data.AccountCheckList accountCheck = _context.AccountCheckLists.FirstOrDefault(x => x.AccountId == accountCheckListInfo.AccountId);

            if (accountCheck != null)
            {
                accountCheck.IsPersonalInfoProvided = accountCheckListInfo.IsPersonalInfoProvided;;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void UpdateIsDocumentsUploaded(AccountCheckListInfo accountCheckListInfo)
        {
            Data.AccountCheckList accountCheck = _context.AccountCheckLists.FirstOrDefault(x => x.AccountId == accountCheckListInfo.AccountId);

            if (accountCheck != null)
            {
                accountCheck.IsDocumentsUploaded = accountCheckListInfo.IsDocumentsUploaded;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void UpdateIsPictureUploaded(AccountCheckListInfo accountCheckListInfo)
        {
            Data.AccountCheckList accountCheck = _context.AccountCheckLists.FirstOrDefault(x => x.AccountId == accountCheckListInfo.AccountId);

            if (accountCheck != null)
            {
                accountCheck.IsPictureUploaded = accountCheckListInfo.IsPictureUploaded;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public AccountCheckListInfo ConvertToFacade(Data.AccountCheckList accountCheckList)
        {
            return new AccountCheckListInfo
            {
                Id = accountCheckList.Id,
                IsPersonalInfoProvided = accountCheckList.IsPersonalInfoProvided,
                IsDocumentsUploaded = accountCheckList.IsDocumentsUploaded,
                IsPictureUploaded = accountCheckList.IsPictureUploaded,
                AccountId = accountCheckList.AccountId

            };
        }

        public Data.AccountCheckList ConvertToDb(AccountCheckListInfo accountCheckListInfo)
        {
            return new Data.AccountCheckList
            {
                Id = accountCheckListInfo.Id,
                IsPersonalInfoProvided = accountCheckListInfo.IsPersonalInfoProvided,
                IsDocumentsUploaded = accountCheckListInfo.IsDocumentsUploaded,
                IsPictureUploaded = accountCheckListInfo.IsPictureUploaded,
                AccountId = accountCheckListInfo.AccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

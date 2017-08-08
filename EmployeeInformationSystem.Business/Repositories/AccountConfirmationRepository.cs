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
    public class AccountConfirmationRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public AccountConfirmationRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public AccountConfirmationRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public AccountConfirmationInfo GetAccountConfirmationByAccountId(int accountId)
        {
            return (from t in _context.AccountConfirmations.ToList()
                    where t.AccountId == accountId
                    select new AccountConfirmationInfo
                    {
                        Id = t.Id,
                        Token = t.Token,
                        AccountId = t.AccountId

                    }).FirstOrDefault();
        }

        public AccountConfirmationInfo GetAccountConfirmationByToken(string token)
        {
            return (from t in _context.AccountConfirmations.ToList()
                    where t.Token == token
                    select new AccountConfirmationInfo
                    {
                        Id = t.Id,
                        Token = t.Token,
                        AccountId = t.AccountId

                    }).FirstOrDefault();
        }

        public void SaveAccountConfirmation(AccountConfirmationInfo accountConfirmationInfo)
        {
            Data.AccountConfirmation accountConfirmation = ConvertToDb(accountConfirmationInfo);

            _context.AccountConfirmations.Add(accountConfirmation);
            _context.SaveChanges();
        }

        public void DeleteAccountConfirmation(int id)
        {
            Data.AccountConfirmation accountConfirmation = _context.AccountConfirmations.Find(id);

            if (accountConfirmation != null)
            {
                _context.AccountConfirmations.Remove(accountConfirmation);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        //public void DeleteAccountConfirmation(int accountId)
        //{
        //    Data.AccountConfirmation accountConfirmation = _context.AccountConfirmations.Where(x => x.AccountId == accountId).FirstOrDefault();

        //    if (accountConfirmation != null)
        //    {
        //        _context.AccountConfirmations.Remove(accountConfirmation);
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException();
        //    }
        //}

        public AccountConfirmationInfo ConvertToFacade(Data.AccountConfirmation accountConfirmation)
        {
            return new AccountConfirmationInfo
            {
                Id = accountConfirmation.Id,
                Token = accountConfirmation.Token,
                AccountId = accountConfirmation.AccountId

            };
        }

        public Data.AccountConfirmation ConvertToDb(AccountConfirmationInfo accountConfirmationInfo)
        {
            return new Data.AccountConfirmation
            {
                Id = accountConfirmationInfo.Id,
                Token = accountConfirmationInfo.Token,
                AccountId = accountConfirmationInfo.AccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

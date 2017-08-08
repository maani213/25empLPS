using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class MedicalAllowanceRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public MedicalAllowanceRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public MedicalAllowanceRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }


        public MedicalAllowanceInfo GetMedicalAllowanceById(int id)
        {
            return (from medicalAllowance in _context.MedicalAllowances.ToList()
                    where medicalAllowance.Id == id
                    select new MedicalAllowanceInfo
                    {
                        Id = medicalAllowance.Id,
                        Category = medicalAllowance.Category,
                        Amount = medicalAllowance.Amount

                    }).FirstOrDefault();
        }

        public MedicalAllowanceInfo GetMedicalAllowanceByCategory(string categoryName)
        {
            return (from medicalAllowance in _context.MedicalAllowances.ToList()
                    where medicalAllowance.Category == categoryName
                    select new MedicalAllowanceInfo
                    {
                        Id = medicalAllowance.Id,
                        Category = medicalAllowance.Category,
                        Amount = medicalAllowance.Amount

                    }).FirstOrDefault();
        }

        public List<MedicalAllowanceInfo> GetMedicalAllowancesList()
        {
            return (from medicalAllowance in _context.MedicalAllowances.ToList()
                    select new MedicalAllowanceInfo
                    {
                        Id = medicalAllowance.Id,
                        Category = medicalAllowance.Category,
                        Amount = medicalAllowance.Amount

                    }).ToList();
        }

        public void SaveMedicalAllowance(MedicalAllowanceInfo medicalAllowanceInfo)
        {
            Data.MedicalAllowance medicalAllowance = ConvertToDb(medicalAllowanceInfo);

            _context.MedicalAllowances.Add(medicalAllowance);
            _context.SaveChanges();

        }

        public void UpdateMedicalAllowance(MedicalAllowanceInfo medicalAllowanceInfo)
        {
            Data.MedicalAllowance medicalAllowance = _context.MedicalAllowances.Find(medicalAllowanceInfo.Id);

            if (medicalAllowance != null)
            {
                medicalAllowance.Category = medicalAllowanceInfo.Category;
                medicalAllowance.Amount = medicalAllowanceInfo.Amount;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void DeleteMedicalAllowance(int id)
        {
            Data.MedicalAllowance medicalAllowance = _context.MedicalAllowances.Find(id);

            if (medicalAllowance != null)
            {
                _context.MedicalAllowances.Remove(medicalAllowance);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public MedicalAllowanceInfo ConvertToFacade(Data.MedicalAllowance medicalAllowance)
        {
            return new MedicalAllowanceInfo
            {
                Id = medicalAllowance.Id,
                Category = medicalAllowance.Category,
                Amount = medicalAllowance.Amount

            };
        }

        public Data.MedicalAllowance ConvertToDb(MedicalAllowanceInfo medicalAllowanceInfo)
        {
            return new Data.MedicalAllowance
            {
                Id = medicalAllowanceInfo.Id,
                Category = medicalAllowanceInfo.Category,
                Amount = medicalAllowanceInfo.Amount

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

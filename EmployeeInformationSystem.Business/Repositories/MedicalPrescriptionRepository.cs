using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class MedicalPrescriptionRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public MedicalPrescriptionRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public MedicalPrescriptionRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public MedicalPrescriptionInfo GetMedicalPrescriptionById(int id)
        {
            return (from medicalPrescription in _context.MedicalPrescriptions.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on medicalPrescription.MedicalCheckoutId equals medicalCheckout.Id
                    where medicalPrescription.Id == id
                    select new MedicalPrescriptionInfo
                    {
                        Id = medicalPrescription.Id,
                        PrescriptionPath = medicalPrescription.PrescriptionPath,
                        FileName = medicalPrescription.FileName,
                        UploadDate = medicalPrescription.UploadDate,
                        MedicalCheckoutId = medicalPrescription.MedicalCheckoutId,
                        EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                        CheckoutStatus = medicalCheckout.Status

                    }).FirstOrDefault();
        }

        public List<MedicalPrescriptionInfo> GetMedicalPrescriptionsListByMedicalCheckoutId(int id)
        {
            return (from medicalPrescription in _context.MedicalPrescriptions.ToList()
                    where medicalPrescription.MedicalCheckoutId == id
                    select new MedicalPrescriptionInfo
                    {
                        Id = medicalPrescription.Id,
                        PrescriptionPath = medicalPrescription.PrescriptionPath,
                        FileName = medicalPrescription.FileName,
                        UploadDate = medicalPrescription.UploadDate,
                        MedicalCheckoutId = medicalPrescription.MedicalCheckoutId

                    }).ToList();
        }

        public List<MedicalPrescriptionInfo> GetMedicalPrescriptionsList()
        {
            return (from medicalPrescription in _context.MedicalPrescriptions.ToList()
                    select new MedicalPrescriptionInfo
                    {
                        Id = medicalPrescription.Id,
                        PrescriptionPath = medicalPrescription.PrescriptionPath,
                        FileName = medicalPrescription.FileName,
                        UploadDate = medicalPrescription.UploadDate,
                        MedicalCheckoutId = medicalPrescription.MedicalCheckoutId

                    }).ToList();
        }

        public void SaveMedicalPrescription(MedicalPrescriptionInfo medicalPrescriptionInfo)
        {
            Data.MedicalPrescription medicalPrescription = ConvertToDb(medicalPrescriptionInfo);

            _context.MedicalPrescriptions.Add(medicalPrescription);
            _context.SaveChanges();

        }

        public void UpdateMedicalPrescription(MedicalPrescriptionInfo medicalPrescriptionInfo)
        {
            Data.MedicalPrescription medicalPrescription = _context.MedicalPrescriptions.Find(medicalPrescriptionInfo.Id);

            if (medicalPrescription != null)
            {
                //Not implemented yet

                //medicalAllowance.Category = medicalAllowanceInfo.Category;
                //medicalAllowance.Amount = medicalAllowanceInfo.Amount;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void DeleteMedicalPrescription(int id)
        {
            Data.MedicalPrescription medicalPrescription = _context.MedicalPrescriptions.Find(id);

            if (medicalPrescription != null)
            {
                _context.MedicalPrescriptions.Remove(medicalPrescription);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public MedicalPrescriptionInfo ConvertToFacade(Data.MedicalPrescription medicalPrescription)
        {
            return new MedicalPrescriptionInfo
            {
                Id = medicalPrescription.Id,
                PrescriptionPath = medicalPrescription.PrescriptionPath,
                FileName = medicalPrescription.FileName,
                UploadDate = medicalPrescription.UploadDate,
                MedicalCheckoutId = medicalPrescription.MedicalCheckoutId

            };
        }

        public Data.MedicalPrescription ConvertToDb(MedicalPrescriptionInfo medicalPrescriptionInfo)
        {
            return new Data.MedicalPrescription
            {
                Id = medicalPrescriptionInfo.Id,
                PrescriptionPath = medicalPrescriptionInfo.PrescriptionPath,
                FileName = medicalPrescriptionInfo.FileName,
                UploadDate = medicalPrescriptionInfo.UploadDate,
                MedicalCheckoutId = medicalPrescriptionInfo.MedicalCheckoutId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

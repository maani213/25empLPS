using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.MedicalCheckoutRepository
{
    public class MedicalCheckoutRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public MedicalCheckoutRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public MedicalCheckoutRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public MedicalCheckoutInfo GetMedicalCheckoutById(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on familyMember.Id equals medicalCheckout.FamilyMemberId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on medicalCheckout.EmployeeInfoId equals employeeInfo.Id
                    where medicalCheckout.Id == id
                    select new MedicalCheckoutInfo
                    {
                        Id = medicalCheckout.Id,
                        RequestDate = medicalCheckout.RequestDate,
                        TreatmentDate = medicalCheckout.TreatmentDate,
                        Description = medicalCheckout.Description,
                        Amount = medicalCheckout.Amount,
                        Status = medicalCheckout.Status,
                        IsCreatedByAdmin = medicalCheckout.IsCreatedByAdmin,
                        RequestProcessDate = medicalCheckout.RequestProcessDate,
                        RequestProcessByAccountId = medicalCheckout.RequestProcessByAccountId,
                        EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                        FamilyMemberId = medicalCheckout.FamilyMemberId,
                        PatientName = familyMember.Name,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).FirstOrDefault();
        }

        public List<MedicalCheckoutInfo> GetPendingMedicalCheckoutsListByEmployeeId(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on familyMember.Id equals medicalCheckout.FamilyMemberId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on medicalCheckout.EmployeeInfoId equals employeeInfo.Id
                    where medicalCheckout.Status == "Pending" && medicalCheckout.EmployeeInfoId == id
                    orderby medicalCheckout.Id ascending
                    select new MedicalCheckoutInfo
                    {
                        Id = medicalCheckout.Id,
                        RequestDate = medicalCheckout.RequestDate,
                        TreatmentDate = medicalCheckout.TreatmentDate,
                        Description = medicalCheckout.Description,
                        Amount = medicalCheckout.Amount,
                        Status = medicalCheckout.Status,
                        IsCreatedByAdmin = medicalCheckout.IsCreatedByAdmin,
                        RequestProcessDate = medicalCheckout.RequestProcessDate,
                        RequestProcessByAccountId = medicalCheckout.RequestProcessByAccountId,
                        EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                        FamilyMemberId = medicalCheckout.FamilyMemberId,
                        PatientName = familyMember.Name,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<MedicalCheckoutInfo> GetPendingMedicalCheckoutsList()
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on familyMember.Id equals medicalCheckout.FamilyMemberId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on medicalCheckout.EmployeeInfoId equals employeeInfo.Id
                    where medicalCheckout.Status == "Pending"
                    group new { familyMember, medicalCheckout, employeeInfo } by medicalCheckout.EmployeeInfoId into grp
                    select new MedicalCheckoutInfo
                    {
                        Id = grp.Key,
                        RequestDate = grp.FirstOrDefault().medicalCheckout.RequestDate,
                        TreatmentDate = grp.FirstOrDefault().medicalCheckout.TreatmentDate,
                        Description = grp.FirstOrDefault().medicalCheckout.Description,
                        Amount = grp.FirstOrDefault().medicalCheckout.Amount,
                        Status = grp.FirstOrDefault().medicalCheckout.Status,
                        IsCreatedByAdmin = grp.FirstOrDefault().medicalCheckout.IsCreatedByAdmin,
                        RequestProcessDate = grp.FirstOrDefault().medicalCheckout.RequestProcessDate,
                        RequestProcessByAccountId = grp.FirstOrDefault().medicalCheckout.RequestProcessByAccountId,
                        EmployeeInfoId = grp.FirstOrDefault().medicalCheckout.EmployeeInfoId,
                        FamilyMemberId = grp.FirstOrDefault().medicalCheckout.FamilyMemberId,
                        PatientName = grp.FirstOrDefault().familyMember.Name,
                        EmployeeName = grp.FirstOrDefault().employeeInfo.FirstName + " " + grp.FirstOrDefault().employeeInfo.LastName

                    }).ToList();
        }

        public List<MedicalCheckoutInfo> GetAvailedMedicalCheckoutsListByEmployeeId(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on familyMember.Id equals medicalCheckout.FamilyMemberId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on medicalCheckout.EmployeeInfoId equals employeeInfo.Id
                    where medicalCheckout.Status == "Approved" && medicalCheckout.EmployeeInfoId == id
                    orderby medicalCheckout.Id ascending
                    select new MedicalCheckoutInfo
                    {
                        Id = medicalCheckout.Id,
                        RequestDate = medicalCheckout.RequestDate,
                        TreatmentDate = medicalCheckout.TreatmentDate,
                        Description = medicalCheckout.Description,
                        Amount = medicalCheckout.Amount,
                        Status = medicalCheckout.Status,
                        IsCreatedByAdmin = medicalCheckout.IsCreatedByAdmin,
                        RequestProcessDate = medicalCheckout.RequestProcessDate,
                        RequestProcessByAccountId = medicalCheckout.RequestProcessByAccountId,
                        EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                        FamilyMemberId = medicalCheckout.FamilyMemberId,
                        PatientName = familyMember.Name,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<MedicalCheckoutInfo> GetAvailedMedicalCheckoutsListByEmployeeIdYearwise(int id, string year)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on familyMember.Id equals medicalCheckout.FamilyMemberId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on medicalCheckout.EmployeeInfoId equals employeeInfo.Id
                    where medicalCheckout.Status == "Approved" && medicalCheckout.EmployeeInfoId == id && medicalCheckout.RequestDate.Value.ToString("yyyy") == year
                    orderby medicalCheckout.Id ascending
                    select new MedicalCheckoutInfo
                    {
                        Id = medicalCheckout.Id,
                        RequestDate = medicalCheckout.RequestDate,
                        TreatmentDate = medicalCheckout.TreatmentDate,
                        Description = medicalCheckout.Description,
                        Amount = medicalCheckout.Amount,
                        Status = medicalCheckout.Status,
                        IsCreatedByAdmin = medicalCheckout.IsCreatedByAdmin,
                        RequestProcessDate = medicalCheckout.RequestProcessDate,
                        RequestProcessByAccountId = medicalCheckout.RequestProcessByAccountId,
                        EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                        FamilyMemberId = medicalCheckout.FamilyMemberId,
                        PatientName = familyMember.Name,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<MedicalCheckoutInfo> GetIncompleteMedicalCheckoutsListByEmployeeId(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    join medicalCheckout in _context.MedicalCheckouts.ToList()
                    on familyMember.Id equals medicalCheckout.FamilyMemberId
                    join employeeInfo in _context.EmployeeInfoes.ToList()
                    on medicalCheckout.EmployeeInfoId equals employeeInfo.Id
                    where medicalCheckout.Status == "Incomplete" && medicalCheckout.EmployeeInfoId == id
                    select new MedicalCheckoutInfo
                    {
                        Id = medicalCheckout.Id,
                        RequestDate = medicalCheckout.RequestDate,
                        TreatmentDate = medicalCheckout.TreatmentDate,
                        Description = medicalCheckout.Description,
                        Amount = medicalCheckout.Amount,
                        Status = medicalCheckout.Status,
                        IsCreatedByAdmin = medicalCheckout.IsCreatedByAdmin,
                        RequestProcessDate = medicalCheckout.RequestProcessDate,
                        RequestProcessByAccountId = medicalCheckout.RequestProcessByAccountId,
                        EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                        FamilyMemberId = medicalCheckout.FamilyMemberId,
                        PatientName = familyMember.Name,
                        EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName

                    }).ToList();
        }

        public List<int> GetMedicalYearsList(int employeeId)
        {
            return (from medicalCheckout in _context.MedicalCheckouts.ToList()
                    where medicalCheckout.Status == "Approved" && medicalCheckout.EmployeeInfoId == employeeId
                    orderby medicalCheckout.RequestDate descending
                    group medicalCheckout by medicalCheckout.RequestDate.Value.ToString("yyyy") into grp
                    select DateTime.ParseExact(grp.Key, "yyyy", CultureInfo.CurrentCulture).Year
                    ).ToList();
        }

        public int SaveMedicalCheckout(MedicalCheckoutInfo medicalCheckoutInfo)
        {
            Data.MedicalCheckout medicalCheckout = ConvertToDb(medicalCheckoutInfo);

            _context.MedicalCheckouts.Add(medicalCheckout);
            _context.SaveChanges();

            return medicalCheckout.Id;

        }

        public void UpdateMedicalCheckout(MedicalCheckoutInfo medicalCheckoutInfo)
        {
            Data.MedicalCheckout medicalCheckout = _context.MedicalCheckouts.Find(medicalCheckoutInfo.Id);

            if (medicalCheckout != null)
            {
                medicalCheckout.RequestDate = medicalCheckoutInfo.RequestDate;
                medicalCheckout.TreatmentDate = medicalCheckoutInfo.TreatmentDate;
                medicalCheckout.Description = medicalCheckoutInfo.Description;
                medicalCheckout.Amount = medicalCheckoutInfo.Amount;
                medicalCheckout.Status = medicalCheckoutInfo.Status;
                medicalCheckout.IsCreatedByAdmin = medicalCheckoutInfo.IsCreatedByAdmin;
                medicalCheckout.RequestProcessDate = medicalCheckoutInfo.RequestProcessDate;
                medicalCheckout.RequestProcessByAccountId = medicalCheckoutInfo.RequestProcessByAccountId;
                medicalCheckout.FamilyMemberId = medicalCheckoutInfo.FamilyMemberId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void DeleteMedicalCheckout(int id)
        {
            Data.MedicalCheckout medicalCheckout = _context.MedicalCheckouts.Find(id);

            if (medicalCheckout != null)
            {
                _context.MedicalCheckouts.Remove(medicalCheckout);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public MedicalCheckoutInfo ConvertToFacade(Data.MedicalCheckout medicalCheckout)
        {
            return new MedicalCheckoutInfo
            {
                Id = medicalCheckout.Id,
                RequestDate = medicalCheckout.RequestDate,
                TreatmentDate = medicalCheckout.TreatmentDate,
                Description = medicalCheckout.Description,
                Amount = medicalCheckout.Amount,
                Status = medicalCheckout.Status,
                IsCreatedByAdmin = medicalCheckout.IsCreatedByAdmin,
                RequestProcessDate = medicalCheckout.RequestProcessDate,
                RequestProcessByAccountId = medicalCheckout.RequestProcessByAccountId,
                EmployeeInfoId = medicalCheckout.EmployeeInfoId,
                FamilyMemberId = medicalCheckout.FamilyMemberId

            };
        }

        public Data.MedicalCheckout ConvertToDb(MedicalCheckoutInfo medicalCheckoutInfo)
        {
            return new Data.MedicalCheckout
            {
                Id = medicalCheckoutInfo.Id,
                RequestDate = medicalCheckoutInfo.RequestDate,
                TreatmentDate = medicalCheckoutInfo.TreatmentDate,
                Description = medicalCheckoutInfo.Description,
                Amount = medicalCheckoutInfo.Amount,
                Status = medicalCheckoutInfo.Status,
                IsCreatedByAdmin = medicalCheckoutInfo.IsCreatedByAdmin,
                RequestProcessDate = medicalCheckoutInfo.RequestProcessDate,
                RequestProcessByAccountId = medicalCheckoutInfo.RequestProcessByAccountId,
                EmployeeInfoId = medicalCheckoutInfo.EmployeeInfoId,
                FamilyMemberId = medicalCheckoutInfo.FamilyMemberId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

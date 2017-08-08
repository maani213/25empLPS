using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class FamilyMemberRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public FamilyMemberRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public FamilyMemberRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }


        public FamilyMemberInfo GetFamilyMemberById(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    where familyMember.Id == id
                    select new FamilyMemberInfo
                    {
                        Id = familyMember.Id,
                        Name = familyMember.Name,
                        CNIC = familyMember.CNIC,
                        Relation = familyMember.Relation,
                        EmployeeInfoId = familyMember.EmployeeInfoId

                    }).FirstOrDefault();
        }

        public List<FamilyMemberInfo> GetFamilyMembersList()
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    select new FamilyMemberInfo
                    {
                        Id = familyMember.Id,
                        Name = familyMember.Name,
                        CNIC = familyMember.CNIC,
                        Relation = familyMember.Relation,
                        EmployeeInfoId = familyMember.EmployeeInfoId

                    }).ToList();
        }

        public List<FamilyMemberInfo> GetFamilyMembersListByEmployeeId(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    where familyMember.Name != "Self" && familyMember.EmployeeInfoId == id
                    select new FamilyMemberInfo
                    {
                        Id = familyMember.Id,
                        Name = familyMember.Name,
                        CNIC = familyMember.CNIC,
                        Relation = familyMember.Relation,
                        EmployeeInfoId = familyMember.EmployeeInfoId

                    }).ToList();
        }

        public List<FamilyMemberInfo> GetAllFamilyMembersListByEmployeeId(int id)
        {
            return (from familyMember in _context.FamilyMembers.ToList()
                    where familyMember.EmployeeInfoId == id
                    select new FamilyMemberInfo
                    {
                        Id = familyMember.Id,
                        Name = familyMember.Name,
                        CNIC = familyMember.CNIC,
                        Relation = familyMember.Relation,
                        EmployeeInfoId = familyMember.EmployeeInfoId

                    }).ToList();
        }

        public void SaveFamilyMember(FamilyMemberInfo familyMemberInfo)
        {
            Data.FamilyMember familyMember = ConvertToDb(familyMemberInfo);

            _context.FamilyMembers.Add(familyMember);
            _context.SaveChanges();

        }

        public void UpdateFamilyMember(FamilyMemberInfo familyMemberInfo)
        {
            Data.FamilyMember familyMember = _context.FamilyMembers.Find(familyMemberInfo.Id);

            if (familyMember != null)
            {
                familyMember.Name = familyMemberInfo.Name;
                familyMember.CNIC = familyMemberInfo.CNIC;
                familyMember.Relation = familyMemberInfo.Relation;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public void DeleteFamilyMember(int id)
        {
            Data.FamilyMember familyMember = _context.FamilyMembers.Find(id);

            if (familyMember != null)
            {
                _context.FamilyMembers.Remove(familyMember);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public FamilyMemberInfo ConvertToFacade(Data.FamilyMember familyMember)
        {
            return new FamilyMemberInfo
            {
                Id = familyMember.Id,
                Name = familyMember.Name,
                CNIC = familyMember.CNIC,
                Relation = familyMember.Relation,
                EmployeeInfoId = familyMember.EmployeeInfoId

            };
        }

        public Data.FamilyMember ConvertToDb(FamilyMemberInfo familyMemberInfo)
        {
            return new Data.FamilyMember
            {
                Id = familyMemberInfo.Id,
                Name = familyMemberInfo.Name,
                CNIC = familyMemberInfo.CNIC,
                Relation = familyMemberInfo.Relation,
                EmployeeInfoId = familyMemberInfo.EmployeeInfoId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

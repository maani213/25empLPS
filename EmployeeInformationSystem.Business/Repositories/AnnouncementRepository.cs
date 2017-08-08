using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class AnnouncementRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public AnnouncementRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public AnnouncementRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<AnnouncementInfo> GetAnnouncementList()
        {
            return (from announcement in _context.Announcements.ToList()
                    where (DateTime.Now - DateTime.Parse(announcement.CreatedOn.ToString())).TotalDays < 10
                    orderby announcement.Id descending
                    select new AnnouncementInfo
                    {
                        Id = announcement.Id,
                        Title = announcement.Title,
                        Description = announcement.Description,
                        CreatedOn = announcement.CreatedOn,
                        CreatedByAccountId = announcement.CreatedByAccountId,
                        ModifiedOn = announcement.ModifiedOn,
                        ModifiedByAccountId = announcement.ModifiedByAccountId

                    }).ToList();
        }

        public AnnouncementInfo GetAnnouncementById(int id)
        {
            return (from announcement in _context.Announcements.ToList()
                    where announcement.Id == id
                    select new AnnouncementInfo
                    {
                        Id = announcement.Id,
                        Title = announcement.Title,
                        Description = announcement.Description,
                        CreatedOn = announcement.CreatedOn,
                        CreatedByAccountId = announcement.CreatedByAccountId,
                        ModifiedOn = announcement.ModifiedOn,
                        ModifiedByAccountId = announcement.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public AnnouncementInfo GetRecentAnnouncement()
        {
            return (from announcement in _context.Announcements.ToList()
                    select new AnnouncementInfo
                    {
                        Id = announcement.Id,
                        Title = announcement.Title,
                        Description = announcement.Description,
                        CreatedOn = announcement.CreatedOn,
                        CreatedByAccountId = announcement.CreatedByAccountId,
                        ModifiedOn = announcement.ModifiedOn,
                        ModifiedByAccountId = announcement.ModifiedByAccountId

                    }).LastOrDefault();
        }

        public void SaveAnnouncement(AnnouncementInfo announcementInfo)
        {
            Data.Announcement announcement = ConvertToDb(announcementInfo);

            _context.Announcements.Add(announcement);
            _context.SaveChanges();
        }

        public void DeleteAnnouncement(int id)
        {
            Data.Announcement announcement = _context.Announcements.Find(id);

            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }

        }

        public void UpdateAnnouncement(AnnouncementInfo announcementInfo)
        {
            Data.Announcement announcement = _context.Announcements.Find(announcementInfo.Id);

            if (announcement != null)
            {
                announcement.Title = announcementInfo.Title;
                announcement.Description = announcementInfo.Description;
                announcement.ModifiedOn = announcementInfo.ModifiedOn;
                announcement.ModifiedByAccountId = announcementInfo.ModifiedByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public AnnouncementInfo ConvertToFacade(Data.Announcement announcement)
        {
            return new AnnouncementInfo
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Description = announcement.Description,
                CreatedOn = announcement.CreatedOn,
                CreatedByAccountId = announcement.CreatedByAccountId,
                ModifiedOn = announcement.ModifiedOn,
                ModifiedByAccountId = announcement.ModifiedByAccountId

            };
        }

        public Data.Announcement ConvertToDb(AnnouncementInfo announcementInfo)
        {
            return new Data.Announcement
            {
                Id = announcementInfo.Id,
                Title = announcementInfo.Title,
                Description = announcementInfo.Description,
                CreatedOn = announcementInfo.CreatedOn,
                CreatedByAccountId = announcementInfo.CreatedByAccountId,
                ModifiedOn = announcementInfo.ModifiedOn,
                ModifiedByAccountId = announcementInfo.ModifiedByAccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

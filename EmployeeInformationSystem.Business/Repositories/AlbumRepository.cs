using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class AlbumRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public AlbumRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public AlbumRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<AlbumInfo> GetAlbumList()
        {
            return (from album in _context.Albums.ToList()
                    orderby album.Id descending
                    select new AlbumInfo
                    {
                        Id = album.Id,
                        Title = album.Title,
                        Description = album.Description,
                        CoverPhotoPath = album.CoverPhotoPath,
                        NumberOfPhotos = _context.Photos.Count(x => x.AlbumId == album.Id),
                        CreatedOn = album.CreatedOn,
                        CreatedByAccountId = album.CreatedByAccountId,
                        ModifiedOn = album.ModifiedOn,
                        ModifiedByAccountId = album.ModifiedByAccountId

                    }).ToList();
        }

        public AlbumInfo GetAlbumById(int id)
        {
            return (from album in _context.Albums.ToList()
                    where album.Id == id
                    select new AlbumInfo
                    {
                        Id = album.Id,
                        Title = album.Title,
                        Description = album.Description,
                        CoverPhotoPath = album.CoverPhotoPath,
                        CreatedOn = album.CreatedOn,
                        CreatedByAccountId = album.CreatedByAccountId,
                        ModifiedOn = album.ModifiedOn,
                        ModifiedByAccountId = album.ModifiedByAccountId

                    }).FirstOrDefault();
        }

        public AlbumInfo GetRecentAlbum()
        {
            return (from album in _context.Albums.ToList()
                    select new AlbumInfo
                    {
                        Id = album.Id,
                        Title = album.Title,
                        Description = album.Description,
                        CoverPhotoPath = album.CoverPhotoPath,
                        CreatedOn = album.CreatedOn,
                        CreatedByAccountId = album.CreatedByAccountId,
                        ModifiedOn = album.ModifiedOn,
                        ModifiedByAccountId = album.ModifiedByAccountId

                    }).LastOrDefault();
        }

        public void SaveAlbum(AlbumInfo albumInfo)
        {
            Data.Album album = ConvertToDb(albumInfo);

            _context.Albums.Add(album);
            _context.SaveChanges();
        }

        public void DeleteAlbum(int id)
        {
            Data.Album album = _context.Albums.Find(id);

            if (album != null)
            {
                _context.Albums.Remove(album);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }

        }

        public void UpdateAlbum(AlbumInfo albumInfo)
        {
            Data.Album album = _context.Albums.Find(albumInfo.Id);

            if (album != null)
            {
                album.Title = albumInfo.Title;
                album.Description = albumInfo.Description;
                album.CoverPhotoPath = albumInfo.CoverPhotoPath;
                album.ModifiedOn = albumInfo.ModifiedOn;
                album.ModifiedByAccountId = albumInfo.ModifiedByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public AlbumInfo ConvertToFacade(Data.Album album)
        {
            return new AlbumInfo
            {
                Id = album.Id,
                Title = album.Title,
                Description = album.Description,
                CoverPhotoPath = album.CoverPhotoPath,
                CreatedOn = album.CreatedOn,
                CreatedByAccountId = album.CreatedByAccountId,
                ModifiedOn = album.ModifiedOn,
                ModifiedByAccountId = album.ModifiedByAccountId

            };
        }

        public Data.Album ConvertToDb(AlbumInfo albumInfo)
        {
            return new Data.Album
            {
                Id = albumInfo.Id,
                Title = albumInfo.Title,
                Description = albumInfo.Description,
                CoverPhotoPath = albumInfo.CoverPhotoPath,
                CreatedOn = albumInfo.CreatedOn,
                CreatedByAccountId = albumInfo.CreatedByAccountId,
                ModifiedOn = albumInfo.ModifiedOn,
                ModifiedByAccountId = albumInfo.ModifiedByAccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

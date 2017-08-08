using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class PhotoRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public PhotoRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public PhotoRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<PhotoInfo> GetPhotoList()
        {
            return (from photo in _context.Photos.ToList()
                    select new PhotoInfo
                    {
                        Id = photo.Id,
                        Name = photo.Name,
                        Path = photo.Path,
                        AlbumId = photo.AlbumId,
                        UploadOn = photo.UploadOn,
                        UploadByAccountId = photo.UploadByAccountId

                    }).ToList();
        }

        public List<PhotoInfo> GetPhotoListByAlbumId(int id)
        {
            return (from photo in _context.Photos.ToList()
                    join album in _context.Albums.ToList()
                    on photo.AlbumId equals album.Id
                    where photo.AlbumId == id
                    select new PhotoInfo
                    {
                        Id = photo.Id,
                        Name = photo.Name,
                        Path = photo.Path,
                        AlbumId = photo.AlbumId,
                        AlbumTitle = album.Title,
                        UploadOn = photo.UploadOn,
                        UploadByAccountId = photo.UploadByAccountId

                    }).ToList();
        }

        public PhotoInfo GetPhotoById(int id)
        {
            return (from photo in _context.Photos.ToList()
                    where photo.Id == id
                    select new PhotoInfo
                    {
                        Id = photo.Id,
                        Name = photo.Name,
                        Path = photo.Path,
                        AlbumId = photo.AlbumId,
                        UploadOn = photo.UploadOn,
                        UploadByAccountId = photo.UploadByAccountId

                    }).FirstOrDefault();
        }

        public void SavePhoto(PhotoInfo photoInfo)
        {
            Data.Photo photo = ConvertToDb(photoInfo);

            _context.Photos.Add(photo);
            _context.SaveChanges();
        }

        public void DeletePhoto(int id)
        {
            Data.Photo photo = _context.Photos.Find(id);

            if (photo != null)
            {
                _context.Photos.Remove(photo);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }

        }

        public void DeleteAlbumPhotos(int id)
        {
            _context.Photos.ToList().RemoveAll(x => x.AlbumId == id);
            _context.SaveChanges();
        }

        public void UpdatePhoto(PhotoInfo photoInfo)
        {
            Data.Photo photo = _context.Photos.Find(photoInfo.Id);

            if (photo != null)
            {
                photo.Name = photoInfo.Name;
                photo.Path = photoInfo.Path;
                photo.AlbumId = photoInfo.AlbumId;
                photo.UploadOn = photoInfo.UploadOn;
                photo.UploadByAccountId = photoInfo.UploadByAccountId;

                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public PhotoInfo ConvertToFacade(Data.Photo photo)
        {
            return new PhotoInfo
            {
                Id = photo.Id,
                Name = photo.Name,
                Path = photo.Path,
                AlbumId = photo.AlbumId,
                UploadOn = photo.UploadOn,
                UploadByAccountId = photo.UploadByAccountId

            };
        }

        public Data.Photo ConvertToDb(PhotoInfo photoInfo)
        {
            return new Data.Photo
            {
                Id = photoInfo.Id,
                Name = photoInfo.Name,
                Path = photoInfo.Path,
                AlbumId = photoInfo.AlbumId,
                UploadOn = photoInfo.UploadOn,
                UploadByAccountId = photoInfo.UploadByAccountId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

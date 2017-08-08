using EmployeeInformationSystem.Data;
using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Business.Repositories
{
    public class DocumentRepository : IDisposable
    {
        EmployeeInformationEntities _context = null;

        public DocumentRepository()
        {
            _context = new EmployeeInformationEntities();
        }

        public DocumentRepository(EmployeeInformationEntities context)
        {
            this._context = context;
        }

        public List<DocumentInfo> GetDocumentListByEmployeeId(int employeeId)
        {
            return (from docs in _context.Documents.ToList()
                    where docs.EmployeeInfoId == employeeId
                    select new DocumentInfo
                    {
                        Id = docs.Id,
                        DocumentPath = docs.DocumentPath,
                        FileName = docs.FileName,
                        UploadDate = docs.UploadDate,
                        EmployeeInfoId = docs.EmployeeInfoId,
                        DocumentType = docs.DocumentType

                    }).ToList();
        }

        public List<DocumentInfo> GetRequiredDocumentListByEmployeeId(int employeeId)
        {
            return (from docs in _context.Documents.ToList()
                    where docs.EmployeeInfoId == employeeId && docs.DocumentType == "CNICFront" || docs.DocumentType == "CNICBack" || docs.DocumentType == "CV"
                    select new DocumentInfo
                    {
                        Id = docs.Id,
                        DocumentPath = docs.DocumentPath,
                        FileName = docs.FileName,
                        UploadDate = docs.UploadDate,
                        EmployeeInfoId = docs.EmployeeInfoId,
                        DocumentType = docs.DocumentType

                    }).ToList();
        }

        public DocumentInfo GetDocumentById(int id)
        {
            Data.Document document = _context.Documents.Find(id);

            if (document != null)
            {
                return ConvertToFacade(document);
            }
            else
            {
                DocumentInfo documentInfo = new DocumentInfo();
                documentInfo = null;

                return documentInfo;
            }
        }

        public int SaveDocument(DocumentInfo documentInfo)
        {
            Data.Document document = ConvertToDb(documentInfo);

            _context.Documents.Add(document);
            _context.SaveChanges();

            return document.Id;
        }

        public void DeleteDocument(int id)
        {
            Data.Document document = _context.Documents.Find(id);

            if (document != null)
            {
                _context.Documents.Remove(document);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public DocumentInfo ConvertToFacade(Data.Document document)
        {
            return new DocumentInfo
            {
                Id = document.Id,
                DocumentPath = document.DocumentPath,
                DocumentType = document.DocumentType,
                FileName = document.FileName,
                UploadDate = document.UploadDate,
                EmployeeInfoId = document.EmployeeInfoId

            };
        }

        public Data.Document ConvertToDb(DocumentInfo documentInfo)
        {
            return new Data.Document
            {
                Id = documentInfo.Id,
                DocumentPath = documentInfo.DocumentPath,
                DocumentType = documentInfo.DocumentType,
                FileName = documentInfo.FileName,
                UploadDate = documentInfo.UploadDate,
                EmployeeInfoId = documentInfo.EmployeeInfoId

            };
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

using Schedent.Domain.DTO.Document;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;

namespace Schedent.BusinessLogic.Services
{
    public class DocumentService : BaseService
    {
        public DocumentService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Document Add(DocumentModel model)
        {
            var document = new Document
            {
                File = ConvertToByteArray(model.File),
                CreatedOn = DateTime.Now
            };

            UnitOfWork.DocumentRepository.Add(document);

            Save();

            return document;
        }

        public byte[] ConvertToByteArray(string img)
        {
            return string.IsNullOrEmpty(img) ? null : Convert.FromBase64String(img);
        }
    }
}

using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;

namespace Schedent.BusinessLogic.Services
{
    public class DocumentService : BaseService
    {
        public DocumentService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Document Add(string file)
        {
            var document = new Document
            {
                File = ConvertToByteArray(file),
                CreatedOn = DateTime.Now
            };

            UnitOfWork.DocumentRepository.Add(document);

            Save();

            return document;
        }

        public static byte[] ConvertToByteArray(string file)
        {
            return string.IsNullOrEmpty(file) ? null : Convert.FromBase64String(file);
        }
    }
}

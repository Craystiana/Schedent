using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using System;

namespace Schedent.BusinessLogic.Services
{
    public class DocumentService : BaseService
    {
        /// <summary>
        /// DocumentService constructor
        /// Inject the UnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public DocumentService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Method for adding a new timetable document
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method for converting a string to byte array
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] ConvertToByteArray(string file)
        {
            return string.IsNullOrEmpty(file) ? null : Convert.FromBase64String(file);
        }
    }
}

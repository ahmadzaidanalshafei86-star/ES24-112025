


using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ES.Web.Services
{
    public class RighttoobtaininformationService
    {
        private readonly ApplicationDbContext _context;

        public RighttoobtaininformationService(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<RighttoobtaininformationFormViewModel> InitializeRighttoobtaininformationFormViewModel()
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);


            return new RighttoobtaininformationFormViewModel
            {

            };
        }

        public async Task<string> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Righttoobtaininformation");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }


        /// <summary>
        /// Saves a RighttoobtaininformationRequest with its Hangars and Refrigators.
        /// </summary>
        public async Task SaveRighttoobtaininformationRequestAsync(
        RighttoobtaininformationRequest request,
        List<string> letterFiles,
        List<string> personalIdFiles,
        List<string> additionalFiles)
        {
            _context.RighttoobtaininformationRequests.Add(request);
            await _context.SaveChangesAsync();

            // Letter Files
            foreach (var file in letterFiles)
            {
                _context.RighttoobtaininformationFiles.Add(new RighttoobtaininformationFile
                {
                    RequestId = request.Id,
                    FileType = "LetterFile",
                    FileName = file,
                    UploadedAt = DateTime.UtcNow
                });
            }

            // Personal ID Files
            foreach (var file in personalIdFiles)
            {
                _context.RighttoobtaininformationFiles.Add(new RighttoobtaininformationFile
                {
                    RequestId = request.Id,
                    FileType = "PersonalIdCopy",
                    FileName = file,
                    UploadedAt = DateTime.UtcNow
                });
            }

            // Additional Documents
            foreach (var file in additionalFiles)
            {
                _context.RighttoobtaininformationFiles.Add(new RighttoobtaininformationFile
                {
                    RequestId = request.Id,
                    FileType = "AdditionalDocuments",
                    FileName = file,
                    UploadedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }


    }
}

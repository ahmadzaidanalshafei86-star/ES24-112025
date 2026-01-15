


using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace ES.Web.Services
{
    public class TenderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _tenderFilesPath = "CMS/documents/Tenders/";

        public TenderService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        // Helper to build full file path
        private string BuildFilePath(string? fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return null!;
            return _tenderFilesPath + fileUrl;
        }

        public async Task<TenderDetailsViewModel?> GetTenderBySlugAsync(string slug)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
        new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = slug },
        new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
    };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetTenderBySlug";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // --- Result set 1: Main tender ---
            if (!await reader.ReadAsync())
                return null;

            var tender = new TenderDetailsViewModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null! : reader.GetString(reader.GetOrdinal("Title")),
                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null! : reader.GetString(reader.GetOrdinal("Code")),
                CopyPrice = reader.IsDBNull(reader.GetOrdinal("CopyPrice")) ? null! : reader.GetString(reader.GetOrdinal("CopyPrice")),
                StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EndDate"))
            };

            // --- Result set 2: Translations ---
            await reader.NextResultAsync();
            if (await reader.ReadAsync())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("TranslatedTitle")))
                    tender.Title = reader.GetString(reader.GetOrdinal("TranslatedTitle"));
            }

            // --- Result set 3: Tender Files ---
            await reader.NextResultAsync();
            var tenderFiles = new List<TenderFilesViewModel>();

            while (await reader.ReadAsync())
            {
                tenderFiles.Add(new TenderFilesViewModel
                {
                    FileName = reader.IsDBNull(reader.GetOrdinal("AltName")) ? "File" : reader.GetString(reader.GetOrdinal("AltName")),
                    FileUrl = BuildFilePath(reader.GetString(reader.GetOrdinal("FileUrl"))), // <-- updated here
                    DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder"))
                });
            }

            // Assign files to view model
            tender.ExistingFiles = tenderFiles;

            return tender;

        }


        public async Task<TenderDetailsViewModel?> GetTenderByIdAsync(int tenderId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
                new SqlParameter("@TenderId", SqlDbType.Int) { Value = tenderId },
                new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
            };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetTenderById";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // -------- Result set 1: Main Tender --------
            if (!await reader.ReadAsync())
                return null;

            var tender = new
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
           
            };

            // -------- Result set 2: Tender Translations --------
            await reader.NextResultAsync();
            string? translatedTitle = null, translatedShortDesc = null, translatedLongDesc = null;

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32(reader.GetOrdinal("LanguageId")) == languageId)
                {
                    translatedTitle = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title"));
                    break;
                }
            }

           

            var viewModel = new TenderDetailsViewModel
            {
                Slug = tender.Slug,
                Title = translatedTitle ?? tender.Title,
            };
            return viewModel;
        }



    }
}

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
    public class PurchaseOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string _poFilesPath = "CMS/documents/PurchaseOrders/";

        public PurchaseOrderService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Helper to build full file path
        private string BuildFilePath(string? fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return null!;
            return _poFilesPath + fileUrl;
        }

        // =============================================================
        // Get Purchase Order by Slug
        // =============================================================
        public async Task<PurchaseOrderDetailsViewModel?> GetPurchaseOrderBySlugAsync(string slug)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
                new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = slug },
                new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
            };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetPurchaseOrderBySlug";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // -------- Result set 1: Main Purchase Order --------
            if (!await reader.ReadAsync())
                return null;

            var po = new PurchaseOrderDetailsViewModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null! : reader.GetString(reader.GetOrdinal("Title")),
                Code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null! : reader.GetString(reader.GetOrdinal("Code")),
                StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                EndDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EndDate"))
            };

            // -------- Result set 2: Translations --------
            await reader.NextResultAsync();
            if (await reader.ReadAsync())
            {
                if (!reader.IsDBNull(reader.GetOrdinal("TranslatedTitle")))
                    po.Title = reader.GetString(reader.GetOrdinal("TranslatedTitle"));
            }

            // -------- Result set 3: Purchase Order Files --------
            await reader.NextResultAsync();
            var poFiles = new List<PurchaseOrderFilesViewModel>();

            while (await reader.ReadAsync())
            {
                poFiles.Add(new PurchaseOrderFilesViewModel
                {
                    FileName = reader.IsDBNull(reader.GetOrdinal("AltName")) ? "File" : reader.GetString(reader.GetOrdinal("AltName")),
                    FileUrl = BuildFilePath(reader.GetString(reader.GetOrdinal("FileUrl"))),
                    DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder"))
                });
            }

            po.ExistingFiles = poFiles;

            return po;
        }


        // =============================================================
        // Get Purchase Order by ID
        // =============================================================
        public async Task<PurchaseOrderDetailsViewModel?> GetPurchaseOrderByIdAsync(int poId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
                new SqlParameter("@PurchaseOrderId", SqlDbType.Int) { Value = poId },
                new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
            };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetPurchaseOrderById";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // -------- Result set 1 --------
            if (!await reader.ReadAsync())
                return null;

            var po = new
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
            };

            // -------- Result set 2: Translations --------
            await reader.NextResultAsync();
            string? translatedTitle = null;

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32(reader.GetOrdinal("LanguageId")) == languageId)
                {
                    translatedTitle = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title"));
                    break;
                }
            }

            var viewModel = new PurchaseOrderDetailsViewModel
            {
                Slug = po.Slug,
                Title = translatedTitle ?? po.Title,
            };

            return viewModel;
        }

    }
}

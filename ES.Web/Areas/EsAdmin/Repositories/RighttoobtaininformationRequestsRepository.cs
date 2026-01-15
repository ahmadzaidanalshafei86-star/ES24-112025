using ES.Core.Entities;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class RighttoobtaininformationRequestsRepository
    {
        private readonly ApplicationDbContext _context;

        public RighttoobtaininformationRequestsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of Right to Obtain Information requests (basic info).
        /// </summary>
        public async Task<List<RighttoobtaininformationRequestsViewModel>> GetRighttoobtaininformationRequestsInfoAsync()
        {
            return await _context.RighttoobtaininformationRequests
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RighttoobtaininformationRequestsViewModel
                {
                    Id = r.Id,
                    ApplicantCategory = r.ApplicantCategory ?? string.Empty,
                    Name = r.Name ?? string.Empty,
                    FatherName = r.FatherName ?? string.Empty,
                    GrandFatherName = r.GrandFatherName ?? string.Empty,
                    FamilyName = r.FamilyName ?? string.Empty,
                    CellPhone = r.CellPhoneNumber ?? string.Empty,
                    Email = r.Email ?? string.Empty,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a single Right to Obtain Information request with all details and attached files.
        /// </summary>
        public async Task<RighttoobtaininformationRequestsViewModel?> GetRighttoobtaininformationRequestsByIdAsync(int id)
        {
            var request = await _context.RighttoobtaininformationRequests
                .Include(r => r.RighttoobtaininformationFiles) // Include files
                .Where(r => r.Id == id)
                .Select(r => new RighttoobtaininformationRequestsViewModel
                {
                    Id = r.Id,
                    ApplicantCategory = r.ApplicantCategory ?? string.Empty,

                    // Private Sector
                    CompanyName = r.CompanyName ?? string.Empty,
                    AuthorizationBookNumber = r.AuthorizationBookNumber ?? string.Empty,
                    CommercialRegistrationNo = r.CommercialRegistrationNo ?? string.Empty,
                    AuthorizationLetterDate = r.AuthorizationLetterDate,
                    DelegateName = r.DelegateName ?? string.Empty,

                    // Personal Info
                    Name = r.Name ?? string.Empty,
                    FatherName = r.FatherName ?? string.Empty,
                    GrandFatherName = r.GrandFatherName ?? string.Empty,
                    FamilyName = r.FamilyName ?? string.Empty,
                    NationalNumber = r.NationalNumber ?? string.Empty,

                    // Contact
                    CellPhone = r.CellPhoneNumber ?? string.Empty,
                    Landline = r.LandlineNumber ?? string.Empty,
                    Fax = r.FaxNumber ?? string.Empty,
                    POBox = r.POBox ?? string.Empty,
                    Email = r.Email ?? string.Empty,

                    // Residence
                    ResidenceGovernorate = r.ResidenceGovernorate ?? string.Empty,
                    ResidenceCity = r.ResidenceCity ?? string.Empty,
                    ResidenceDistrict = r.ResidenceDistrict ?? string.Empty,

                    // Workplace
                    WorkplaceGovernorate = r.WorkplaceGovernorate ?? string.Empty,
                    WorkplaceCity = r.WorkplaceCity ?? string.Empty,
                    Employer = r.Employer ?? string.Empty,

                    // Identification
                    IdentificationType = r.IdentificationType ?? string.Empty,
                    DocumentName = r.OtherDocumentName ?? string.Empty,

                    // Purpose
                    Purpose = r.InformationPurpose ?? string.Empty,
                    OtherPurpose = r.OtherPurpose ?? string.Empty,

                    // Delivery Method
                    DeliveryMethod = r.DeliveryMethod ?? string.Empty,
                    OtherDelivery = r.OtherDeliveryMethod ?? string.Empty,

                    // Information Topic
                    InformationTopic = r.InformationTopic ?? string.Empty,

                    // Agreement
                    Agreement = r.Agreement ?? false,
                    CreatedAt = r.CreatedAt,

                    // Files
                    Files = r.RighttoobtaininformationFiles
                        .Select(f => new RighttoobtaininformationFileViewModel
                        {
                            FileType = f.FileType,
                            FileName = f.FileName,
                            FileUrl = "/images/Righttoobtaininformation/" + f.FileName
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return request;
        }
    }
}

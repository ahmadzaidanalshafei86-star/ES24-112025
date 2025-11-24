

using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging;
using ES.Core.Enums;
using ES.Web.Areas.EsAdmin.Models;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class TendersRepository
    {
        private readonly ApplicationDbContext _context;

        public TendersRepository(ApplicationDbContext context)
        {
            _context = context;
           
        }
        public async Task<Tender> GetTenderByIdAsync(int id)
        {
            var tender = await _context.Tenders
                        .Include(p => p.Materials)
                        .SingleOrDefaultAsync(p => p.Id == id);
            return tender;



        }
        public async Task<Tender> GetTenderByIdWithTranslationsAsync(int tenderId)
        {
            var tender = await _context.Tenders
                 .Include(c => c.Language)
                 .Include(c => c.TenderTranslates!)
                 .ThenInclude(c => c.Language)
                 .SingleOrDefaultAsync(c => c.Id == tenderId);

            if (tender == null)
                throw new Exception(message: "Tender not found");

            return tender;
        }


        public async Task<Tender> GetTenderWithGalleryImagesAsync(int id)
        {
            var tender = await _context.Tenders
                        .Include(p => p.TenderFiles)
                        .Include(p => p.Materials)
                        .SingleOrDefaultAsync(p => p.Id == id);
            return tender;
        }

        public async Task<IEnumerable<TenderViewModel>> GetAllTendersAsync()
        {
            return await _context.Tenders
                .Include(t => t.Material)
                        .Select(t => new TenderViewModel
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Code = t.Code,
                            CopyPrice = t.CopyPrice,
                            StartDate = t.StartDate,
                            EndDate = t.EndDate,
                            Publish = t.Publish,
                            MoveToArchive = t.MoveToArchive,
                            MaterialId = t.Material != null ? t.Material.Id : 0,
                            MaterialName = t.Material != null ? t.Material.Name : ""

                        })
                        .ToListAsync();
        }

        public async Task<int> AddTenderAsync(Tender tender)
        {
            await _context.Tenders.AddAsync(tender);
            await _context.SaveChangesAsync();
            return tender.Id;
        }

        public async Task AddRelatedMaterialsAsync(Tender tender, List<int> relatedMaterialsIds)
        {

            // 1. Remove existing relations
            var existing = await _context.TenderMaterials
                .Where(tm => tm.TenderId == tender.Id)
                .ToListAsync();

            _context.TenderMaterials.RemoveRange(existing);

            // 2. Add new relations
            var newRelations = relatedMaterialsIds
                .Select(materialId => new TenderMaterial
                {
                    TenderId = tender.Id,
                    MaterialId = materialId
                })
                .ToList();

            await _context.TenderMaterials.AddRangeAsync(newRelations);

            await _context.SaveChangesAsync();
        }

        public async Task<TenderFormViewModel> InitializeTenderFormViewModelAsync(TenderFormViewModel? model = null)
        {
            bool isNew = model == null || model.Id == 0;

            model ??= new TenderFormViewModel();

            // Set default dates only for "Create"
            if (isNew)
            {
                model.StartDate = DateTime.Today;
                model.EndDate = DateTime.Today;
                model.EnvelopeOpeningDate = DateTime.Today;
                model.LastCopyPurchaseDate = DateTime.Today;
            }

            model.SortingTypes = GetSelectListFromEnum<TypeOfSorting>();
            model.Materials = await GetMaterialsNamesAsync();
            return model;
        }
      
        public void Updatetender(Tender tender)
        {
            _context.Update(tender);
            _context.SaveChanges();
        }

        public void Deletetender(Tender tender)
        {
            // 1. Delete TenderMaterials
            var materials = _context.TenderMaterials
                .Where(m => m.TenderId == tender.Id);
            _context.TenderMaterials.RemoveRange(materials);

           
            // 3. Delete the tender itself
            _context.Tenders.Remove(tender);

            _context.SaveChanges();
        }



        private List<SelectListItem> GetSelectListFromEnum<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)(object)e).ToString(),
                    Text = e.ToString()
                }).ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetMaterialsNamesAsync()
        {
            return await _context.Materials
             .Select(pc => new SelectListItem
             {
                 Value = pc.Id.ToString(),
                 Text = pc.Name
             })
             .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetTendersSlugesAsync()
        {
            return await _context.Tenders
             .Select(pc => new SelectListItem
             {
                 Value = pc.Slug,
                 Text = pc.Title
             })
             .ToListAsync();
        }
    }
}

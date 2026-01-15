using Microsoft.AspNetCore.Mvc.Rendering;
using ES.Core.Enums;
using ES.Web.Areas.EsAdmin.Models;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class PurchaseOrdersRepository
    {
        private readonly ApplicationDbContext _context;

        public PurchaseOrdersRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdAsync(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                        .Include(p => p.Materials)
                        .SingleOrDefaultAsync(p => p.Id == id);
            return purchaseOrder;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderByIdWithTranslationsAsync(int purchaseOrderId)
        {
            var purchaseOrder = await _context.PurchaseOrders
                 .Include(c => c.Language)
                 .Include(c => c.PurchaseOrderTranslates!)
                 .ThenInclude(c => c.Language)
                 .SingleOrDefaultAsync(c => c.Id == purchaseOrderId);

            if (purchaseOrder == null)
                throw new Exception("PurchaseOrder not found");

            return purchaseOrder;
        }

        public async Task<PurchaseOrder> GetPurchaseOrderWithGalleryImagesAsync(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders
                        .Include(p => p.PurchaseOrderFiles)
                        .Include(p => p.Materials)
                        .SingleOrDefaultAsync(p => p.Id == id);
            return purchaseOrder;
        }

        public async Task<IEnumerable<PurchaseOrderViewModel>> GetAllPurchaseOrdersAsync()
        {
            return await _context.PurchaseOrders
                .Include(t => t.Material)
                .Select(t => new PurchaseOrderViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Code = t.Code,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    Publish = t.Publish,
                    MoveToArchive = t.MoveToArchive,
                    MaterialId = t.Material != null ? t.Material.Id : 0,
                    MaterialName = t.Material != null ? t.Material.Name : ""
                })
                .ToListAsync();
        }

        public async Task<int> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder)
        {
            await _context.PurchaseOrders.AddAsync(purchaseOrder);
            await _context.SaveChangesAsync();
            return purchaseOrder.Id;
        }

        public async Task AddRelatedMaterialsAsync(PurchaseOrder purchaseOrder, List<int> relatedMaterialsIds)
        {
            // 1. Remove existing relations
            var existing = await _context.PurchaseOrderMaterials
                .Where(tm => tm.PurchaseOrderId == purchaseOrder.Id)
                .ToListAsync();
            _context.PurchaseOrderMaterials.RemoveRange(existing);

            // 2. Add new relations
            var newRelations = relatedMaterialsIds
                .Select(materialId => new PurchaseOrderMaterial
                {
                    PurchaseOrderId = purchaseOrder.Id,
                    MaterialId = materialId
                })
                .ToList();

            await _context.PurchaseOrderMaterials.AddRangeAsync(newRelations);
            await _context.SaveChangesAsync();
        }

        public async Task<PurchaseOrderFormViewModel> InitializePurchaseOrderFormViewModelAsync(PurchaseOrderFormViewModel? model = null)
        {
            bool isNew = model == null || model.Id == 0;

            model ??= new PurchaseOrderFormViewModel();

            // Set default dates only for "Create"
            if (isNew)
            {
                model.StartDate = DateTime.Today;
                model.EndDate = DateTime.Today;
                model.EnvelopeOpeningDate = DateTime.Today;
            }

            model.SortingTypes = GetSelectListFromEnum<TypeOfSorting>();
            model.Materials = await GetMaterialsNamesAsync();
            return model;
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            _context.Update(purchaseOrder);
            _context.SaveChanges();
        }

        public void DeletePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            // 1. Delete PurchaseOrderMaterials
            var materials = _context.PurchaseOrderMaterials
                .Where(m => m.PurchaseOrderId == purchaseOrder.Id);
            _context.PurchaseOrderMaterials.RemoveRange(materials);

            // 2. Delete the purchase order itself
            _context.PurchaseOrders.Remove(purchaseOrder);

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

        public async Task<IEnumerable<SelectListItem>> GetPurchaseOrdersSlugesAsync()
        {
            return await _context.PurchaseOrders
             .Select(pc => new SelectListItem
             {
                 Value = pc.Slug,
                 Text = pc.Title
             })
             .ToListAsync();
        }
    }
}






using ES.Core.Entities;
using ES.Core.Enums;
using ES.Web.Areas.EsAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ES.Web.Areas.EsAdmin.Repositories
{
    public class BranchesRepository
    {
        private readonly ApplicationDbContext _context;
        public BranchesRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<BranchesViewModel>> GetBranchesWithParentInfoAsync()
        {
            var Branches = await _context.Branches
                .Select(c => new BranchesViewModel
                {
                    Id = c.Id,
                    Slug = c.Slug,
                    Name = c.Name

                })
                .ToListAsync();

            return Branches;
            //return await _context.branchViewModel
            //.FromSqlRaw("EXEC GetBranchesWithParentInfo")
            //.ToListAsync();

        }

        public async Task<Branch> GetBranchByIdAsync(int id)
        {
            var branch = await _context.Branches
            .FirstOrDefaultAsync(c => c.Id == id); // Filter by specific material

            if (branch == null)
                throw new Exception(message: "Branch not found");

            return branch;
        }

        public async Task<Branch> GetBranchByIdWithTranslationsAsync(int branchId)
        {
            var branch = await _context.Branches
                .Include(c => c.Language)
                .Include(c => c.BranchesTranslates!)
                .ThenInclude(c => c.Language)
                .SingleOrDefaultAsync(c => c.Id == branchId);

            if (branch == null)
                throw new Exception(message: "Branch not found");

            return branch;
        }

        public async Task<Branch> GetbranchWithAllDataAsync(int branchId)
        {
            var branch = await _context.Branches
                .Include(b => b.RelatedHangars)
                    .ThenInclude(h => h.HangarsTranslates)
                .Include(b => b.RelatedRefrigators)
                    .ThenInclude(r => r.RefrigatorsTranslates)
                .SingleOrDefaultAsync(c => c.Id == branchId);

            if (branch == null)
                throw new Exception("branch not found");

            return branch;
        }



        public async Task<IEnumerable<SelectListItem>> GetBranchesSlugsNamesAsync()
        {
            return await _context.Branches
             .Select(pc => new SelectListItem
             {
                 Value = pc.Slug,
                 Text = pc.Name
             })
             .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBranchesNamesAsync()
        {
            return await _context.Branches
             .Select(pc => new SelectListItem
             {
                 Value = pc.Id.ToString(),
                 Text = pc.Name
             })
             .ToListAsync();
        }




        public async Task<int> AddbranchAsync(Branch branch)
        {
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
            return branch.Id;
        }// Return the ID of the newly created branch

        //    var idParameter = new SqlParameter
        //    {
        //        ParameterName = "@Id",
        //        SqlDbType = System.Data.SqlDbType.Int,
        //        Direction = System.Data.ParameterDirection.Output
        //    };

        //    await _context.Database.ExecuteSqlRawAsync(
        //        @"EXEC Addmaterial 
        //    @Name, @LongDescription, @ShortDescription, @FeaturedImageUrl, @FeaturedImageAltName,
        //    @CoverImageUrl, @CoverImageAltName, @MetaDescription, @MetaKeywords, @Order, @TypeOfSorting,
        //    @CreatedDate, @ParentbranchId, @LanguageId, @ThemeId, @IsPublished, @Id OUTPUT",
        //        new SqlParameter("@Name", material.Name),
        //        new SqlParameter("@LongDescription", material.LongDescription ?? (object)DBNull.Value),
        //        new SqlParameter("@ShortDescription", material.ShortDescription ?? (object)DBNull.Value),
        //        new SqlParameter("@FeaturedImageUrl", material.FeaturedImageUrl ?? (object)DBNull.Value),
        //        new SqlParameter("@FeaturedImageAltName", material.FeaturedImageAltName ?? (object)DBNull.Value),
        //        new SqlParameter("@CoverImageUrl", material.CoverImageUrl ?? (object)DBNull.Value),
        //        new SqlParameter("@CoverImageAltName", material.CoverImageAltName ?? (object)DBNull.Value),
        //        new SqlParameter("@MetaDescription", material.MetaDescription ?? (object)DBNull.Value),
        //        new SqlParameter("@MetaKeywords", material.MetaKeywords ?? (object)DBNull.Value),
        //        new SqlParameter("@Order", material.Order),
        //        new SqlParameter("@TypeOfSorting", (int)material.TypeOfSorting),
        //        new SqlParameter("@CreatedDate", material.CreatedDate),
        //        new SqlParameter("@ParentbranchId", material.ParentbranchId ?? (object)DBNull.Value),
        //        new SqlParameter("@LanguageId", material.LanguageId),
        //        new SqlParameter("@ThemeId", material.ThemeId),
        //        new SqlParameter("@IsPublished", material.IsPublished),
        //        idParameter
        //    );

        //    return (int)idParameter.Value;
        //}

        public async Task AddRelatedBranchesAsync(Branch branch, List<int> relatedbranchIds)
        {
            foreach (var relatedbranchId in relatedbranchIds)
            {
                var relatedbranch = await _context.Branches.FindAsync(relatedbranchId);

            }
        }

        public async Task<bool> DeletebranchAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
                return false;

            _context.Branches.Remove(branch);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }


        public async Task<IEnumerable<SelectListItem>> GetThemesAsync()
        {
            return await _context.Themes
           .Select(th => new SelectListItem
           {
               Value = th.Id.ToString(),
               Text = th.ThemeName
           })
           .OrderBy(th => th.Value)
           .ToListAsync();
        }
        public async Task<IEnumerable<SelectListItem>> GetLanguagesAsync()
        {
            return await _context.Languages
                .Select(l => new SelectListItem
                {
                    Value = l.Id.ToString(),
                    Text = l.Name
                })
                .ToListAsync();
        }
        public async Task<BranchFormViewModel> InitializebranchFormViewModelAsync(BranchFormViewModel? model = null)
        {
            model ??= new BranchFormViewModel(); // Initialize a new model if none is provided.

            // Populate dropdowns
            model.SortingTypes = GetSelectListFromEnum<TypeOfSorting>();
            model.Branches = await GetBranchesNamesAsync();
            model.Languages = await GetLanguagesAsync(); // Make sure this method returns SelectListItems

            if (model.Id.HasValue)
            {
                var branch = await GetbranchWithAllDataAsync(model.Id.Value);

                // Map Hangars and their translations
                model.Hangars = branch.RelatedHangars?
                    .Select(h => new HangarDto
                    {
                        Id = h.Id,
                        Name = h.Name,
                        Size = h.Size,
                        Type = h.Type,
                        Translates = h.HangarsTranslates?
                            .Select(ht => new HangarTranslateDto
                            {
                                Name = ht.Name,
                                Size = ht.Size,
                                Type = ht.Type,
                                LanguageId = ht.LanguageId
                            }).ToList() ?? new List<HangarTranslateDto>()
                    }).ToList() ?? new List<HangarDto>();

                // Map Refrigators and their translations
                model.Refrigators = branch.RelatedRefrigators?
                    .Select(r => new RefrigatorDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Size = r.Size,
                        Type = r.Type,
                        Translates = r.RefrigatorsTranslates?
                            .Select(rt => new RefrigatorTranslateDto
                            {
                                Name = rt.Name,
                                Size = rt.Size,
                                Type = rt.Type,
                                LanguageId = rt.LanguageId
                            }).ToList() ?? new List<RefrigatorTranslateDto>()
                    }).ToList() ?? new List<RefrigatorDto>();
            }

            return model;
        }

     

        public bool IsParentbranch(int branchId)
        {
            var childBranches = _context.Branches;
            if (childBranches.Any())
                return true;

            return false;
        }

        public bool IsRelatedToAnotherbranch(int branchId)
        {
            var IsRelated = _context.Branches;


            return false;
        }

        public void Updatebranch(Branch branch)
        {
            _context.Branches.Update(branch);
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

        #region Hangar Methods

        public async Task AddHangarAsync(Hangar hangar)
        {
            await _context.Hangars.AddAsync(hangar);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHangarsByBranchIdAsync(int branchId)
        {
            var hangars = _context.Hangars.Where(h => h.BranchId == branchId);
            _context.Hangars.RemoveRange(hangars);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Hangar>> GetHangarsByBranchIdAsync(int branchId)
        {
            return await _context.Hangars
                .Where(h => h.BranchId == branchId)
                .ToListAsync();
        }

        #endregion

        #region Refrigator Methods

        public async Task AddRefrigatorAsync(Refrigator refrigator)
        {
            await _context.Refrigators.AddAsync(refrigator);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRefrigatorsByBranchIdAsync(int branchId)
        {
            var refrigators = _context.Refrigators.Where(r => r.BranchId == branchId);
            _context.Refrigators.RemoveRange(refrigators);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Refrigator>> GetRefrigatorsByBranchIdAsync(int branchId)
        {
            return await _context.Refrigators
                .Where(r => r.BranchId == branchId)
                .ToListAsync();
        }

        #endregion

        public async Task AddHangarTranslationAsync(int hangarId, HangarTranslateDto dto)
        {
            if (dto == null) return;

            var translation = new HangarTranslate
            {
                HangarId = hangarId,
                LanguageId = dto.LanguageId,
                Name = dto.Name ?? string.Empty,
                Size = dto.Size ?? string.Empty,
                Type = dto.Type ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.HangarsTranslate.Add(translation);
            await _context.SaveChangesAsync();
        }

        public async Task AddRefrigatorTranslationAsync(int refrigatorId, RefrigatorTranslateDto dto)
        {
            if (dto == null) return;

            var translation = new RefrigatorTranslate
            {
                RefrigatorId = refrigatorId,
                LanguageId = dto.LanguageId,
                Name = dto.Name ?? string.Empty,
                Size = dto.Size ?? string.Empty,
                Type = dto.Type ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            _context.RefrigatorsTranslate.Add(translation);
            await _context.SaveChangesAsync();
        }



    }
}



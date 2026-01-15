using ES.Core.Entities;
using ES.Core.Enums;
using ES.Web.Helpers;
using ES.Web.Models;
using Microsoft.Data.SqlClient;
using NuGet.Packaging;
using System.Data;

namespace ES.Web.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        //public async Task<CategoryWithPagesViewModel?> GetCategoryWithPagesBySlugAsync(string Categoryslug, int Take = int.MaxValue)
        //{
        //    var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

        //    var parameters = new[]
        //    {
        //        new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = Categoryslug },
        //        new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
        //    };

        //    await _context.Database.OpenConnectionAsync();

        //    using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
        //    command.CommandText = "GetCategoryWithPagesBySlug";
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.AddRange(parameters);

        //    using var reader = await command.ExecuteReaderAsync();

        //    // --- Result set 1: Main category ---
        //    if (!await reader.ReadAsync())
        //        return null;

        //    var category = new
        //    {
        //        Id = reader.GetInt32(reader.GetOrdinal("Id")),
        //        Slug = reader.GetString(reader.GetOrdinal("Slug")),
        //        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
        //        ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
        //        LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
        //        CoverImageUrl = reader.IsDBNull(reader.GetOrdinal("CoverImageUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverImageUrl")),
        //        CoverImageAltName = reader.IsDBNull(reader.GetOrdinal("CoverImageAltName")) ? null : reader.GetString(reader.GetOrdinal("CoverImageAltName")),
        //        FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeaturedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageUrl")),
        //        FeaturedImageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName")),
        //        TypeOfSorting = reader.IsDBNull(reader.GetOrdinal("TypeOfSorting")) ? "Manual" : reader.GetString(reader.GetOrdinal("TypeOfSorting")),
        //        ThemeName = reader.IsDBNull(reader.GetOrdinal("ThemeName")) ? null : reader.GetString(reader.GetOrdinal("ThemeName"))
        //    };

        //    // --- Result set 2: Category translations ---
        //    await reader.NextResultAsync();
        //    string? translatedName = null, translatedShortDesc = null, translatedLongDesc = null;

        //    while (await reader.ReadAsync())
        //    {
        //        if (reader.GetInt32("LanguageId") == languageId)
        //        {
        //            translatedName = reader.IsDBNull("Name") ? null : reader.GetString("Name");
        //            translatedShortDesc = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription");
        //            translatedLongDesc = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription");
        //            break;
        //        }
        //    }

        //    // --- Result set 3: Related pages ---
        //    await reader.NextResultAsync();
        //    var pages = new List<PageViewModel>();
        //    var typeOfSorting = Enum.TryParse<TypeOfSorting>(category.TypeOfSorting, true, out var parsedSorting) 
        //        ? parsedSorting 
        //        : TypeOfSorting.Manual;

        //    while (await reader.ReadAsync())
        //    {
        //        pages.Add(new PageViewModel
        //        {
        //            Slug = reader.GetString("Slug"),
        //            Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
        //            FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
        //            FeaturedImageUrl = reader.IsDBNull("FeatruedImageUrl") ? null : reader.GetString("FeatruedImageUrl"), // Note: Using actual column name from DB
        //            ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
        //            LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
        //            VideoUrl = reader.IsDBNull("VideoURL") ? null : reader.GetString("VideoURL"),
        //            Order = reader.GetInt32("Order"),
        //            CreatedAt = reader.GetDateTime("CreatedDate"),
        //            DateInput = reader.IsDBNull("DateInput") ? null : reader.GetDateTime("DateInput"),
        //            Count = reader.IsDBNull("Count") ? 0 : reader.GetInt32("Count"),
        //            GalleryStyle = reader.IsDBNull(reader.GetOrdinal("GalleryStyle"))
        //            ? null
        //            : reader.GetString(reader.GetOrdinal("GalleryStyle")), // ✅ fixed: string
        //            LinkUrl = reader.IsDBNull("LinkToUrl") ? null : reader.GetString("LinkToUrl")
        //        });
        //    }

        //    // --- Result set 4: Page translations ---
        //    await reader.NextResultAsync();
        //    var pageTranslations = new Dictionary<string, (string? Title, string? ShortDescription, string? LongDescription)>();

        //    while (await reader.ReadAsync())
        //    {
        //        if (reader.GetInt32("LanguageId") == languageId)
        //        {
        //            var slug = reader.GetString("Slug");
        //            pageTranslations[slug] = (
        //                reader.IsDBNull("Title") ? null : reader.GetString("Title"),
        //                reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
        //                reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription")
        //            );
        //        }
        //    }


        //      // --- Result set 3: Gallery images ---
        //    await reader.NextResultAsync();
        //    var galleryImages = new List<GalleryImageViewModel>();

        //    while (await reader.ReadAsync())
        //    {
        //        galleryImages.Add(new GalleryImageViewModel
        //        {
        //            GalleryImageAltName = reader.GetString("AltName"),
        //            GalleryImageUrl = reader.GetString("GalleryImageUrl")
        //        });
        //    }

        //    // Apply translations to pages
        //    foreach (var page in pages)
        //    {
        //        if (pageTranslations.TryGetValue(page.Slug, out var translation))
        //        {
        //            page.Title = translation.Title ?? page.Title;
        //            page.ShortDescription = translation.ShortDescription ?? page.ShortDescription;
        //            page.LongDescription = translation.LongDescription ?? page.LongDescription;

        //            page.GalleryImages = galleryImages;
        //            page.GalleryStyle = page.GalleryStyle;
        //        }
        //    }

        //    // Apply sorting
        //    var sortedPages = typeOfSorting switch
        //    {
        //        TypeOfSorting.Manual => pages.OrderBy(p => p.Order).ToList(),
        //        TypeOfSorting.NewToOld => pages.OrderByDescending(p => p.CreatedAt).ToList(),
        //        TypeOfSorting.OldToNew => pages.OrderBy(p => p.CreatedAt).ToList(),
        //        TypeOfSorting.Alphabetical => pages.OrderBy(p => p.Title).ToList(),
        //        TypeOfSorting.AlphabeticalReversed => pages.OrderByDescending(p => p.Title).ToList(),
        //        _ => pages.OrderBy(p => p.Order).ToList()
        //    };

        //    if (Take < int.MaxValue)
        //        sortedPages = sortedPages.OrderByDescending(p => p.CreatedAt).Take(Take).ToList();

        //    var viewModel = new CategoryWithPagesViewModel
        //    {
        //        Slug = category.Slug,
        //        Name = translatedName ?? category.Name,
        //        ShortDescription = translatedShortDesc ?? category.ShortDescription,
        //        LongDescription = translatedLongDesc ?? category.LongDescription,
        //        CoverImageUrl = category.CoverImageUrl,
        //        CoverimageAltName = category.CoverImageAltName,
        //        FeaturedImageUrl = category.FeaturedImageUrl,
        //        FeaturedimageAltName = category.FeaturedImageAltName,
        //        ThemeName = category.ThemeName,
        //        Pages = sortedPages
        //    };

        //    return viewModel;
        //}

        //    public async Task<CategoryWithPagesViewModel?> GetCategoryWithPagesBySlugAsync(string Categoryslug, int Take = int.MaxValue)
        //    {
        //        var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

        //        var parameters = new[]
        //        {
        //    new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = Categoryslug },
        //    new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
        //};

        //        await _context.Database.OpenConnectionAsync();

        //        using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
        //        command.CommandText = "GetCategoryWithPagesBySlug";
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddRange(parameters);

        //        using var reader = await command.ExecuteReaderAsync();

        //        // ---------------------------------------------
        //        // 1) Main Category
        //        // ---------------------------------------------
        //        if (!await reader.ReadAsync())
        //            return null;

        //        var category = new
        //        {
        //            Id = reader.GetInt32("Id"),
        //            Slug = reader.GetString("Slug"),
        //            Name = reader.IsDBNull("Name") ? null : reader.GetString("Name"),
        //            ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
        //            LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
        //            CoverImageUrl = reader.IsDBNull("CoverImageUrl") ? null : reader.GetString("CoverImageUrl"),
        //            CoverImageAltName = reader.IsDBNull("CoverImageAltName") ? null : reader.GetString("CoverImageAltName"),
        //            FeaturedImageUrl = reader.IsDBNull("FeaturedImageUrl") ? null : reader.GetString("FeaturedImageUrl"),
        //            FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
        //            TypeOfSorting = reader.IsDBNull("TypeOfSorting") ? "Manual" : reader.GetString("TypeOfSorting"),
        //            ThemeName = reader.IsDBNull("ThemeName") ? null : reader.GetString("ThemeName")
        //        };

        //        // ---------------------------------------------
        //        // 2) Category Translations
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();

        //        string? translatedName = null, translatedShortDesc = null, translatedLongDesc = null;

        //        while (await reader.ReadAsync())
        //        {
        //            if (reader.GetInt32("LanguageId") == languageId)
        //            {
        //                translatedName = reader.IsDBNull("Name") ? null : reader.GetString("Name");
        //                translatedShortDesc = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription");
        //                translatedLongDesc = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription");
        //                break;
        //            }
        //        }

        //        // ---------------------------------------------
        //        // 3) Pages
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();

        //        var pages = new List<PageViewModel>();

        //        var typeOfSorting = Enum.TryParse<TypeOfSorting>(category.TypeOfSorting, true, out var parsedSorting)
        //            ? parsedSorting
        //            : TypeOfSorting.Manual;

        //        while (await reader.ReadAsync())
        //        {
        //            pages.Add(new PageViewModel
        //            {
        //                Slug = reader.GetString("Slug"),
        //                Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
        //                FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
        //                FeaturedImageUrl = reader.IsDBNull("FeatruedImageUrl") ? null : reader.GetString("FeatruedImageUrl"),
        //                ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
        //                LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
        //                VideoUrl = reader.IsDBNull("VideoURL") ? null : reader.GetString("VideoURL"),
        //                Order = reader.GetInt32("Order"),
        //                CreatedAt = reader.GetDateTime("CreatedDate"),
        //                DateInput = reader.IsDBNull("DateInput") ? null : reader.GetDateTime("DateInput"),
        //                Count = reader.IsDBNull("Count") ? 0 : reader.GetInt32("Count"),
        //                LinkUrl = reader.IsDBNull("LinkToUrl") ? null : reader.GetString("LinkToUrl")
        //            });
        //        }

        //        // ---------------------------------------------
        //        // 4) Page Translations
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();

        //        var pageTranslations = new Dictionary<string, (string? Title, string? ShortDescription, string? LongDescription)>();

        //        while (await reader.ReadAsync())
        //        {
        //            if (reader.GetInt32("LanguageId") == languageId)
        //            {
        //                var slug = reader.GetString("Slug");

        //                pageTranslations[slug] = (
        //                    reader.IsDBNull("Title") ? null : reader.GetString("Title"),
        //                    reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
        //                    reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription")
        //                );
        //            }
        //        }

        //        // ---------------------------------------------
        //        // 5) Gallery Images (Per Page)
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();

        //        var galleryImages = new List<GalleryImageViewModel>();

        //        while (await reader.ReadAsync())
        //        {
        //            galleryImages.Add(new GalleryImageViewModel
        //            {
        //                PageId = reader.GetInt32("PageId"),
        //                GalleryImageAltName = reader.GetString("AltName"),
        //                GalleryImageUrl = reader.GetString("GalleryImageUrl")
        //            });
        //        }

        //        // ---------------------------------------------
        //        // 6) Attach Translations + Gallery to pages
        //        // ---------------------------------------------
        //        foreach (var page in pages)
        //        {
        //            // Apply text translation
        //            if (pageTranslations.TryGetValue(page.Slug, out var translation))
        //            {
        //                page.Title = translation.Title ?? page.Title;
        //                page.ShortDescription = translation.ShortDescription ?? page.ShortDescription;
        //                page.LongDescription = translation.LongDescription ?? page.LongDescription;
        //            }

        //            // Attach gallery images specific to this page
        //            page.GalleryImages = galleryImages
        //                .Where(x => x.PageId == page.id)
        //                .ToList();
        //        }

        //        // ---------------------------------------------
        //        // 7) Sorting
        //        // ---------------------------------------------
        //        var sortedPages = typeOfSorting switch
        //        {
        //            TypeOfSorting.Manual => pages.OrderBy(p => p.Order).ToList(),
        //            TypeOfSorting.NewToOld => pages.OrderByDescending(p => p.CreatedAt).ToList(),
        //            TypeOfSorting.OldToNew => pages.OrderBy(p => p.CreatedAt).ToList(),
        //            TypeOfSorting.Alphabetical => pages.OrderBy(p => p.Title).ToList(),
        //            TypeOfSorting.AlphabeticalReversed => pages.OrderByDescending(p => p.Title).ToList(),
        //            _ => pages.OrderBy(p => p.Order).ToList()
        //        };

        //        if (Take < int.MaxValue)
        //            sortedPages = sortedPages
        //                .OrderByDescending(p => p.CreatedAt)
        //                .Take(Take)
        //                .ToList();

        //        // ---------------------------------------------
        //        // 8) Return ViewModel
        //        // ---------------------------------------------
        //        return new CategoryWithPagesViewModel
        //        {
        //            Slug = category.Slug,
        //            Name = translatedName ?? category.Name,
        //            ShortDescription = translatedShortDesc ?? category.ShortDescription,
        //            LongDescription = translatedLongDesc ?? category.LongDescription,
        //            CoverImageUrl = category.CoverImageUrl,
        //            CoverimageAltName = category.CoverImageAltName,
        //            FeaturedImageUrl = category.FeaturedImageUrl,
        //            FeaturedimageAltName = category.FeaturedImageAltName,
        //            ThemeName = category.ThemeName,
        //            Pages = sortedPages
        //        };
        //    }
        //    public async Task<CategoryWithPagesViewModel?> GetCategoryWithPagesBySlugAsync(string Categoryslug, int Take = int.MaxValue)
        //    {
        //        var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

        //        var parameters = new[]
        //        {
        //    new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = Categoryslug },
        //    new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
        //};

        //        await _context.Database.OpenConnectionAsync();

        //        using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
        //        command.CommandText = "GetCategoryWithPagesBySlug";
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddRange(parameters);

        //        using var reader = await command.ExecuteReaderAsync();

        //        // ---------------------------------------------
        //        // 1) Main Category
        //        // ---------------------------------------------
        //        if (!await reader.ReadAsync())
        //            return null;

        //        var category = new
        //        {
        //            Id = reader.GetInt32("Id"),
        //            Slug = reader.GetString("Slug"),
        //            Name = reader.IsDBNull("Name") ? null : reader.GetString("Name"),
        //            ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
        //            LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
        //            CoverImageUrl = reader.IsDBNull("CoverImageUrl") ? null : reader.GetString("CoverImageUrl"),
        //            CoverImageAltName = reader.IsDBNull("CoverImageAltName") ? null : reader.GetString("CoverImageAltName"),
        //            FeaturedImageUrl = reader.IsDBNull("FeaturedImageUrl") ? null : reader.GetString("FeaturedImageUrl"),
        //            FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
        //            TypeOfSorting = reader.IsDBNull("TypeOfSorting") ? "Manual" : reader.GetString("TypeOfSorting"),
        //            ThemeName = reader.IsDBNull("ThemeName") ? null : reader.GetString("ThemeName")
        //        };

        //        // ---------------------------------------------
        //        // 2) Category Translations
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();
        //        string? translatedName = null, translatedShortDesc = null, translatedLongDesc = null;

        //        while (await reader.ReadAsync())
        //        {
        //            if (reader.GetInt32(reader.GetOrdinal("LanguageId")) == languageId)
        //            {
        //                translatedName = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name"));
        //                translatedShortDesc = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription"));
        //                translatedLongDesc = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription"));
        //                break;
        //            }
        //        }

        //        // ---------------------------------------------
        //        // 3) Pages
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();
        //        var pages = new List<PageViewModel>();

        //        var typeOfSorting = Enum.TryParse<TypeOfSorting>(category.TypeOfSorting, true, out var parsedSorting)
        //            ? parsedSorting
        //            : TypeOfSorting.Manual;

        //        while (await reader.ReadAsync())
        //        {
        //            pages.Add(new PageViewModel
        //            {
        //                id = reader.GetInt32(reader.GetOrdinal("Id")), // ← مهم
        //                Slug = reader.GetString(reader.GetOrdinal("Slug")),
        //                Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
        //                FeaturedImageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName")),
        //                FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeatruedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeatruedImageUrl")),
        //                ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
        //                LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
        //                VideoUrl = reader.IsDBNull(reader.GetOrdinal("VideoURL")) ? null : reader.GetString(reader.GetOrdinal("VideoURL")),
        //                Order = reader.GetInt32(reader.GetOrdinal("Order")),
        //                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
        //                DateInput = reader.IsDBNull(reader.GetOrdinal("DateInput")) ? null : reader.GetDateTime(reader.GetOrdinal("DateInput")),
        //                Count = reader.IsDBNull(reader.GetOrdinal("Count")) ? 0 : reader.GetInt32(reader.GetOrdinal("Count")),
        //                LinkUrl = reader.IsDBNull(reader.GetOrdinal("LinkToUrl")) ? null : reader.GetString(reader.GetOrdinal("LinkToUrl")),
        //                GalleryStyle = reader.IsDBNull(reader.GetOrdinal("GalleryStyle")) ? null : reader.GetString(reader.GetOrdinal("GalleryStyle")),
        //                GalleryImages = new List<GalleryImageViewModel>() // Initialize empty list
        //            });
        //        }

        //        // ---------------------------------------------
        //        // 4) Page Translations
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();
        //        var pageTranslations = new Dictionary<string, (string? Title, string? ShortDescription, string? LongDescription)>();

        //        while (await reader.ReadAsync())
        //        {
        //            if (reader.GetInt32(reader.GetOrdinal("LanguageId")) == languageId)
        //            {
        //                var slug = reader.GetString(reader.GetOrdinal("Slug"));
        //                pageTranslations[slug] = (
        //                    reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
        //                    reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
        //                    reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription"))
        //                );
        //            }
        //        }

        //        // ---------------------------------------------
        //        // 5) Gallery Images
        //        // ---------------------------------------------
        //        await reader.NextResultAsync();
        //        var galleryImages = new List<GalleryImageViewModel>();

        //        while (await reader.ReadAsync())
        //        {
        //            galleryImages.Add(new GalleryImageViewModel
        //            {
        //                GalleryImageAltName = reader.GetString(reader.GetOrdinal("AltName")),
        //                GalleryImageUrl = reader.GetString(reader.GetOrdinal("GalleryImageUrl")),
        //                DisplayOrder = reader.GetInt32(reader.GetOrdinal("DisplayOrder")),
        //                PageId = reader.GetInt32(reader.GetOrdinal("PageId")) // Add this property to your GalleryImageViewModel
        //            });
        //        }

        //        // ---------------------------------------------
        //        // 6) Attach Translations + Gallery to pages
        //        // ---------------------------------------------
        //        foreach (var page in pages)
        //        {
        //            // Apply translations
        //            if (pageTranslations.TryGetValue(page.Slug, out var translation))
        //            {
        //                page.Title = translation.Title ?? page.Title;
        //                page.ShortDescription = translation.ShortDescription ?? page.ShortDescription;
        //                page.LongDescription = translation.LongDescription ?? page.LongDescription;
        //            }

        //            // Attach gallery images
        //            page.GalleryImages = galleryImages
        //     .Where(x => x.PageId == page.id) // ← الربط الصحيح
        //     .OrderBy(x => x.DisplayOrder)
        //     .ToList();
        //        }

        //        // ---------------------------------------------
        //        // 7) Sorting
        //        // ---------------------------------------------
        //        var sortedPages = typeOfSorting switch
        //        {
        //            TypeOfSorting.Manual => pages.OrderBy(p => p.Order).ToList(),
        //            TypeOfSorting.NewToOld => pages.OrderByDescending(p => p.CreatedAt).ToList(),
        //            TypeOfSorting.OldToNew => pages.OrderBy(p => p.CreatedAt).ToList(),
        //            TypeOfSorting.Alphabetical => pages.OrderBy(p => p.Title).ToList(),
        //            TypeOfSorting.AlphabeticalReversed => pages.OrderByDescending(p => p.Title).ToList(),
        //            _ => pages.OrderBy(p => p.Order).ToList()
        //        };

        //        if (Take < int.MaxValue)
        //            sortedPages = sortedPages.Take(Take).ToList();

        //        // ---------------------------------------------
        //        // 8) Return ViewModel
        //        // ---------------------------------------------
        //        return new CategoryWithPagesViewModel
        //        {
        //            Slug = category.Slug,
        //            Name = translatedName ?? category.Name,
        //            ShortDescription = translatedShortDesc ?? category.ShortDescription,
        //            LongDescription = translatedLongDesc ?? category.LongDescription,
        //            CoverImageUrl = category.CoverImageUrl,
        //            CoverimageAltName = category.CoverImageAltName,
        //            FeaturedImageUrl = category.FeaturedImageUrl,
        //            FeaturedimageAltName = category.FeaturedImageAltName,
        //            ThemeName = category.ThemeName,
        //            Pages = sortedPages
        //        };
        //    }

        public async Task<CategoryWithPagesViewModel?> GetCategoryWithPagesBySlugAsync(
        string Categoryslug, int Take = int.MaxValue)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
        new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = Categoryslug },
        new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
    };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetCategoryWithPagesBySlug";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // ============================================================================
            // 1) MAIN CATEGORY
            // ============================================================================
            if (!await reader.ReadAsync())
                return null;

            var categoryVm = new CategoryWithPagesViewModel
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")), // ← هنا تعيين Id
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Name = reader.IsDBNull("Name") ? null : reader.GetString("Name"),
                ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
                CoverImageUrl = reader.IsDBNull("CoverImageUrl") ? null : reader.GetString("CoverImageUrl"),
                CoverimageAltName = reader.IsDBNull("CoverImageAltName") ? null : reader.GetString("CoverImageAltName"),
                FeaturedImageUrl = reader.IsDBNull("FeaturedImageUrl") ? null : reader.GetString("FeaturedImageUrl"),
                FeaturedimageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
                ThemeName = reader.IsDBNull("ThemeName") ? null : reader.GetString("ThemeName"),
                Pages = new List<PageViewModel>(),
            };

            var mainTypeOfSorting = reader.IsDBNull("TypeOfSorting")
                ? "Manual"
                : reader.GetString("TypeOfSorting");

            // ============================================================================
            // 2) CATEGORY TRANSLATIONS
            // ============================================================================
            await reader.NextResultAsync();

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32(reader.GetOrdinal("LanguageId")) == languageId)
                {
                    categoryVm.Name = reader.IsDBNull("Name") ? categoryVm.Name : reader.GetString("Name");
                    categoryVm.ShortDescription = reader.IsDBNull("ShortDescription") ? categoryVm.ShortDescription : reader.GetString("ShortDescription");
                    categoryVm.LongDescription = reader.IsDBNull("LongDescription") ? categoryVm.LongDescription : reader.GetString("LongDescription");
                    break;
                }
            }

            // ============================================================================
            // 3) PAGES
            // ============================================================================
            await reader.NextResultAsync();
            var pages = new List<PageViewModel>();

            while (await reader.ReadAsync())
            {
                pages.Add(new PageViewModel
                {
                    id = reader.GetInt32("Id"),
                    Slug = reader.GetString("Slug"),
                    Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                    ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                    LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
                    FeaturedImageUrl = reader.IsDBNull("FeatruedImageUrl") ? null : reader.GetString("FeatruedImageUrl"),
                    FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
                    GalleryStyle = reader.IsDBNull("GalleryStyle") ? null : reader.GetString("GalleryStyle"),
                    Order = reader.GetInt32("Order"),
                    CreatedAt = reader.GetDateTime("CreatedDate"),
                    VideoUrl = reader.IsDBNull("VideoURL") ? null : reader.GetString("VideoURL"),
                    DateInput = reader.IsDBNull("DateInput") ? null : reader.GetDateTime("DateInput"),
                    Count = reader.IsDBNull("Count") ? 0 : reader.GetInt32("Count"),
                    LinkUrl = reader.IsDBNull("LinkToUrl") ? null : reader.GetString("LinkToUrl"),
                    GalleryImages = new List<GalleryImageViewModel>()
                });
            }

            // ============================================================================
            // 4) PAGE TRANSLATIONS
            // ============================================================================
            await reader.NextResultAsync();
            var pageTranslations = new Dictionary<string, (string? Title, string? ShortDesc, string? LongDesc)>();

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32("LanguageId") == languageId)
                {
                    pageTranslations[reader.GetString("Slug")] = (
                        reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                        reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                        reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription")
                    );
                }
            }

            // ============================================================================
            // 5) GALLERY IMAGES
            // ============================================================================
            await reader.NextResultAsync();
            var galleryImages = new List<GalleryImageViewModel>();

            while (await reader.ReadAsync())
            {
                galleryImages.Add(new GalleryImageViewModel
                {
                    PageId = reader.GetInt32("PageId"),
                    GalleryImageAltName = reader.GetString("AltName"),
                    GalleryImageUrl = reader.GetString("GalleryImageUrl"),
                    DisplayOrder = reader.GetInt32("DisplayOrder")
                });
            }

            // ATTACH TRANSLATION + IMAGES
            foreach (var page in pages)
            {
                if (pageTranslations.TryGetValue(page.Slug, out var t))
                {
                    page.Title = t.Title ?? page.Title;
                    page.ShortDescription = t.ShortDesc ?? page.ShortDescription;
                    page.LongDescription = t.LongDesc ?? page.LongDescription;
                }

                page.GalleryImages = galleryImages
                    .Where(g => g.PageId == page.id)
                    .OrderBy(g => g.DisplayOrder)
                    .ToList();
            }

            // ============================================================================
            // 6) SUBCATEGORIES
            // ============================================================================
            await reader.NextResultAsync();
            var subcategories = new List<PageViewModel>();

            while (await reader.ReadAsync())
            {
                subcategories.Add(new PageViewModel
                {
                    id = reader.GetInt32("Id"),
                    Slug = reader.GetString("Slug"),
                    Title = reader.GetString("Name"),
                    ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                    LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
                    FeaturedImageUrl = reader.IsDBNull("FeaturedImageUrl") ? null : reader.GetString("FeaturedImageUrl"),
                    FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
                    GalleryStyle = reader.IsDBNull("GalleryStyle") ? null : reader.GetString("GalleryStyle"),
                    ParentCategoryId = reader.GetInt32("ParentCategoryId")
                });
            }

            // ============================================================================
            // 7) SUBCATEGORY TRANSLATIONS
            // ============================================================================
            await reader.NextResultAsync();
            var subcatTranslations = new Dictionary<int, (string?, string?, string?)>();

            while (await reader.ReadAsync())
            {
                var subId = reader.GetInt32(reader.GetOrdinal("SubCategoryId"));
                if (reader.GetInt32("LanguageId") == languageId)
                {
                    subcatTranslations[subId] = (
                        reader.IsDBNull("Name") ? null : reader.GetString("Name"),
                        reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                        reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription")
                    );
                }
            }

            foreach (var sc in subcategories)
            {
                if (subcatTranslations.TryGetValue(sc.id ?? 0, out var t))
                {
                    sc.Title = t.Item1 ?? sc.Title;
                    sc.ShortDescription = t.Item2 ?? sc.ShortDescription;
                    sc.LongDescription = t.Item3 ?? sc.LongDescription;
                }
            }


            // ============================================================================
            // 8) SORT PAGES
            // ============================================================================
            var sorting = Enum.TryParse<TypeOfSorting>(mainTypeOfSorting, true, out var sort)
                ? sort : TypeOfSorting.Manual;

            pages = sorting switch
            {
                TypeOfSorting.Manual => pages.OrderBy(x => x.Order).ToList(),
                TypeOfSorting.NewToOld => pages.OrderByDescending(x => x.CreatedAt).ToList(),
                TypeOfSorting.OldToNew => pages.OrderBy(x => x.CreatedAt).ToList(),
                TypeOfSorting.Alphabetical => pages.OrderBy(x => x.Title).ToList(),
                TypeOfSorting.AlphabeticalReversed => pages.OrderByDescending(x => x.Title).ToList(),
                _ => pages
            };

            if (Take < int.MaxValue)
                pages = pages.Take(Take).ToList();

            // ============================================================================
            // RETURN VIEWMODEL
            // ============================================================================
            categoryVm.Pages = pages;

            return categoryVm;
        }

        //public async Task<CategoryWithPagesViewModel?> GetCategoryWithRelatedNewsAsync(int categoryId, int takePerRelated = 5)
        //{
        //    // جلب الفئة الرئيسية مع RelatedCategories
        //    var categoryEntity = await _context.Categories
        //                                       .Include(c => c.RelatedCategories)
        //                                       .FirstOrDefaultAsync(c => c.Id == categoryId);

        //    if (categoryEntity?.RelatedCategories == null || !categoryEntity.RelatedCategories.Any())
        //        return null;

        //    var relatedNewsVm = new CategoryWithPagesViewModel
        //    {
        //        Name = "Related Categories",
        //        Pages = new List<PageViewModel>()
        //    };

        //    foreach (var relatedCat in categoryEntity.RelatedCategories)
        //    {
        //        // جلب الصفحات لكل فئة مرتبطة باستخدام stored procedure
        //        var relatedCategoryVm = await GetCategoryWithPagesByIdAsync(relatedCat.Id, takePerRelated);

        //        if (relatedCategoryVm?.Pages?.Any() == true)
        //        {
        //            relatedNewsVm.Pages.AddRange(relatedCategoryVm.Pages);
        //        }
        //    }

        //    // إذا لم تُضاف أي صفحة، أرجع null
        //    return relatedNewsVm.Pages.Any() ? relatedNewsVm : null;
        //}
        public async Task<List<CategoryWithPagesViewModel>> GetSubCategoriesByParentIdAsync(int parentCategoryId)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
        new SqlParameter("@ParentCategoryId", SqlDbType.Int) { Value = parentCategoryId },
        new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
    };

            var subCategoriesList = new List<CategoryWithPagesViewModel>();

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetSubCategoriesByParentId";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // --- Result set 1: Subcategories ---
            while (await reader.ReadAsync())
            {
                subCategoriesList.Add(new CategoryWithPagesViewModel
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Slug = reader.GetString(reader.GetOrdinal("Slug")),
                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                    ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
                    LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
                    CoverImageUrl = reader.IsDBNull(reader.GetOrdinal("CoverImageUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverImageUrl")),
                    CoverimageAltName = reader.IsDBNull(reader.GetOrdinal("CoverImageAltName")) ? null : reader.GetString(reader.GetOrdinal("CoverImageAltName")),
                    FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeaturedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageUrl")),
                    FeaturedimageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName")),
                    Pages = new List<PageViewModel>()
                });
            }

            // --- Result set 2: Subcategory translations ---
            if (await reader.NextResultAsync())
            {
                var translations = new Dictionary<int, (string? Name, string? ShortDesc, string? LongDesc)>();
                while (await reader.ReadAsync())
                {
                    var subCatId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                    translations[subCatId] = (
                        reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                        reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
                        reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription"))
                    );
                }

                foreach (var sc in subCategoriesList)
                {
                    if (translations.TryGetValue(sc.Id, out var t))
                    {
                        sc.Name = t.Name ?? sc.Name;
                        sc.ShortDescription = t.ShortDesc ?? sc.ShortDescription;
                        sc.LongDescription = t.LongDesc ?? sc.LongDescription;
                    }
                }
            }

            // --- Result set 3: Pages ---
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    var subCatId = reader.GetInt32(reader.GetOrdinal("CategoryId"));
                    var pageVm = new PageViewModel
                    {
                        id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Slug = reader.GetString(reader.GetOrdinal("Slug")),
                        Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                        ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
                        LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
                        FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeaturedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageUrl")),
                        FeaturedImageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName"))
                    };

                    var parentSubCat = subCategoriesList.FirstOrDefault(sc => sc.Id == subCatId);
                    parentSubCat?.Pages.Add(pageVm);
                }
            }

            // --- Result set 4: Page translations (apply directly if exist) ---
            if (await reader.NextResultAsync())
            {
                while (await reader.ReadAsync())
                {
                    var slug = reader.GetString(reader.GetOrdinal("Slug")).Trim();
                    var translatedTitle = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title"));
                    var translatedShortDesc = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription"));
                    var translatedLongDesc = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription"));

                    foreach (var sc in subCategoriesList)
                    {
                        var page = sc.Pages.FirstOrDefault(p => p.Slug.Trim() == slug);
                        if (page != null)
                        {
                            page.Title = translatedTitle ?? page.Title;
                            page.ShortDescription = translatedShortDesc ?? page.ShortDescription;
                            page.LongDescription = translatedLongDesc ?? page.LongDescription;
                        }
                    }
                }
            }

            return subCategoriesList;
        }



        public async Task<List<CategoryWithPagesViewModel>> GetCategoryWithRelatedNewsAsync(int categoryId, int takePerRelated = 5)
        {
            // جلب الفئة الرئيسية مع RelatedCategories
            var categoryEntity = await _context.Categories
                                               .Include(c => c.RelatedCategories)
                                               .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (categoryEntity?.RelatedCategories == null || !categoryEntity.RelatedCategories.Any())
                return new List<CategoryWithPagesViewModel>();

            var relatedCategoriesList = new List<CategoryWithPagesViewModel>();

            foreach (var relatedCat in categoryEntity.RelatedCategories)
            {
                // جلب الصفحات لكل فئة مرتبطة باستخدام stored procedure
                var relatedCategoryVm = await GetCategoryWithPagesByIdAsync(relatedCat.Id, takePerRelated);

                if (relatedCategoryVm?.Pages?.Any() == true)
                {
                    // إنشاء نسخة لكل فئة مرتبطة مع اسمها وصفحاتها
                    var vm = new CategoryWithPagesViewModel
                    {
                        Id = relatedCategoryVm.Id,
                        Name = relatedCategoryVm.Name,
                        Slug = relatedCategoryVm.Slug,
                        Pages = relatedCategoryVm.Pages
                    };

                    relatedCategoriesList.Add(vm);
                }
            }

            return relatedCategoriesList;
        }


        public async Task<CategoryWithPagesViewModel?> GetCategoryWithPagesByIdAsync(int Id, int Take = int.MaxValue)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = Id },
                new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
            };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetCategoryWithPagesById";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // --- Result set 1: Main category ---
            if (!await reader.ReadAsync())
                return null;

            var category = new
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
                LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
                CoverImageUrl = reader.IsDBNull(reader.GetOrdinal("CoverImageUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverImageUrl")),
                CoverImageAltName = reader.IsDBNull(reader.GetOrdinal("CoverImageAltName")) ? null : reader.GetString(reader.GetOrdinal("CoverImageAltName")),
                FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeaturedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageUrl")),
                FeaturedImageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName")),
                TypeOfSorting = reader.IsDBNull(reader.GetOrdinal("TypeOfSorting")) ? "Manual" : reader.GetString(reader.GetOrdinal("TypeOfSorting")),
                ThemeName = reader.IsDBNull(reader.GetOrdinal("ThemeName")) ? null : reader.GetString(reader.GetOrdinal("ThemeName"))
            };

            // --- Result set 2: Category translations ---
            await reader.NextResultAsync();
            string? translatedName = null, translatedShortDesc = null, translatedLongDesc = null;

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32("LanguageId") == languageId)
                {
                    translatedName = reader.IsDBNull("Name") ? null : reader.GetString("Name");
                    translatedShortDesc = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription");
                    translatedLongDesc = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription");
                    break;
                }
            }

            // --- Result set 3: Related pages ---
            await reader.NextResultAsync();
            var pages = new List<PageViewModel>();
            var typeOfSorting = Enum.TryParse<TypeOfSorting>(category.TypeOfSorting, true, out var parsedSorting)
                ? parsedSorting
                : TypeOfSorting.Manual;

            while (await reader.ReadAsync())
            {
                pages.Add(new PageViewModel
                {
                    Slug = reader.GetString("Slug"),
                    Title = reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                    FeaturedImageAltName = reader.IsDBNull("FeaturedImageAltName") ? null : reader.GetString("FeaturedImageAltName"),
                    FeaturedImageUrl = reader.IsDBNull("FeatruedImageUrl") ? null : reader.GetString("FeatruedImageUrl"), // Note: Using actual column name from DB
                    ShortDescription = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                    LongDescription = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription"),
                    VideoUrl = reader.IsDBNull("VideoURL") ? null : reader.GetString("VideoURL"),
                    Order = reader.GetInt32("Order"),
                    CreatedAt = reader.GetDateTime("CreatedDate"),
                    DateInput = reader.IsDBNull("DateInput") ? null : reader.GetDateTime("DateInput"),
                    Count = reader.IsDBNull("Count") ? 0 : reader.GetInt32("Count"),
                    LinkUrl = reader.IsDBNull("LinkToUrl") ? null : reader.GetString("LinkToUrl")
                });
            }

            // --- Result set 4: Page translations ---
            await reader.NextResultAsync();
            var pageTranslations = new Dictionary<string, (string? Title, string? ShortDescription, string? LongDescription)>();

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32("LanguageId") == languageId)
                {
                    var slug = reader.GetString("Slug");
                    pageTranslations[slug] = (
                        reader.IsDBNull("Title") ? null : reader.GetString("Title"),
                        reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription"),
                        reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription")
                    );
                }
            }

            // Apply translations to pages
            foreach (var page in pages)
            {
                if (pageTranslations.TryGetValue(page.Slug, out var translation))
                {
                    page.Title = translation.Title ?? page.Title;
                    page.ShortDescription = translation.ShortDescription ?? page.ShortDescription;
                    page.LongDescription = translation.LongDescription ?? page.LongDescription;
                }
            }

            // Apply sorting
            var sortedPages = typeOfSorting switch
            {
                TypeOfSorting.Manual => pages.OrderBy(p => p.Order).ToList(),
                TypeOfSorting.NewToOld => pages.OrderByDescending(p => p.CreatedAt).ToList(),
                TypeOfSorting.OldToNew => pages.OrderBy(p => p.CreatedAt).ToList(),
                TypeOfSorting.Alphabetical => pages.OrderBy(p => p.Title).ToList(),
                TypeOfSorting.AlphabeticalReversed => pages.OrderByDescending(p => p.Title).ToList(),
                _ => pages.OrderBy(p => p.Order).ToList()
            };

            if (Take < int.MaxValue)
                sortedPages = sortedPages.OrderByDescending(p => p.CreatedAt).Take(Take).ToList();

            var viewModel = new CategoryWithPagesViewModel
            {
                Slug = category.Slug,
                Name = translatedName ?? category.Name,
                ShortDescription = translatedShortDesc ?? category.ShortDescription,
                LongDescription = translatedLongDesc ?? category.LongDescription,
                CoverImageUrl = category.CoverImageUrl,
                CoverimageAltName = category.CoverImageAltName,
                FeaturedImageUrl = category.FeaturedImageUrl,
                FeaturedimageAltName = category.FeaturedImageAltName,
                ThemeName = category.ThemeName,
                Pages = sortedPages
            };

            return viewModel;
        }
        public async Task<CategoryOnlyViewModel?> GetCategoryBySlugAsync(string CategorySlug)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
                new SqlParameter("@Slug", SqlDbType.NVarChar) { Value = CategorySlug },
                new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
            };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetCategoryBySlug";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // --- Result set 1: Main category ---
            if (!await reader.ReadAsync())
                return null;

            var category = new
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
                LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
                CoverImageUrl = reader.IsDBNull(reader.GetOrdinal("CoverImageUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverImageUrl")),
                CoverImageAltName = reader.IsDBNull(reader.GetOrdinal("CoverImageAltName")) ? null : reader.GetString(reader.GetOrdinal("CoverImageAltName")),
                FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeaturedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageUrl")),
                FeaturedImageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName"))
            };

            // --- Result set 2: Category translations ---
            await reader.NextResultAsync();
            string? translatedName = null, translatedShortDesc = null, translatedLongDesc = null;

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32("LanguageId") == languageId)
                {
                    translatedName = reader.IsDBNull("Name") ? null : reader.GetString("Name");
                    translatedShortDesc = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription");
                    translatedLongDesc = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription");
                    break;
                }
            }

            var viewModel = new CategoryOnlyViewModel
            {
                Slug = category.Slug,
                Name = translatedName ?? category.Name,
                ShortDescription = translatedShortDesc ?? category.ShortDescription,
                LongDescription = translatedLongDesc ?? category.LongDescription,
                CoverImageUrl = category.CoverImageUrl,
                CoverimageAltName = category.CoverImageAltName,
                FeaturedImageUrl = category.FeaturedImageUrl,
                FeaturedimageAltName = category.FeaturedImageAltName,
            };

            return viewModel;
        }
        public async Task<CategoryOnlyViewModel?> GetCategoryByIdAsync(int Id)
        {
            var languageId = await LanguageHelper.GetCurrentLanguageIdAsync(_context);

            var parameters = new[]
            {
                new SqlParameter("@CategoryId", SqlDbType.Int) { Value = Id },
                new SqlParameter("@LanguageId", SqlDbType.Int) { Value = languageId }
            };

            await _context.Database.OpenConnectionAsync();

            using var command = (SqlCommand)_context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "GetCategoryById";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddRange(parameters);

            using var reader = await command.ExecuteReaderAsync();

            // --- Result set 1: Main category ---
            if (!await reader.ReadAsync())
                return null;

            var category = new
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Slug = reader.GetString(reader.GetOrdinal("Slug")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                ShortDescription = reader.IsDBNull(reader.GetOrdinal("ShortDescription")) ? null : reader.GetString(reader.GetOrdinal("ShortDescription")),
                LongDescription = reader.IsDBNull(reader.GetOrdinal("LongDescription")) ? null : reader.GetString(reader.GetOrdinal("LongDescription")),
                CoverImageUrl = reader.IsDBNull(reader.GetOrdinal("CoverImageUrl")) ? null : reader.GetString(reader.GetOrdinal("CoverImageUrl")),
                CoverImageAltName = reader.IsDBNull(reader.GetOrdinal("CoverImageAltName")) ? null : reader.GetString(reader.GetOrdinal("CoverImageAltName")),
                FeaturedImageUrl = reader.IsDBNull(reader.GetOrdinal("FeaturedImageUrl")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageUrl")),
                FeaturedImageAltName = reader.IsDBNull(reader.GetOrdinal("FeaturedImageAltName")) ? null : reader.GetString(reader.GetOrdinal("FeaturedImageAltName"))
            };

            // --- Result set 2: Category translations ---
            await reader.NextResultAsync();
            string? translatedName = null, translatedShortDesc = null, translatedLongDesc = null;

            while (await reader.ReadAsync())
            {
                if (reader.GetInt32("LanguageId") == languageId)
                {
                    translatedName = reader.IsDBNull("Name") ? null : reader.GetString("Name");
                    translatedShortDesc = reader.IsDBNull("ShortDescription") ? null : reader.GetString("ShortDescription");
                    translatedLongDesc = reader.IsDBNull("LongDescription") ? null : reader.GetString("LongDescription");
                    break;
                }
            }

            var viewModel = new CategoryOnlyViewModel
            {
                Slug = category.Slug,
                Name = translatedName ?? category.Name,
                ShortDescription = translatedShortDesc ?? category.ShortDescription,
                LongDescription = translatedLongDesc ?? category.LongDescription,
                CoverImageUrl = category.CoverImageUrl,
                CoverimageAltName = category.CoverImageAltName,
                FeaturedImageUrl = category.FeaturedImageUrl,
                FeaturedimageAltName = category.FeaturedImageAltName,
            };

            return viewModel;
        }
    }
}


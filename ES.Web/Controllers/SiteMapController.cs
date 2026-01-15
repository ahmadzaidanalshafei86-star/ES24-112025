using ES.Web.Models;
using ES.Web.Services;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ES.Web.Controllers
{
    public class SiteMapController : Controller
    {
        private readonly MenuItemsService _menuService;
        private readonly IStringLocalizer<SiteMapController> _localizer;

        public SiteMapController(MenuItemsService menuService, IStringLocalizer<SiteMapController> localizer)
        {
            _menuService = menuService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Get menu items from service
            var menuItems = await _menuService.GetMenuItemsAsync();

            // 2. Map to view models recursively
            var model = new MenuViewModel
            {
                MenuItems = menuItems.Select(MapMenuItem).ToList()
            };

            return View(model);
        }

        private MenuItemViewModel MapMenuItem(MenuItem menuItem)
        {
            var translation = menuItem.Translations.FirstOrDefault();

            return new MenuItemViewModel
            {
                Id = menuItem.Id,
                Title = translation?.Title ?? menuItem.Title,
                Url = menuItem.Url,
                Icon = menuItem.Icon,
                Type = menuItem.Type,
                Target = menuItem.Target,
                Children = menuItem.Children?.Select(MapMenuItem).ToList() ?? new List<MenuItemViewModel>()
            };
        }
    }
}

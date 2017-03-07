using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.Site;
using ModCore.Specifications.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Site
{
    public class MenuManager : IMenuManager
    {
        private IDataRepositoryAsync<Menu> _repository;
        private List<Menu> _menus;

        public List<Menu> Menus
        {
            get
            {
                if (_menus == null)
                {
                    _menus = _repository.FindAllAsync(new AllMenus()).Result.ToList();
                }

                return _menus;
            }
        }

        public MenuManager(IDataRepositoryAsync<Menu> repository)
        {
            _repository = repository;
        }

        public Menu GetMenuByName(string menuName)
        {
            return this.Menus.SingleOrDefault(a => a.Name.ToLower() == menuName.ToLower());
        }

        public async Task AddParentToMenuAsync(MenuItem menuItem, string menuName)
        {
            var menu = GetMenuByName(menuName);

            menuItem.Id = Guid.NewGuid().ToString();

            menu.MenuItems.Add(menuItem);
            menu.MenuItems.OrderBy(a => a.Order);

            await _repository.UpdateAsync(menu);

            Refresh();
        }

        public async Task AddToSubMenuAsync(MenuItem menuItem, string menuName, string subMenuId)
        {
            var menu = GetMenuByName(menuName);

            menuItem.Id = Guid.NewGuid().ToString();

            menu.MenuItems.Add(menuItem);
            menu.MenuItems.OrderBy(a => a.Order);

            await _repository.UpdateAsync(menu);

            Refresh();
        }

        public MenuItem GetMenuItem(string menuName, string subMenuId)
        {
            var menu = GetMenuByName(menuName);

            return GetMenuItem(menu.MenuItems, subMenuId);
        }

        private MenuItem GetMenuItem(List<MenuItem> menuItems, string subMenuId)
        {
            foreach (var menuItem in menuItems)
            {
                if (menuItem.Id == subMenuId)
                    return menuItem;

                if (!menuItem.isParent)
                    return GetMenuItem(menuItem.Children, subMenuId);
            }

            return null;
        }

        private void Refresh()
        {
            _menus = null;
        }

        
    }
    public static class BuiltInMenus
    {
        public static string MainMenu => "MAIN_MENU";

        public static string AdminMenu => "ADMIN_MENU";

    }
}

using ModCore.Models.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface IMenuManager
    {
        Menu GetMenuByName(string menuName);

        Task AddParentToMenuAsync(MenuItem menuItem, string menuName);

        Task AddToSubMenuAsync(MenuItem menuItem, string menuName, string subMenuId);

    }
}

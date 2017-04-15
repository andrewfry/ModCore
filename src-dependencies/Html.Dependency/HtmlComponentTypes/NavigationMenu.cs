using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModHtml.Dependency.HtmlComponentTypes
{
    public class NavigationMenu : BaseHtmlComponent
    {
        public NavigationMenu() {
            NavigationMenuItems = new List<NavigationMenuItem>();
        }

        public IList<NavigationMenuItem> NavigationMenuItems { get; set; }

    }

    public class NavigationMenuItem
    {
        public string DisplayName { get; set; }

        //public IList<NavigationMenuItem> ChildMenuItems { get; set; }

        public int Position { get; set; }

        public string Href { get; set; }
        
    }

}

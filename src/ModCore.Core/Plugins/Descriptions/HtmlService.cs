using ModCore.Abstraction.Plugins.Builtins;
using ModCore.Abstraction.Services.Site;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.Plugins.Descriptions
{
    public class HtmlService : BasePluginDescription, IPluginDescription
    {
        public override string Name { get { return "HTMLService"; } }

        public override List<Type> RequiredInterfaces
        {
            get
            {
                return new List<Type>()
                {
                    typeof(IHtmlService)
                };
            }
        }
    }
}

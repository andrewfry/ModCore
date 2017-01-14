using ModCore.Abstraction.Plugins.Builtins;
using ModCore.Abstraction.Services.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins.Descriptions
{
    public class EmailService : BasePluginDescription, IPluginDescription
    {
        public override string Name { get { return "EmailService"; } }

        public override List<Type> RequiredInterfaces
        {
            get
            {
                return new List<Type>()
                {
                    typeof(IEmailService),
                };
            }
        }
    }
}

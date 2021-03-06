﻿using ModCore.Abstraction.Plugins.Builtins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Plugins.Descriptions
{
    public class AuthenticationService : BasePluginDescription, IPluginDescription
    {
        public override string Name { get { return "AuthenticationService"; } }

        public override List<Type> RequiredInterfaces
        {
            get
            {
                return new List<Type>()
                {
                    typeof(IAuthenticationService),
                };
            }
        }

    }
}

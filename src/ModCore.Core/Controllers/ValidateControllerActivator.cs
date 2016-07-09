using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Core.Controllers
{
    public class ValidateControllerActivator : DefaultControllerActivator, IControllerActivator
    {
        public ValidateControllerActivator(ITypeActivatorCache typeActivatorCache)
           : base(typeActivatorCache)
        {

        }

        public override object Create(ControllerContext actionContext)
        {

            if (!actionContext.ActionDescriptor.ControllerTypeInfo.IsSubclassOf(typeof(BaseController)))
            {
                throw new Exception(string.Format("The controller {0} must inherit from BaseController", actionContext.ActionDescriptor.ControllerName));
            }

            return base.Create(actionContext);
        }

        public override void Release(ControllerContext context, object controller)
        {
            base.Release(context, controller);
        }
    }
}

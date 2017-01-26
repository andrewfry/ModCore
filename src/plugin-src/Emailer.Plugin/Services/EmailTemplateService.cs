using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Services.Access;
using ModCore.Abstraction.Site;
using ModCore.Core.HelperExtensions;
using ModCore.Models.Access;
using ModCore.Models.Communication;
using ModCore.Models.Enum;
using ModCore.Services.Base;
using ModCore.Utilities.HelperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Emailer.Plugin.Services
{
    public class EmailTemplateService : BaseServiceAsync<EmailTemplate>
    {


   

        public EmailTemplateService(IDataRepositoryAsync<EmailTemplate> repos, IMapper mapper, ILog logger) :
            base(repos, mapper, logger)
        {

        }

      
    }
}

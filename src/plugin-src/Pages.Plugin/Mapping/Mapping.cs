using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pages.Plugin.Models;
using Pages.Plugin.ViewModels;
using MongoDB.Bson.Serialization;
using ModHtml.Dependency.HtmlComponentTypes;

namespace Pages.Plugin.Mapping
{
    public class PageMapping : Profile
    {
        public PageMapping()
        {
            CreateMap<PageViewModel, Page>().ReverseMap();

            if (!BsonClassMap.IsClassMapRegistered(typeof(NavigationMenu)))
            {
                BsonClassMap.RegisterClassMap<NavigationMenu>();
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(TextArea)))
            {
                BsonClassMap.RegisterClassMap<TextArea>();
            }
        }
    }
}

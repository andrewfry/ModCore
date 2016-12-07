using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pages.Plugin.Models;
using Pages.Plugin.ViewModels;

namespace Pages.Plugin.Mapping
{
    public class PageMapping : Profile
    {
        public PageMapping()
        {
            CreateMap<PageViewModel, Page>().ReverseMap();
        }
    }
}

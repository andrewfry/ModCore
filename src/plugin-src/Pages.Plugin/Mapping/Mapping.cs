using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Pages.Plugin.Models;

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModCore.Core.Controllers;
using ModCore.Abstraction.Plugins;
using ModCore.Abstraction.Site;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModCore.ViewModels.Base;
using Pages.Plugin.ViewModels;
using Pages.Plugin.Services;
using Pages.Plugin.Models;
using ModCore.Abstraction.Services.Access;

namespace Pages.Plugin.Areas.Admin
{
    [Area("Admin")]
    public class PageController : BasePluginController
    {
        public override IPlugin Plugin
        {
            get
            {
                return new Pages();
            }
        }
        private IPageService _pageService;
        public PageController(IPageService pageService, IPluginLog log, ISiteSettingsManagerAsync siteSettingsManager, IPluginSettingsManager pluginSettingsManager,
            IBaseViewModelProvider baseViewModelProvider, IMapper mapper, ISessionService sessionService) :
            base(log, siteSettingsManager, pluginSettingsManager, baseViewModelProvider, mapper, sessionService)
        {
            _pageService = pageService;
        }

        public async Task<IActionResult> Index()
        {
            var m = new PageListViewModel();

            m.PageList = await _pageService.PageList();
            return View(m);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var m = new PageViewModel();
            m.PageStatus = PageStatus.Draft;
            //m.AvailableTemplates = _
            m.AvailableHtmlComponents = _pageService.AvailableHTMLComponents();
            
            return View(m);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PageViewModel model)
        {
            var message = "Please correct errors on page.";
            if (ModelState.IsValid)
            {
                //var navMenu = new ModHtml.Dependency.HtmlComponentTypes.NavigationMenu();
                //navMenu.DisplayTypeName = "Navigation Menu";
                //navMenu.Name = "navMenuTest";
                //navMenu.Position = 1;
                //navMenu.BaseClass = "nav-menu";
                //navMenu.NavigationMenuItems.Add(new ModHtml.Dependency.HtmlComponentTypes.NavigationMenuItem()
                //{
                //    DisplayName = "Test Link 1",
                //    Href = "/TestLink",
                //    Position = 1
                //});

                //model.HtmlComponents.Add(navMenu);
                await _pageService.CreatePage(model);
                message = "Page successfully saved";
            }
            model.AvailableHtmlComponents = _pageService.AvailableHTMLComponents();
            ViewBag.Message = message;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var model = new PageViewModel();
            var page = await _pageService.GetByIdAsync(Id);
            model = _mapper.Map<PageViewModel>(page);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PageViewModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                await _pageService.Update(model);
                message = "Page successfully saved";
                ViewBag.Message = message;
                return RedirectToAction("Index", "Page", new { area ="Admin"});
            }
            ViewBag.Message = "Please correct errors on the page";           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var message = "";
            try
            {
                await _pageService.Delete(Id);
                message = "Page deleted";
                ViewBag.Message = message;
                return RedirectToAction("Index", "Page");
            }
            catch
            {
                ViewBag.Message = "Please correct errors on the page";
            }
            return RedirectToAction("Index", "Page");
        }
    }
}

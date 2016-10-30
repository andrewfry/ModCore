using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Plugins;
using ModCore.Core.Plugins;
using ModCore.Models.Themes;
using ModCore.Specifications.Plugins;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ModCore.Abstraction.Themes;
using ModCore.Specifications.Themes;

namespace ModCore.Core.Themes
{
    public class ThemeManager : IThemeManager
    {
        private string _themesLocation;
        private IList<ITheme> _availableThemes;
        private ITheme _activeTheme;
        private IThemeManager _themeManager;
        private IConfigurationRoot _configurationRoot;
        IHostingEnvironment _hostingEnvironment;
        IDataRepository<ActiveTheme> _repository;

        public IList<ITheme> AvailableThemes {
            get {
                if (_availableThemes == null || _availableThemes.Count == 0)
                {
                    _availableThemes = GetAvailableThemes(_themesLocation);
                }

                return _availableThemes;
            }
           
        }

        public ITheme ActiveTheme {
            get {
                if (_activeTheme == null)
                {
                    //load active theme from db see if that exists in available themes
                    var activeTheme = _repository.Find(new ActiveSiteTheme());
                    if (AvailableThemes.Any(a => a.ThemeName == activeTheme.ThemeName))
                    {
                        _activeTheme = AvailableThemes.Single(a => a.ThemeName == activeTheme.ThemeName);
                    }
                }

                return _activeTheme;
            }
        }

        private IList<ITheme> GetAvailableThemes(string directoryPath)
        {
            var themeList = new List<ITheme>();

            if (string.IsNullOrEmpty(directoryPath))
                throw new ArgumentNullException("path");

            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException(directoryPath);

            foreach (var folder in Directory.EnumerateDirectories(directoryPath))
            {
                ITheme returnTheme = null;

                var themePath = Path.GetFullPath(Path.Combine(folder, "theme.json"));
                // read JSON and load theme info
                returnTheme = JsonConvert.DeserializeObject<Theme>(File.ReadAllText(themePath));

                if (returnTheme != null)
                {
                    themeList.Add(returnTheme);
                }
            }

            return themeList;
        }

        public ThemeManager(IConfigurationRoot configurationRoot, 
            IHostingEnvironment hostingEnvironment, IDataRepository<ActiveTheme> repository)
        {
            _configurationRoot = configurationRoot;
            _hostingEnvironment = hostingEnvironment;

            _repository = repository;            
            var themesDir = _configurationRoot.GetValue<string>("ThemesDir");
            _themesLocation = Path.Combine(_hostingEnvironment.ContentRootPath, themesDir);

            if (string.IsNullOrEmpty(_themesLocation))
            {
                throw new ArgumentNullException("ThemesDir");
            }
        }

        public void ActivateTheme(ITheme theme)
        {
            var activeTheme = _repository.Find(new ActiveSiteTheme());
            if (activeTheme != null) {
                activeTheme.Description = theme.Description;
                activeTheme.DisplayName = theme.DisplayName;
                activeTheme.ThemeName = theme.ThemeName;
                activeTheme.ThemeVersion = theme.ThemeVersion;

                _repository.Update(activeTheme);          
            }

            //clear page cache??
        }

      
    }
}

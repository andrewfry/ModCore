﻿using ModCore.Models.Plugins;
using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.ViewModels.Admin.Plugin
{
    public class vPluginList : BaseViewModel
    {
        public List<vPlugin> Plugins { get; set; }

        public List<PluginError> PluginErrors { get; set; }

        public List<PluginError> PluginWarnings { get; set; }
    }
}

using System.Collections.Generic;

namespace ModCore.Models.Plugins
{
    public class PluginInstallResult
    {
        public bool WasSuccessful { get; set; }

        public List<string> Errors { get; set; }
    }
}

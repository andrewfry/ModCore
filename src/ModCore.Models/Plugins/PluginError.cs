using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.Plugins
{
    public class PluginError
    {
        public PluginErrorLevel ErrorLevel { get; set; }

        public PluginErrorType ErrorType { get; set; }

        public string PluginName { get; set; }

        public string ErrorMessage { get; set; }

        public PluginError(PluginErrorLevel errorLevel, PluginErrorType errorType, string pluginName, string errorMessage)
        {
            this.ErrorLevel = errorLevel;
            this.ErrorType = errorType;
            this.PluginName = pluginName;
            this.ErrorMessage = errorMessage;
        }

    }


    public enum PluginErrorType
    {
        AssemblyLoad,
        PluginLoad,
    }

    public enum PluginErrorLevel
    {
        Error,
        Warning,
        Debug,
    }

}

using ModCore.Abstraction.DataAccess;
using ModCore.Abstraction.Site;
using ModCore.Models.Core;
using ModCore.Models.Enum;
using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ModCore.Core.Site
{
    public class SiteLogger : ILog
    {
        IDataRepository<Log> _dbRepos;
        protected string _pluginName;
        
        public SiteLogger(IDataRepository<Log> logDb)
        {
            _dbRepos = logDb;
            _pluginName = string.Empty;
        }

        public void LogError<T>(Exception exception, string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogError(className, exception, message, messageVariables);
        }

        public void LogError(string className, Exception exception, string message, params string[] messageVariables)
        {
            LogError(className, null, exception, message, messageVariables);
        }

        public void LogError(string className, SessionData sessionData, Exception exception, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(ErrorLevel.Warning, className, message, exception.Message, exception.InnerException.ToString(), exception.StackTrace, sessionData);
        }

        public void LogWarning<T>(Exception exception, string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogWarning(className, exception, message, messageVariables);
        }

        public void LogWarning(string className, Exception exception, string message, params string[] messageVariables)
        {
            LogWarning(className, null, exception, message, messageVariables);
        }

        public void LogWarning(string className, SessionData sessionData, Exception exception, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(ErrorLevel.Warning, className, message, exception.Message, exception.InnerException.ToString(), exception.StackTrace, sessionData);
        }

        public void LogDebug<T>(string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogDebug(className, message, messageVariables);
        }

        public void LogDebug(string className, string message, params string[] messageVariables)
        {
            LogDebug(className, null, message, messageVariables);
        }

        public void LogDebug(string className, SessionData sessionData, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(ErrorLevel.Debug, className, message, "", "", "", sessionData);
        }

        public void LogInfo<T>(string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogInfo(className, message, messageVariables);
        }

        public void LogInfo(string className, string message, params string[] messageVariables)
        {
            LogInfo(className, null, message, messageVariables);
        }

        public void LogInfo(string className, SessionData sessionData, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(ErrorLevel.Information, className, message, "", "", "", sessionData);
        }

        protected virtual void ExecuteLogging(ErrorLevel errorLevel, string className, string message,
            string errorMessage, string innerException, string stackTrace, SessionData sessionData)
        {
            var log = new Log
            {
                ClassName = className,
                PluginName = _pluginName,
                ErrorLevel = errorLevel,
                ErrorMessage = errorMessage,
                InnerException = innerException,
                Message = message,
                Session = sessionData,
                StackTrace = stackTrace
            };


            _dbRepos.Insert(log);
        }

    }
}

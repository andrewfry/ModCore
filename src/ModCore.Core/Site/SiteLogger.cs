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
using Microsoft.Extensions.Logging;

namespace ModCore.Core.Site
{
    public class SiteLogger : ILog, ILogger
    {
        IDataRepositoryAsync<Log> _dbRepos;
        protected string _pluginName;
        ISiteSettingsManagerAsync _siteSettingsManager;

        public SiteLogger(IDataRepositoryAsync<Log> logDb, ISiteSettingsManagerAsync siteSettingsManager)
        {
            _dbRepos = logDb;
            _pluginName = string.Empty;
            _siteSettingsManager = siteSettingsManager;
        }

        public void LogError<T>(Exception exception, string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogError(className, exception, message, messageVariables);
        }

        public void LogError(string className, Exception exception, string message, params string[] messageVariables)
        {
            LogError(className, null, exception, "", message, messageVariables);
        }

        public void LogError(string className, SessionData sessionData, Exception exception, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(LogLevel.Error, className, message, exception?.Message, exception?.InnerException?.ToString(), exception?.StackTrace, sessionData, route);
        }

        public void LogWarning<T>(Exception exception, string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogWarning(className, exception, message, messageVariables);
        }

        public void LogWarning(string className, Exception exception, string message, params string[] messageVariables)
        {
            LogWarning(className, null, exception, "", message, messageVariables);
        }

        public void LogWarning(string className, SessionData sessionData, Exception exception, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(LogLevel.Warning, className, message, exception?.Message, exception?.InnerException?.ToString(), exception?.StackTrace, sessionData, route);
        }

        public void LogDebug<T>(string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogDebug(className, message, messageVariables);
        }

        public void LogDebug(string className, string message, params string[] messageVariables)
        {
            LogDebug(className, null, "", message, messageVariables);
        }

        public void LogDebug(string className, SessionData sessionData, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(LogLevel.Debug, className, message, "", "", "", sessionData, route);
        }

        public void LogInfo<T>(string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            LogInfo(className, message, messageVariables);
        }

        public void LogInfo(string className, string message, params string[] messageVariables)
        {
            LogInfo(className, null, "", message, messageVariables);
        }

        public void LogInfo(string className, SessionData sessionData, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            ExecuteLogging(LogLevel.Information, className, message, "", "", "", sessionData, route);
        }

        protected virtual void ExecuteLogging(LogLevel errorLevel, string className, string message,
            string errorMessage, string innerException, string stackTrace, SessionData sessionData, string route)
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
                StackTrace = stackTrace,
                Route = route,
                InsertDate = DateTime.UtcNow
            };


            _dbRepos.InsertAsync(log);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            var settingLogLevel = _siteSettingsManager.GetSettingAsync<LogLevel>(BuiltInSettings.LogLevel).Result;
            if ((int)logLevel <= (int)settingLogLevel)
            {
                return true;
            }

            return false;
        }

        public void Log<T>(LogLevel logLevel, EventId eventId, T state, Exception exception, Func<T, Exception, string> formatter)
        {
            ExecuteLogging(logLevel, typeof(T).FullName, formatter(state, exception), exception?.Message?.ToString(), exception?.InnerException?.ToString(), exception?.StackTrace?.ToString(), null, "");
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}

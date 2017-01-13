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

        public async Task LogError<T>(Exception exception, string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            await LogError(className, exception, message, messageVariables);
        }

        public async Task LogError(string className, Exception exception, string message, params string[] messageVariables)
        {
            await LogError(className, null, exception, "", message, messageVariables);
        }

        public async Task LogError(string className, SessionData sessionData, Exception exception, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            await ExecuteLogging(LogLevel.Error, className, message, exception?.Message, exception?.InnerException?.ToString(), exception?.StackTrace, sessionData, route);
        }

        public async Task LogWarning<T>(Exception exception, string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            await LogWarning(className, exception, message, messageVariables);
        }

        public async Task LogWarning(string className, Exception exception, string message, params string[] messageVariables)
        {
            await LogWarning(className, null, exception, "", message, messageVariables);
        }

        public async Task LogWarning(string className, SessionData sessionData, Exception exception, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            await ExecuteLogging(LogLevel.Warning, className, message, exception?.Message, exception?.InnerException?.ToString(), exception?.StackTrace, sessionData, route);
        }

        public async Task LogDebug<T>(string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            await LogDebug(className, message, messageVariables);
        }

        public async Task LogDebug(string className, string message, params string[] messageVariables)
        {
            await LogDebug(className, null, "", message, messageVariables);
        }

        public async Task LogDebug(string className, SessionData sessionData, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            await ExecuteLogging(LogLevel.Debug, className, message, "", "", "", sessionData, route);
        }

        public async Task LogInfo<T>(string message, params string[] messageVariables)
        {
            var className = typeof(T).FullName;
            await LogInfo(className, message, messageVariables);
        }

        public async Task LogInfo(string className, string message, params string[] messageVariables)
        {
            await LogInfo(className, null, "", message, messageVariables);
        }

        public async Task LogInfo(string className, SessionData sessionData, string route, string message, params string[] messageVariables)
        {
            message = string.Format(message, messageVariables);

            await ExecuteLogging(LogLevel.Information, className, message, "", "", "", sessionData, route);
        }

        protected virtual async Task ExecuteLogging(LogLevel errorLevel, string className, string message,
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
                InsertDate = DateTime.UtcNow,
            };


           await _dbRepos.InsertAsync(log);
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

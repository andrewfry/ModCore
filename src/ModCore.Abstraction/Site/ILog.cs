using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface ILog
    {
        void LogError<T>(Exception exception, string message, params string[] messageVariables);

        void LogError(string className, Exception exception, string message, params string[] messageVariables);

        void LogError(string className, SessionData sessionData, Exception exception, string route, string message, params string[] messageVariables);

        void LogWarning<T>(Exception exception, string message, params string[] messageVariables);

        void LogWarning(string className, Exception exception, string message, params string[] messageVariables);

        void LogWarning(string className, SessionData sessionData,  Exception exception, string route, string message, params string[] messageVariables);

        void LogDebug<T>(string message, params string[] messageVariables);

        void LogDebug(string className, string message, params string[] messageVariables);

        void LogDebug(string className, SessionData sessionData, string route, string message, params string[] messageVariables);

        void LogInfo<T>(string message, params string[] messageVariables);

        void LogInfo(string className, string message, params string[] messageVariables);

        void LogInfo(string className, SessionData sessionData, string route, string message, params string[] messageVariables);

    }
}

using ModCore.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface ILog
    {
        Task LogError<T>(Exception exception, string message, params string[] messageVariables);

        Task LogError(string className, Exception exception, string message, params string[] messageVariables);

        Task LogError(string className, SessionData sessionData, Exception exception, string route, string message, params string[] messageVariables);

        Task LogWarning<T>(Exception exception, string message, params string[] messageVariables);

        Task LogWarning(string className, Exception exception, string message, params string[] messageVariables);

        Task LogWarning(string className, SessionData sessionData,  Exception exception, string route, string message, params string[] messageVariables);

        Task LogDebug<T>(string message, params string[] messageVariables);

        Task LogDebug(string className, string message, params string[] messageVariables);

        Task LogDebug(string className, SessionData sessionData, string route, string message, params string[] messageVariables);

        Task LogInfo<T>(string message, params string[] messageVariables);

        Task LogInfo(string className, string message, params string[] messageVariables);

        Task LogInfo(string className, SessionData sessionData, string route, string message, params string[] messageVariables);

    }
}

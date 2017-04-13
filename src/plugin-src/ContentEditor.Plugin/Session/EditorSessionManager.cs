using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ModCore.Abstraction.Plugins;
using ContentEditor.Plugin.Session;

namespace Content.Plugin.Session
{
    public class EditorSessionManager
    {
        private static BlockingCollection<EditorSession> Sessions = new BlockingCollection<EditorSession>();

        public EditorSessionManager()
        {

        }

        public EditorSession GetSession(string sessionId)
        {
            return Sessions.SingleOrDefault(a => a.SessionId == sessionId);
        }

    }

   
}

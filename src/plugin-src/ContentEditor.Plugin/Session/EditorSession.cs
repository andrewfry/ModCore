using ModCore.Abstraction.Plugins;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentEditor.Plugin.Session
{
    public class EditorSession
    {
        public IPlugin RequestingPlugin { get; set; }

        public string SessionId { get; set; }

        public Dictionary<string, object> Data { get; set; }

        public EditorSession()
        {
            Data = new Dictionary<string, object>();
        }

    }
}

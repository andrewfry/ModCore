using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ModCore.Core.Site
{
    public class ApplicationManager
    {
        private static ApplicationManager _appManager;
        private IWebHost _web;
        private CancellationTokenSource _tokenSource;
        private bool _running;
        private bool _restart;
        private Type _startUpType;

        public bool Restarting => _restart;

        internal ApplicationManager()
        {
            _running = false;
            _restart = false;

        }

        public static ApplicationManager Load()
        {
            if (_appManager == null)
                _appManager = new ApplicationManager();

            return _appManager;
        }

        public void SetStartUpClass(Type startUpType)
        {
            _startUpType = startUpType;
        }

        public static void Launch(Type startUpType)
        {
            var appManager = ApplicationManager.Load();
            appManager.SetStartUpClass(startUpType);

            do
            {
                appManager.Start();
            }

            while (appManager.Restarting);
        }

        public void Start()
        {
            if (_running)
                return;

            if (_tokenSource != null && _tokenSource.IsCancellationRequested)
                return;

            _tokenSource = new CancellationTokenSource();
            _tokenSource.Token.ThrowIfCancellationRequested();
            _running = true;

            _web = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            //.UseIISIntegration() //TODO - should this even be supported?
            .UseStartup(_startUpType)
            .Build();

            _web.Run(_tokenSource.Token);
        }

        public void Stop()
        {
            if (!_running)
                return;

            _tokenSource.Cancel();
            _running = false;
        }

        public void Restart()
        {
            Stop();

            _restart = true;
            _tokenSource = null;
        }

    }


}

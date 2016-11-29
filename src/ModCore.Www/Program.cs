using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ModCore.Core.Site;

namespace ModCore.Www
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ApplicationManager.Launch(typeof(Startup));
        }
    }
}

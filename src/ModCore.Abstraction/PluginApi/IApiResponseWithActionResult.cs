using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Abstraction.PluginApi
{
   public  interface IApiResponseWithActionResult : IApiResponse
    {
         IActionResult ActionResult { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.PluginApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace ModCore.Core.PluginApi
{
   public  class ApiResponseWithActionResult : ApiResponse, IApiResponseWithActionResult
    {
        public IActionResult ActionResult { get; set; }
    }
}

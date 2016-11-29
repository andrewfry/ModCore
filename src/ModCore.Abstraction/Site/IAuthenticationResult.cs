using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Abstraction.Site
{
    public interface IAuthenticationResult
    {
        bool HasResult { get; }

        bool Successful { get; }
        
        string ErrorMessage { get; }

        IActionResult ActionResult { get; }
    }
}

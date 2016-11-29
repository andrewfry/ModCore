using Microsoft.AspNetCore.Mvc;
using ModCore.Abstraction.Site;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicAuthentication.Plugin.Models
{
    public class BasicAuthenticationResult : IAuthenticationResult
    {
        private bool _successful;
        private string _errorMessage;
        private IActionResult _actionResult;

        public IActionResult ActionResult { get { return _actionResult; } }

        public bool HasResult => ActionResult != null;

        public bool Successful { get { return _successful; } }

        public string ErrorMessage { get { return _errorMessage; } }

        public BasicAuthenticationResult(bool successful)
        {
            _successful = successful;
            _errorMessage = "";
        }

        public BasicAuthenticationResult(bool successful, string errorMessage)
        {
            _successful = successful;
            _errorMessage = errorMessage;
        }

        public void SetResult(IActionResult actionResult)
        {
            _actionResult = actionResult;
        }

    }
}

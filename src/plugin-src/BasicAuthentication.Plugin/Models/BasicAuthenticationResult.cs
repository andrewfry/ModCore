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

    }
}

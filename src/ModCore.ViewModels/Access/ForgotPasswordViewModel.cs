using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ModCore.ViewModels.Base;

namespace ModCore.ViewModels.Access
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

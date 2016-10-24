using ModCore.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModeCore.ViewModels.Access
{
    public class ExternalLoginConfirmationViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}

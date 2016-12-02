using ModCore.ViewModels.Base;
using System.ComponentModel.DataAnnotations;

namespace ModCore.ViewModels.Page
{
    public class PageViewModel : BaseViewModel 
    {
        [Required]
        [Display(Name = "Page Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Friendly Url")]
        public string Url { get; set; }

        [Required]
        [Display(Name = "Content")]
        public string HTMLContent { get; set; }

        
        public bool Active { get; set; }
        
    }
}

using System.ComponentModel.DataAnnotations;

namespace Web_Server.ViewModels
{
    public class ApplyWithNewCVVm
    {
        [Required]
        public int PostId { get; set; }
        
        [Required]
        public IFormFile CVFile { get; set; }
        
        [Required]
        public string Text { get; set; }
    }

    public class ApplyWithExistingCVVm
    {
        [Required]
        public int PostId { get; set; }
        
        [Required]
        public string Text { get; set; }
    }
} 
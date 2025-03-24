using System.ComponentModel.DataAnnotations;

namespace Web_Server.ViewModels
{
    public class UpdateEmailVm
    {
        [Required]
        [EmailAddress]
        public string newEmail { get; set; }
    }
}

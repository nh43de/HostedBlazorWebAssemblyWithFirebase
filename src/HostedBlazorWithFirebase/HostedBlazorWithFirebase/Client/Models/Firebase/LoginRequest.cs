using System.ComponentModel.DataAnnotations;

namespace HostedBlazorWithFirebase.Client.Models
{
    public class LoginRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set;  }
    }
}

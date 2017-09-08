using System.ComponentModel.DataAnnotations;
namespace Enceladus.Api.Models
{
    public class LoginViewModel
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}

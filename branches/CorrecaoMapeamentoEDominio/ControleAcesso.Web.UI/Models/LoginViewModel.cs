using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ControleAcesso.Web.UI.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}
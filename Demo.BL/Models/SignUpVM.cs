using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Demo.BL.Models
{
    public class SignUpVM
    {
        [Required(ErrorMessage ="ThisField Required")]
        [EmailAddress(ErrorMessage ="Invalid Mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "ThisField Required")]
        [MinLength(6,ErrorMessage ="Min Length 6")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ThisField Required")]
        [MinLength(6, ErrorMessage = "Min Length 6")]
        [Compare("Password", ErrorMessage = "Password Not match")]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }
    }
       
    

}

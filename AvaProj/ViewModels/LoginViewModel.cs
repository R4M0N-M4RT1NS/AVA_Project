using System.ComponentModel.DataAnnotations;

namespace AvaProj.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Matrícula ou Email")]
        public string Identificador { get; set; } // pode ser matrícula ou email

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Lembrar-me?")]
        public bool LembrarMe { get; set; }
    }
}

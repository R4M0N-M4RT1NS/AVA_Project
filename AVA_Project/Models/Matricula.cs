using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AvaProj.Models
{
    public class Matricula
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [ValidateNever]
        public Usuario Usuario { get; set; }
        public int DisciplinaOfertadaId { get; set; }  // FK
        [ValidateNever]
        public DisciplinaOfertada DisciplinaOfertada { get; set; }
    }
}

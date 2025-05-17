using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AvaProj.Models
{
    public class DisciplinaOfertada
    {
        public int Id { get; set; }
        public int DisciplinaId { get; set; }
        [ValidateNever]
        public Disciplina Disciplina { get; set; }
        public int ProfessorId { get; set; }
        [ValidateNever]
        public Professor Professor { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }
    }
}

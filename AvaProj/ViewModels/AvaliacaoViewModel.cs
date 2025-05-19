using System.ComponentModel.DataAnnotations;

namespace AvaProj.ViewModels
{
    public class AvaliacaoViewModel
    {
        public int MatriculaId { get; set; }

        public string DisciplinaNome { get; set; }
        public string ProfessorNome { get; set; }

        [Range(1, 10)]
        public int Nota { get; set; }

        public string Comentario { get; set; }

        public bool Recomendaria { get; set; }
    }
}

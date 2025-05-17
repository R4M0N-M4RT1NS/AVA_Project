using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AvaProj.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int MatriculaId { get; set; }
        public Matricula Matricula { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public bool Recomendaria { get; set; }
        public DateTime DataAvaliacao { get; set; }
    }
}

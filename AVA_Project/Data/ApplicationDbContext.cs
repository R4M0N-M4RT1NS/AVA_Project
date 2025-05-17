using AvaProj.Models;
using Microsoft.EntityFrameworkCore;

namespace AvaProj.Data {
    public class ApplicationDbContext : DbContext {
        private DbContext dbContext;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {
        }

        public ApplicationDbContext(DbContext dbContext) {
            this.dbContext = dbContext;
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<DisciplinaOfertada> DisciplinasOfertadas { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
    }
}
using AvaProj.Data;
using AvaProj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvaProj.Controllers {
    [Authorize(Roles = "Professor")]
    public class DashboardController : Controller {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var nomeUsuarioLogado = User.Identity.Name;

            // Encontra o professor pelo nome
            var professor = await _context.Professores
                .FirstOrDefaultAsync(p => p.Nome == nomeUsuarioLogado);

            if (professor == null) {
                return Unauthorized(); // ou NotFound(), caso prefira
            }

            // Pega os IDs das disciplinas ofertadas por esse professor
            var disciplinasIds = await _context.DisciplinasOfertadas
                .Where(d => d.ProfessorId == professor.Id)
                .Select(d => d.Id)
                .ToListAsync();

            // Filtra as avaliações das disciplinas desse professor
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Matricula)
                    .ThenInclude(m => m.DisciplinaOfertada)
                        .ThenInclude(d => d.Disciplina)
                .Where(a => disciplinasIds.Contains(a.Matricula.DisciplinaOfertadaId))
                .ToListAsync();

            var mediaTotal = avaliacoes.Any() ? avaliacoes.Average(a => a.Nota) : 0;

            // Agrupa por Data e Disciplina
            var mediasPorDisciplina = avaliacoes
                .GroupBy(a => new {
                    Data = a.DataAvaliacao.Date,
                    Disciplina = a.Matricula.DisciplinaOfertada.Disciplina.Nome
                })
                .Select(g => new {
                    Data = g.Key.Data,
                    Disciplina = g.Key.Disciplina,
                    Media = g.Average(a => a.Nota)
                })
                .OrderBy(x => x.Data)
                .ToList();

            // Labels para o gráfico
            var disciplinas = mediasPorDisciplina.Select(x => x.Disciplina).Distinct().ToList();
            var labels = mediasPorDisciplina.Select(x => x.Data.ToString("dd/MM/yyyy")).Distinct().ToList();

            // Dados para gráfico
            var dadosPorDisciplina = disciplinas.ToDictionary(
                disc => disc,
                disc => labels.Select(data =>
                    mediasPorDisciplina
                        .FirstOrDefault(x => x.Data.ToString("dd/MM/yyyy") == data && x.Disciplina == disc)?.Media ?? 0
                ).ToList()
            );

            // Comentários e notas para tabela inferior
            var avaliacoesDetalhadas = avaliacoes
                .Select(a => new {
                    Data = a.DataAvaliacao,
                    Comentario = a.Comentario,
                    Nota = a.Nota
                })
                .OrderByDescending(a => a.Data)
                .ToList();

            // ViewBags
            ViewBag.MediaTotal = mediaTotal;
            ViewBag.MediasPorDisciplina = mediasPorDisciplina;
            ViewBag.Labels = labels;
            ViewBag.Disciplinas = disciplinas;
            ViewBag.DadosPorDisciplina = dadosPorDisciplina;
            ViewBag.AvaliacoesDetalhadas = avaliacoesDetalhadas;
            ViewBag.NomeDisciplina = disciplinas.FirstOrDefault();


            return View();
        }





    }
}
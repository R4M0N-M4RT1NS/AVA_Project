using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AvaProj.Data;
using AvaProj.Models;
using AvaProj.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace AvaProj.Controllers
{
    [Authorize(Roles = "Aluno")]
    public class AvaliacaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AvaliacaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Avaliacao
        public async Task<IActionResult> Index()
        {
            var usuarioId = int.Parse(User.FindFirst("UsuarioId").Value);

            var disciplinas = await _context.Matriculas
                .Where(m => m.UsuarioId == usuarioId)
                .Include(m => m.DisciplinaOfertada)
                    .ThenInclude(d => d.Disciplina)
                .Include(m => m.DisciplinaOfertada)
                    .ThenInclude(d => d.Professor)
                .ToListAsync();

            return View(disciplinas);
        }

        // GET: Avaliacao/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Matricula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        // GET: Avaliacao/Create/5
        public async Task<IActionResult> Create(int? matriculaId)
        {
            if (matriculaId == null)
                return NotFound();

            var matricula = await _context.Matriculas
                .Include(m => m.DisciplinaOfertada.Disciplina)
                .Include(m => m.DisciplinaOfertada.Professor)
                .FirstOrDefaultAsync(m => m.Id == matriculaId);


            if (matricula == null)
                return NotFound();

            var viewModel = new AvaliacaoViewModel
            {
                MatriculaId = matricula.Id,
                
                DisciplinaNome = matricula.DisciplinaOfertada.Disciplina.Nome,
                ProfessorNome = matricula.DisciplinaOfertada.Professor.Nome
            };

            return View(viewModel);
        }



        // POST: Avaliacao/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AvaliacaoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var avaliacao = new Avaliacao
            {
                MatriculaId = model.MatriculaId,
                Nota = model.Nota,
                Comentario = model.Comentario,
                Recomendaria = model.Recomendaria,
                DataAvaliacao = DateTime.Now
            };

            _context.Add(avaliacao);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        
        // GET: Avaliacao/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
            {
                return NotFound();
            }
            ViewData["MatriculaId"] = new SelectList(_context.Matriculas, "Id", "Id", avaliacao.MatriculaId);
            return View(avaliacao);
        }

        // POST: Avaliacao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MatriculaId,Nota,Comentario,Recomendaria,DataAvaliacao")] Avaliacao avaliacao)
        {
            if (id != avaliacao.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avaliacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvaliacaoExists(avaliacao.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MatriculaId"] = new SelectList(_context.Matriculas, "Id", "Id", avaliacao.MatriculaId);
            return View(avaliacao);
        }

        // GET: Avaliacao/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Matricula)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (avaliacao == null)
            {
                return NotFound();
            }

            return View(avaliacao);
        }

        // POST: Avaliacao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao != null)
            {
                _context.Avaliacoes.Remove(avaliacao);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvaliacaoExists(int id)
        {
            return _context.Avaliacoes.Any(e => e.Id == id);
        }
        
    }
}

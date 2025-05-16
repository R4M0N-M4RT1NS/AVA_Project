using AvaProj.Data;
using AvaProj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaProj.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class DisciplinaOfertadaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisciplinaOfertadaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DisciplinaOfertada
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DisciplinasOfertadas.Include(d => d.Disciplina).Include(d => d.Professor);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DisciplinaOfertada/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplinaOfertada = await _context.DisciplinasOfertadas
                .Include(d => d.Disciplina)
                .Include(d => d.Professor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disciplinaOfertada == null)
            {
                return NotFound();
            }

            return View(disciplinaOfertada);
        }

        // GET: DisciplinaOfertada/Create
        public IActionResult Create()
        {
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nome");
            ViewData["ProfessorId"] = new SelectList(_context.Professores, "Id", "Nome");
            return View();
        }

        // POST: DisciplinaOfertada/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DisciplinaId,ProfessorId,Ano,Semestre")] DisciplinaOfertada disciplinaOfertada)
        {
            if (ModelState.IsValid)
            {
                _context.Add(disciplinaOfertada);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            else
            {
                // Log or inspect the ModelState errors
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }

            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nome", disciplinaOfertada.DisciplinaId);
            ViewData["ProfessorId"] = new SelectList(_context.Professores, "Id", "Nome", disciplinaOfertada.ProfessorId);
            return View(disciplinaOfertada);
        }

        // GET: DisciplinaOfertada/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplinaOfertada = await _context.DisciplinasOfertadas.FindAsync(id);
            if (disciplinaOfertada == null)
            {
                return NotFound();
            }
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nome", disciplinaOfertada.DisciplinaId);
            ViewData["ProfessorId"] = new SelectList(_context.Professores, "Id", "Nome", disciplinaOfertada.ProfessorId);
            return View(disciplinaOfertada);
        }

        // POST: DisciplinaOfertada/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DisciplinaId,ProfessorId,Ano,Semestre")] DisciplinaOfertada disciplinaOfertada)
        {
            if (id != disciplinaOfertada.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disciplinaOfertada);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisciplinaOfertadaExists(disciplinaOfertada.Id))
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
            ViewData["DisciplinaId"] = new SelectList(_context.Disciplinas, "Id", "Nome", disciplinaOfertada.DisciplinaId);
            ViewData["ProfessorId"] = new SelectList(_context.Professores, "Id", "Nome", disciplinaOfertada.ProfessorId);
            return View(disciplinaOfertada);
        }

        // GET: DisciplinaOfertada/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disciplinaOfertada = await _context.DisciplinasOfertadas
                .Include(d => d.Disciplina)
                .Include(d => d.Professor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disciplinaOfertada == null)
            {
                return NotFound();
            }

            return View(disciplinaOfertada);
        }

        // POST: DisciplinaOfertada/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var disciplinaOfertada = await _context.DisciplinasOfertadas.FindAsync(id);
            if (disciplinaOfertada != null)
            {
                _context.DisciplinasOfertadas.Remove(disciplinaOfertada);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisciplinaOfertadaExists(int id)
        {
            return _context.DisciplinasOfertadas.Any(e => e.Id == id);
        }
    }
}

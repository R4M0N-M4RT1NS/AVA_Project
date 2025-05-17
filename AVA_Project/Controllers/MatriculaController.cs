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
    public class MatriculaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatriculaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Matricula
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Matriculas.Include(m => m.DisciplinaOfertada).Include(m => m.Usuario);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Matricula/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.DisciplinaOfertada)
                .Include(m => m.Usuario)
                .Where(m => m.Usuario.Papel == "Aluno") // Filtra apenas alunos
                .FirstOrDefaultAsync(m => m.Id == id && m.Usuario.Papel == "Aluno");
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // GET: Matricula/Create
        public IActionResult Create()
        {
            ViewBag.DisciplinaOfertadaId = new SelectList(_context.DisciplinasOfertadas, "Id", "Id");
            ViewBag.UsuarioId = new SelectList(_context.Usuarios.Where(u => u.Papel == "Aluno"), "Id", "Id");


            return View();
        }

        // POST: Matricula/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,DisciplinaOfertadaId")] Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                _context.Add(matricula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.DisciplinaOfertadaId = new SelectList(_context.DisciplinasOfertadas, "Id", "Id", matricula.DisciplinaOfertadaId);
            ViewBag.UsuarioId = new SelectList(_context.Usuarios.Where(u => u.Papel == "Aluno"), "Id", "Id", matricula.UsuarioId);
            return View(matricula);
        }

        // GET: Matricula/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula == null)
            {
                return NotFound();
            }
            ViewBag.DisciplinaOfertadaId = new SelectList(_context.DisciplinasOfertadas, "Id", "Id", matricula.DisciplinaOfertadaId);
            ViewBag.UsuarioId = new SelectList(_context.Usuarios.Where(u => u.Papel == "Aluno"), "Id", "Id", matricula.UsuarioId);
            return View(matricula);
        }

        // POST: Matricula/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,DisciplinaOfertadaId")] Matricula matricula)
        {
            if (id != matricula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(matricula.Id))
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
            ViewBag.DisciplinaOfertadaId = new SelectList(_context.DisciplinasOfertadas, "Id", "Id", matricula.DisciplinaOfertadaId);
            ViewBag.UsuarioId = new SelectList(_context.Usuarios.Where(u => u.Papel == "Aluno"), "Id", "Id", matricula.UsuarioId);
            return View(matricula);
        }

        // GET: Matricula/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.DisciplinaOfertada)
                .Include(m => m.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // POST: Matricula/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int id)
        {
            return _context.Matriculas.Any(e => e.Id == id);
        }
    }
}

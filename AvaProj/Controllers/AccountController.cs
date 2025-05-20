using AvaProj.Models;
using AvaProj.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using AvaProj.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public AccountController(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existingUser = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == model.Email || u.Matricula == model.Matricula);

        if (existingUser != null)
        {
            ModelState.AddModelError("", "Usuário já existe com este email ou matrícula.");
            return View(model);
        }

        var usuario = new Usuario
        {
            Nome = model.Nome,
            Email = model.Email,
            Matricula = model.Matricula,
            Papel = model.Papel,
            SenhaHash = _passwordHasher.HashPassword(null, model.Senha)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // Criação do cookie de autenticação
        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim("UsuarioId", usuario.Id.ToString()),
        new Claim(ClaimTypes.Role, usuario.Papel)
    };

        var identity = new ClaimsIdentity(claims, "Login");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Login", "Account");
    }


    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == model.Identificador || u.Matricula == model.Identificador);

        if (usuario == null ||
            _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha) != PasswordVerificationResult.Success)
        {
            ModelState.AddModelError("", "Usuário ou senha inválidos");
            return View(model);
        }

        // Criação do cookie de autenticação
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim("UsuarioId", usuario.Id.ToString()),
            new Claim(ClaimTypes.Role, usuario.Papel)
        };

        var identity = new ClaimsIdentity(claims, "Login");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        switch (usuario.Papel)
        {
            case "Aluno":
                return RedirectToAction("Index", "Home");

            case "Professor":
                return RedirectToAction("Index", "Home");

            case "Administrador":
                return RedirectToAction("Index", "Home");

            default:
                return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Iltuttofare.Api.Data;
using Iltuttofare.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Iltuttofare.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Subcategorias)
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("categorias-com-profissionais")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriasComProfissionais()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Subcategorias)
                    .ThenInclude(s => s.ProfissionalSubcategorias)
                        .ThenInclude(ps => ps.Profissional)
                .Select(c => new
                {
                    c.Id,
                    c.Nome,
                    Subcategorias = c.Subcategorias.Select(s => new
                    {
                        s.Id,
                        s.Nome,
                        Profissionais = s.ProfissionalSubcategorias.Select(ps => new
                        {
                            ps.Profissional.Id,
                            ps.Profissional.NomeCompleto,
                            ps.Profissional.Email,
                            ps.Profissional.Telefone
                        }).Distinct()
                    })
                })
                .ToListAsync();

            return Ok(categorias);
        }

        [HttpGet("categorias-com-subcategorias-e-profissionais")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoriasComSubcategoriasEProfissionais()
        {
            var categorias = await _context.Categorias
                .Include(c => c.Subcategorias)
                    .ThenInclude(s => s.ProfissionalSubcategorias)
                        .ThenInclude(ps => ps.Profissional)
                .Select(c => new
                {
                    c.Id,
                    c.Nome,
                    Subcategorias = c.Subcategorias.Select(s => new
                    {
                        s.Id,
                        s.Nome,
                        Profissionais = s.ProfissionalSubcategorias.Select(ps => new
                        {
                            ps.Profissional.Id,
                            ps.Profissional.NomeCompleto,
                            ps.Profissional.Telefone,
                            ps.Profissional.Email
                        }).Distinct()
                    })
                })
                .ToListAsync();

            return Ok(categorias);
        }

    }
}

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
    }
}

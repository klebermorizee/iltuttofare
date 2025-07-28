using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Iltuttofare.Api.Data;
using Microsoft.EntityFrameworkCore;
using Iltuttofare.Api.Models.Requests;
using Iltuttofare.Api.Models;
using Iltuttofare.Api.Models.Responses;

namespace Iltuttofare.Api.Controllers
{
    [ApiController]
    [Route("me")]
    [Authorize]
    public class ProfissionalController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfissionalController(AppDbContext context)
        {
            _context = context;
        }

        private int ObterIdDoUsuarioAutenticado()
        {
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "id");

            if (idClaim == null)
                throw new Exception("ID do usuário não encontrado no token.");

            return int.Parse(idClaim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetMe()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int profissionalId))
                return Unauthorized();

            var profissional = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                    .ThenInclude(ps => ps.Subcategoria)
                .FirstOrDefaultAsync(p => p.Id == profissionalId);

            if (profissional == null)
                return NotFound("Profissional não encontrado.");

            return Ok(new
            {
                profissional.Id,
                profissional.NomeCompleto,
                profissional.Telefone,
                profissional.Email,
                profissional.Cep,
                profissional.CodiceFiscale,
                profissional.DataNascimento,
                profissional.Genero,
                Subcategorias = profissional.ProfissionalSubcategorias.Select(s => new
                {
                    s.Subcategoria.Id,
                    s.Subcategoria.Nome
                })
            });
        }
 
        [HttpPut]
        public async Task<IActionResult> AtualizarMe([FromBody] AtualizarDadosRequest request)
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idClaim) || !int.TryParse(idClaim, out int profissionalId))
                return Unauthorized();

            var profissional = await _context.Profissionais.FindAsync(profissionalId);

            if (profissional == null)
                return NotFound("Profissional não encontrado.");

            // Atualiza apenas se os campos forem enviados
            if (!string.IsNullOrWhiteSpace(request.NomeCompleto))
                profissional.NomeCompleto = request.NomeCompleto;

            if (!string.IsNullOrWhiteSpace(request.Email))
                profissional.Email = request.Email;

            if (!string.IsNullOrWhiteSpace(request.Telefone))
                profissional.Telefone = request.Telefone;

            if (!string.IsNullOrWhiteSpace(request.Cep))
                profissional.Cep = request.Cep;

            if (!string.IsNullOrWhiteSpace(request.CodiceFiscale))
                profissional.CodiceFiscale = request.CodiceFiscale;

            if (!string.IsNullOrWhiteSpace(request.Genero))
                profissional.Genero = request.Genero;

            if (request.DataNascimento.HasValue)
                profissional.DataNascimento = DateTime.SpecifyKind(request.DataNascimento.Value, DateTimeKind.Utc);


            await _context.SaveChangesAsync();

            return Ok(new { message = "Dados atualizados com sucesso." });
        }
        [Authorize]
        [HttpPut("me/subcategorias")]
        public async Task<IActionResult> AtualizarSubcategorias([FromBody] List<int> subcategoriasIds)
        {
            var profissionalId = ObterIdDoUsuarioAutenticado(); // implementa isso como preferir

            var profissional = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                .FirstOrDefaultAsync(p => p.Id == profissionalId);

            if (profissional == null)
                return NotFound();

            // Remove vínculos antigos
            _context.ProfissionalSubcategorias.RemoveRange(profissional.ProfissionalSubcategorias);

            // Adiciona os novos vínculos
            var novasRelacoes = subcategoriasIds.Select(id => new ProfissionalSubcategoria
            {
                ProfissionalId = profissionalId,
                SubcategoriaId = id,
                DataCadastro = DateTime.UtcNow
            });

            await _context.ProfissionalSubcategorias.AddRangeAsync(novasRelacoes);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Subcategorias atualizadas com sucesso." });
        }

        [AllowAnonymous]
        [HttpGet("pub/ProfissionalSubcategorias")]
        public async Task<IActionResult> BuscarPorSubcategoria([FromQuery] int subcategoriaId)
        {
            var profissionais = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                    .ThenInclude(ps => ps.Subcategoria)
                .Where(p => p.ProfissionalSubcategorias.Any(ps => ps.SubcategoriaId == subcategoriaId))
                .Select(p => new ProfissionalPublicoResponse
                {
                    Id = p.Id,
                    Nome = p.NomeCompleto,
                    Email = p.Email,
                    Subcategorias = p.ProfissionalSubcategorias.Select(ps => ps.Subcategoria.Nome).ToList()
                })
                .ToListAsync();

            return Ok(profissionais);
        }

        [AllowAnonymous] 
        [HttpGet("pub/profissionais")]
        public async Task<IActionResult> ListarProfissionais()
        {
            var profissionais = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                    .ThenInclude(ps => ps.Subcategoria)
                .Select(p => new
                {
                    p.Id,
                    p.NomeCompleto,
                    p.Email,
                    p.Telefone,
                    Subcategorias = p.ProfissionalSubcategorias.Select(ps => new
                    {
                        ps.Subcategoria.Id,
                        ps.Subcategoria.Nome
                    })
                })
                .ToListAsync();

            return Ok(profissionais);
        }

        [HttpGet("pub/profissionais/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObterProfissionalPorId(int id)
        {
            var profissional = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                    .ThenInclude(ps => ps.Subcategoria)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.NomeCompleto,
                    p.Email,
                    p.Telefone,
                    Subcategorias = p.ProfissionalSubcategorias.Select(ps => new
                    {
                        ps.Subcategoria.Id,
                        ps.Subcategoria.Nome
                    })
                })
                .FirstOrDefaultAsync();

            if (profissional == null)
                return NotFound();

            return Ok(profissional);
        }

        [HttpGet("pub/profissionais/subcategoria/{subcategoriaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> PubBuscarPorSubcategoria(int subcategoriaId)
        {
            var profissionais = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                    .ThenInclude(ps => ps.Subcategoria)
                .Where(p => p.ProfissionalSubcategorias.Any(ps => ps.SubcategoriaId == subcategoriaId))
                .Select(p => new
                {
                    p.Id,
                    p.NomeCompleto,
                    p.Email,
                    p.Telefone,
                    Subcategorias = p.ProfissionalSubcategorias.Select(ps => new
                    {
                        ps.Subcategoria.Id,
                        ps.Subcategoria.Nome
                    })
                })
                .ToListAsync();

            return Ok(profissionais);
        }


    }

}


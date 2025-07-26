using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Iltuttofare.Api.Data;
using Iltuttofare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Iltuttofare.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AppDbContext context, IConfiguration config, ILogger<AuthController> logger)
        {
            _context = context;
            _config = config;
            _logger = logger;
        }

        [HttpPost("request-code")]
        public async Task<IActionResult> RequestCode([FromBody] AuthRequest request)
        {
            var profissional = await _context.Profissionais
                .FirstOrDefaultAsync(p => p.Telefone == request.Telefone);

            if (profissional == null)
                return BadRequest("Telefone não encontrado.");

            var codigo = new Random().Next(1000, 9999).ToString();

            var sessao = new SessaoCadastro
            {
                Telefone = request.Telefone,
                Email = profissional.Email ?? "login@iltuttofare.it",
                CodigoSMS = codigo,
                Validado = false
            };

            _context.SessoesCadastro.Add(sessao);
            await _context.SaveChangesAsync();

            _logger.LogInformation("[LOGIN] SMS para {Telefone}: código {Codigo}", request.Telefone, codigo);

            return Ok(new { message = "Código SMS enviado." });
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] AuthVerify request)
        {
            var sessao = await _context.SessoesCadastro
                .OrderByDescending(s => s.CriadoEm)
                .FirstOrDefaultAsync(s => s.Telefone == request.Telefone && s.CodigoSMS == request.CodigoSms);

            if (sessao == null)
                return BadRequest("Código inválido.");

            var profissional = await _context.Profissionais
                .FirstOrDefaultAsync(p => p.Telefone == request.Telefone);

            if (profissional == null)
                return BadRequest("Profissional não encontrado.");

            var token = GerarTokenJwt(profissional);

            return Ok(new { token });
        }

        private string GerarTokenJwt(Profissional profissional)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, profissional.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, profissional.Telefone)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class AuthRequest
        {
            public string Telefone { get; set; }
        }

        public class AuthVerify
        {
            public string Telefone { get; set; }
            public string CodigoSms { get; set; }
        }
    }
}

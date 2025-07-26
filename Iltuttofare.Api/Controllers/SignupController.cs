using Microsoft.AspNetCore.Mvc;
using Iltuttofare.Api.Models;
using Iltuttofare.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Iltuttofare.Api.Controllers
{
    [ApiController]
    [Route("signup")]
    public class SignupController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SignupController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartCadastro([FromBody] SignupRequest request)
        {
            if (string.IsNullOrEmpty(request.Telefone) || string.IsNullOrEmpty(request.Email))
                return BadRequest("Telefone e Email são obrigatórios.");

            var codigo = new Random().Next(1000, 9999).ToString();

            var sessao = new SessaoCadastro
            {
                Telefone = request.Telefone,
                Email = request.Email,
                CodigoSMS = codigo
            };

            _context.SessoesCadastro.Add(sessao);
            await _context.SaveChangesAsync();

            // Por enquanto, só simula envio de SMS (log)
            Console.WriteLine($"[DEV] SMS para {request.Telefone}: código {codigo}");

            return Accepted(new { message = "SMS enviado com sucesso." });
        }

        [HttpPost("verify-sms")]
        public async Task<IActionResult> VerificarCodigo([FromBody] VerifySmsRequest request)
        {
            var sessao = await _context.SessoesCadastro
                .FirstOrDefaultAsync(s => s.Telefone == request.Telefone && s.CodigoSMS == request.CodigoSms);

            if (sessao == null)
                return BadRequest("Código incorreto ou número inválido.");

            sessao.Validado = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Código validado com sucesso." });
        }

        [HttpPost("details")]
        public async Task<IActionResult> SalvarDadosIniciais([FromBody] DadosIniciaisRequest request)
        {
            // Verifica se o número foi validado via SMS
            var sessao = await _context.SessoesCadastro
                .FirstOrDefaultAsync(s => s.Telefone == request.Telefone && s.Validado);

            if (sessao == null)
                return BadRequest("Número não validado.");

            var profissional = new Profissional
            {
                NomeCompleto = request.NomeCompleto,
                Cep = request.Cep,
                Telefone = request.Telefone
            };

            _context.Profissionais.Add(profissional);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dados iniciais salvos com sucesso." });
        }

        [HttpPost("complete")]
        public async Task<IActionResult> FinalizarCadastro([FromBody] FinalizarCadastroRequest request)
        {
            var profissional = await _context.Profissionais
                .FirstOrDefaultAsync(p => p.Telefone == request.Telefone);

            if (profissional == null)
                return BadRequest("Profissional não encontrado.");

            profissional.DataNascimento = DateTime.SpecifyKind(
                request.DataNascimento, DateTimeKind.Utc
            );

            profissional.CodiceFiscale = request.CodiceFiscale;
            profissional.DataNascimento = DateTime.SpecifyKind(request.DataNascimento, DateTimeKind.Utc);
            profissional.Genero = request.Genero;


            await _context.SaveChangesAsync();

            return Ok(new { message = "Cadastro finalizado com sucesso." });
        }

        [HttpPost("select-subcategorias")]
        public async Task<IActionResult> SelecionarSubcategorias([FromBody] SubcategoriaSelectionRequest request)
        {
            var profissional = await _context.Profissionais
                .Include(p => p.ProfissionalSubcategorias)
                .FirstOrDefaultAsync(p => p.Telefone == request.Telefone);

            if (profissional == null)
                return BadRequest("Profissional não encontrado.");

            // Remove anteriores (caso reenvie)
            profissional.ProfissionalSubcategorias.Clear();

            foreach (var subId in request.SubcategoriasIds)
            {
                profissional.ProfissionalSubcategorias.Add(new ProfissionalSubcategoria
                {
                    ProfissionalId = profissional.Id,
                    SubcategoriaId = subId
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "Subcategorias salvas com sucesso." });
        }
    }

    public class FinalizarCadastroRequest
    {
        public string Telefone { get; set; }
        public string CodiceFiscale { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; }
    }

    public class SignupRequest
    {
        public string Telefone { get; set; }
        public string Email { get; set; }
    }

    public class VerifySmsRequest
    {
        public string Telefone { get; set; }
        public string CodigoSms { get; set; }
    }

     public class DadosIniciaisRequest
    {
        public string Telefone { get; set; }
        public string NomeCompleto { get; set; }
        public string Cep { get; set; }
    }
    public class SubcategoriaSelectionRequest
    {
        public string Telefone { get; set; }
        public List<int> SubcategoriasIds { get; set; }
    }

}

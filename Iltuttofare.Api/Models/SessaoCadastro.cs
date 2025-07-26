using System;

namespace Iltuttofare.Api.Models
{
    public class SessaoCadastro
    {
        public int Id { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CodigoSMS { get; set; }
        public bool Validado { get; set; } = false;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}

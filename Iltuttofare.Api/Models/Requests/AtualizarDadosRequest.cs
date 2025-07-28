namespace Iltuttofare.Api.Models.Requests
{
    public class AtualizarDadosRequest
    {
        public string? NomeCompleto { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Cep { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? CodiceFiscale { get; set; }
        public string? Genero { get; set; }
    }
}

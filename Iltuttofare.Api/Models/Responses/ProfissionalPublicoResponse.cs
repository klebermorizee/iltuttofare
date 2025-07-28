namespace Iltuttofare.Api.Models.Responses
{
    public class ProfissionalPublicoResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public List<string> Subcategorias { get; set; }
    }
}

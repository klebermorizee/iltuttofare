namespace Iltuttofare.Api.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public ICollection<Subcategoria> Subcategorias { get; set; }
    }
}

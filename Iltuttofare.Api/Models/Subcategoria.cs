namespace Iltuttofare.Api.Models
{
    public class Subcategoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
        public ICollection<ProfissionalSubcategoria> ProfissionalSubcategorias { get; set; }
    }
}

namespace Iltuttofare.Api.Models
{
    public class ProfissionalSubcategoria
    {
        public int ProfissionalId { get; set; }
        public int SubcategoriaId { get; set; }

        public Profissional Profissional { get; set; }
        public Subcategoria Subcategoria { get; set; }
    }
}

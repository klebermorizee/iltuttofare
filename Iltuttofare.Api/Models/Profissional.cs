using System;

namespace Iltuttofare.Api.Models
{
    public class Profissional
    {
        public int Id { get; set; }

        public string NomeCompleto { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        // Campos que virão na etapa seguinte
        public string? CodiceFiscale { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string? Genero { get; set; }
        public ICollection<ProfissionalSubcategoria> ProfissionalSubcategorias { get; set; }
        public ICollection<Subcategoria> Subcategorias { get; set; } = new List<Subcategoria>();


    }
}

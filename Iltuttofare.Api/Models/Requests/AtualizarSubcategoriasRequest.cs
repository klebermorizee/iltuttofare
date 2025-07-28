using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class AtualizarSubcategoriasRequest
{
    [Required]
    [JsonPropertyName("subcategoriasIds")]
    public List<int> SubcategoriasIds { get; set; } = new();
}

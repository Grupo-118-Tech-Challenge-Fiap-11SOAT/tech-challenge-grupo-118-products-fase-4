using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

[ExcludeFromCodeCoverage]
public class DessertDto : ProductDto
{
    [JsonPropertyName("portionSize")]
    public string PortionSize { get; set; } = null!;

    public DessertDto()
    {
        
    }
}

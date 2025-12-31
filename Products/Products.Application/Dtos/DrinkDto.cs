using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

[ExcludeFromCodeCoverage]
public class DrinkDto : ProductDto
{
    [JsonPropertyName("size")]
    public string Size { get; set; } = null!;

    [JsonPropertyName("flavor")]
    public string? Flavor { get; set; }

    public DrinkDto()
    {
        
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

[ExcludeFromCodeCoverage]
public class SnackDto : ProductDto
{
    [JsonRequired]
    [JsonPropertyName("ingredients")]
    public List<string> Ingredients { get; set; } = new();

    public SnackDto()
    {
        
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

[ExcludeFromCodeCoverage]
public class AccompanimentDto : ProductDto
{
    [JsonPropertyName("size")]
    public string Size { get; set; } = null!;

    public AccompanimentDto()
    {
        
    }
}

using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

public class AccompanimentDto : ProductDto
{
    [JsonPropertyName("size")]
    public string Size { get; set; } = null!;

    public AccompanimentDto()
    {
        
    }
}

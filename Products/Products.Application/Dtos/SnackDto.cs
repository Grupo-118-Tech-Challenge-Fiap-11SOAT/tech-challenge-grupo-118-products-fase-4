using System.Text.Json.Serialization;

namespace Products.Application.Dtos;

public class SnackDto : ProductDto
{
    [JsonRequired]
    [JsonPropertyName("ingredients")]
    public List<string> Ingredients { get; set; } = new();

    public SnackDto()
    {
        
    }
}

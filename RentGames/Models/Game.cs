using System.Text.Json.Serialization;

namespace Models.Game;

public record Game(Guid Id, float Price, string Name, bool IsPromo, bool IsStock)
{    
    [JsonConstructor]
    public Game(float price, string name, bool isPromo, bool isStock) : this(Guid.NewGuid(), price, name, isPromo, isStock)
    {
    }
}


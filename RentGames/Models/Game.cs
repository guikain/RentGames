namespace Models.Game;

public record Game(Guid Id, float Price, string Name, bool IsPromo, bool IsStock);


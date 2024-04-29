
using Models.Game;
using Microsoft.AspNetCore.Mvc;
using RentGames.Data;

class MyStore{
    public static void Main(String[] args) {

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

       //conectando com o bando de dados!
       builder.Services.AddDbContext<AppDbContext>();
        

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApiDocument(config =>
        {
            config.DocumentName = "RentAGame";
            config.Title = "RentGames";
            config.Version = "v1";
        });
        
        WebApplication app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi();
            app.UseSwaggerUi(config =>
            {
                config.DocumentTitle = "RentGames API";
                config.Path = "/swagger";
                config.DocumentPath = "/swagger/{documentName}/swagger.json";
                config.DocExpansion = "list";
            });
        }

        //meus endpoints!

        //1
        app.MapGet("/games", ([FromServices] AppDbContext context) => {
            var games = context.Games.ToList();
            return games.Any() ? Results.Ok(games) : Results.NotFound();
        }).Produces<List<Game>>();

        //2
        app.MapGet("/games/promo", ([FromServices] AppDbContext context) => {
            var promos = context.Games.Where(g => g.IsPromo).ToList();
            return promos.Any() ? Results.Ok(promos) : Results.NotFound("Não foi localizada nenhuma promoção.");
        }).Produces<List<Game>>();

        //3
        app.MapGet("/games/{id}", ([FromServices] AppDbContext context, [FromRoute] Guid id) => {
            var game = context.Games.FirstOrDefault(g => g.Id == id);
            return game != null ? Results.Ok(game) : Results.NotFound("Jogo não encontrado.");
        }).Produces<Game>();

        //4
        app.MapPost("/games", ([FromServices] AppDbContext context, Game game) => {
            context.Games.Add(game);
            context.SaveChanges();
            return Results.Created($"/games/{game.Id}", game);
        }).Produces<Game>();

        //5
        app.MapPut("/games/{id}", ([FromServices] AppDbContext context, [FromRoute] Guid id, [FromBody] Game updatedGame) => {
            var existingGame = context.Games.FirstOrDefault(g => g.Id == id);
            if (existingGame == null) {
                return Results.NotFound("Jogo não encontrado.");
            }
            var newGame = existingGame with {
                Price = updatedGame.Price,
                Name = updatedGame.Name,
                IsPromo = updatedGame.IsPromo
            };
            context.Games.Remove(existingGame);
            context.Games.Add(newGame);
            context.SaveChanges();

            return Results.Ok(newGame);
        }).Produces<Game>();

        //6
        app.MapDelete("/games/{id}", ([FromServices] AppDbContext context, [FromRoute] Guid id) => {
            var game = context.Games.FirstOrDefault(g => g.Id == id);
            if (game != null) {
                context.Games.Remove(game);
                context.SaveChanges();
                return Results.Ok(game);
            }
            return Results.NotFound("Jogo não encontrado.");
        }).Produces<Game>();


        //7
        app.MapPatch("/games/{id}/stock", ([FromServices] AppDbContext context, [FromRoute] Guid id, [FromBody] bool isStock) => {
            var game = context.Games.Find(id);
            if (game == null) {
                return Results.NotFound("Jogo não encontrado.");
            }
            
            var updatedGame = game with { IsStock = isStock };
            context.Entry(game).CurrentValues.SetValues(updatedGame);
            context.SaveChanges();
            
            return Results.Ok(updatedGame);
        }).Produces<Game>().WithName("UpdateGamePromoStatus");


        //Criando dados previamente para testar
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            if (!context.Games.Any())
            {
                context.Games.AddRange(
                    new Game(59.99f, "The Witcher 3", false, true),
                    new Game(40.00f, "Cyberpunk 2077", true, true),
                    new Game(10.00f, "O Jogo", true, true)
                );
                context.SaveChanges();
            }
        }


        app.Run();
    }
}
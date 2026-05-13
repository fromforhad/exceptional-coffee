using Microsoft.EntityFrameworkCore;
using CoffeeData;
using CoffeeModel;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Coffees") ?? "Data Source=Coffees.db";
builder.Services.AddSqlite<CoffeeDb>(connectionString);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// ----------------------------- CRUD ----------------------------- //
// Retrieve all coffees
app.MapGet("/coffee", (CoffeeDb db) =>
{
    return db.Coffees.ToList();
});

// Retrieve one coffee
app.MapGet("/coffee/{name}", async (string name, CoffeeDb db) =>
    await db.Coffees.FirstOrDefaultAsync(c => c.Name == name)
        is Coffee coffee 
            ? Results.Ok(coffee) 
            : Results.NotFound());

// Add a coffee
app.MapPost("/coffee/add", (Coffee coffee, CoffeeDb db) =>
{
    db.Coffees.Add(coffee);
    db.SaveChanges();
    return Results.Created($"/coffees/{coffee.Id}", coffee);
});

// Update a coffee
app.MapPut("/coffee/update/{id}", (int id, Coffee coffee, CoffeeDb db) =>
{
    var newCoffee = db.Coffees.Find(id);
    if (newCoffee is null) return Results.NotFound();

    newCoffee.Name = coffee.Name;
    newCoffee.Size = coffee.Size;
    newCoffee.Quantity = coffee.Quantity;
    newCoffee.Price = coffee.Price;

    db.SaveChanges();
    return Results.NoContent();
});

// Delete a coffee
app.MapDelete("/coffee/delete/{id}", (int id, CoffeeDb db) =>
{
    var deleteCoffee = db.Coffees.Find(id);
    if (deleteCoffee is null) return Results.NotFound();

    db.Coffees.Remove(deleteCoffee);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run("http://localhost:5000");

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/users", () => "[users]");
app.MapGet("/user/{id}", (int id) => $"user {id}");

app.Run();

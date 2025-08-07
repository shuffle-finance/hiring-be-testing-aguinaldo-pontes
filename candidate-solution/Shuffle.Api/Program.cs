var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.MapGet("/users/{userId}/transactions", (userId) =>
{
    return Task.FromResult(userId + "transactions");
});

app.MapGet("/users/{userId}/balance", (userId) =>
{
    return Task.FromResult(userId + "balance");
});

app.Run();


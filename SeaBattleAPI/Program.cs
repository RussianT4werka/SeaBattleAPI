using SeaBattleAPI.DB;
using SeaBattleAPI.Tools;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<user50_battleContext>();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(s => s.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR().AddJsonProtocol(s=>s.PayloadSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<MainHub>("Hub");

app.Run();
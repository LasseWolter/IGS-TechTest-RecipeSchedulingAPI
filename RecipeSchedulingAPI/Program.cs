using RecipeSchedulingAPI.Enums;
using RecipeSchedulingAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/schedule", () =>
    {
        Schedule schedule = new Schedule();
        schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Water, waterAmount: 3));
        schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Light, lightIntesnity: LightIntensity.High));
        return schedule;
    })
    .WithName("GetSchedule")
    .WithOpenApi();

app.Run();

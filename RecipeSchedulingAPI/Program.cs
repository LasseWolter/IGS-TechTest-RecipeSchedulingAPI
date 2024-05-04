using System.Text.Json;
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
        var rawJsonString = """
                            {
                              "input": [
                                {
                                  "trayNumber": 1,
                                  "recipeName": "Basil",
                                  "startDate": "2022-01-24T12:30:00.0000000Z"
                                },
                                {
                                  "trayNumber": 2,
                                  "recipeName": "Strawberries",
                                  "startDate": "2021-13-08T17:33:00.0000000Z"
                                },
                                {
                                  "trayNumber": 3,
                                  "recipeName": "Basil",
                                  "startDate": "2030-01-01T23:45:00.0000000Z"
                                }
                              ]
                            }
                            """;

        InputData inputData = JsonSerializer.Deserialize<InputData>(rawJsonString);
        Schedule schedule = new Schedule();
        schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Water, waterAmount: 3));
        schedule.Events.Add(new Event(DateTime.UtcNow, CommandType.Light, lightIntesnity: LightIntensity.High));
        return schedule;
    })
    .WithName("GetSchedule")
    .WithOpenApi();

app.Run();
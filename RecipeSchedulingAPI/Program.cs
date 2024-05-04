using RecipeSchedulingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// TODO: Add DI
builder.Services.AddControllers();
builder.Logging.AddConsole();
builder.Services.AddScoped<SchedulingService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Handle unhandled exceptions

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
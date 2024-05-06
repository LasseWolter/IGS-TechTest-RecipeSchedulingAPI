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
// REMARK: In most cases you wouldn't want to enable swagger in production. It would leak details about your API that you might want to keep private. 
// Here, I'm enabling it without an environment flag to make it easy to review/explore my API and also make it part of my docker image which is built in release mode.
app.UseSwagger();
app.UseSwaggerUI();

// Handle unhandled exceptions

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
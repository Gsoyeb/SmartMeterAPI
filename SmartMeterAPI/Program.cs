using Microsoft.EntityFrameworkCore;
using SmartMeterAPI.Infrastracture.Data;
using SmartMeterAPI.Infrastracture.Repositories.IRepositories;
using SmartMeterAPI.Infrastracture.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IMeterReaderRepository, MeterReaderRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Use CORS policy
app.UseCors("AllowReactApp");

app.MapControllers();


// Seed the data
string seedFileName = builder.Configuration.GetValue<string>("SeedPath");
string baseDirectory = AppContext.BaseDirectory;
string seedPath = Path.Combine(baseDirectory, "..", "..", "..", "..", seedFileName);
CustomerSeeder.SeedCSV(app.Services, seedPath);

app.Run();

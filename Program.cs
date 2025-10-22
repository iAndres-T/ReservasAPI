using Microsoft.EntityFrameworkCore;
using ReservasAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBReservasContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AzureConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null
            );
        }));


// Add services to the container.
//builder.WebHost.UseUrls("http://*:80");
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

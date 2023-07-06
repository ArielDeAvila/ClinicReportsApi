using ClinicReportsAPI.Data;
using ClinicReportsAPI.Extensions;
using ClinicReportsAPI.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string Cors = "Cors";

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Cors,
            policy =>
            {
                policy.WithOrigins("*");
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            }
        );
});

builder.Services.AddDbContext<SystemReportContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SystemReport"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddInjectionServices();
builder.Services.AddAutentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    SystemReportContext context = scope.ServiceProvider.GetRequiredService<SystemReportContext>();
    context.Database.Migrate();
}

QuestPDF.Settings.License = LicenseType.Community;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

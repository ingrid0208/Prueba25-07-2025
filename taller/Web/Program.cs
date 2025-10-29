using Entity.Domain.Config;
using Entity.Validations.Interfaces;
using Entity.Validations.Modules.Auth;
using Entity.Validations.Service;
using FluentValidation;
using FluentValidation.AspNetCore;
using Web.Extensions;
using Web.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Services 
builder.Services.AddApplicationServices();
builder.Services.AddSwaggerWithJwt();
builder.Services.AddDatabase(builder.Configuration);

//Jwt y Cookie
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomCors(builder.Configuration);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CookieSettings>(builder.Configuration.GetSection("Cookie"));

//Validations
builder.Services.AddScoped<IValidatorService, ValidatorService>();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

//app.UseMiddleware<DbContextMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


MigrationManager.MigrateAllDatabases(app.Services, builder.Configuration);


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

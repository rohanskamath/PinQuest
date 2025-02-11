using dotnetcorebackend;
using dotnetcorebackend.Application.Repositories.UserRepository;
using dotnetcorebackend.Application.Services.UserService.Commands;
using dotnetcorebackend.Infrastructure.Context;
using dotnetcorebackend.Infrastructure.Mappings;
using DotNetEnv;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*************************************************************************************/

// Jwt Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            AuthenticationType="Jwt",
            ValidateIssuer = true, // Ensures valid Issuer is matched
            ValidateAudience = true, // Ensures valid Audience is matched
            ValidateLifetime = true, // Prevents expired token from been accepted
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Injecting ApplicationDBContext class
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PinQuestDBConnection")));

// Injecting MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddFluentValidationAutoValidation()
   .AddFluentValidationClientsideAdapters()
   .AddValidatorsFromAssemblyContaining<RegisterUserCommandValidator>();

// Injecting AutoMapper
builder.Services.AddAutoMapper(typeof(UserProfile));

// Injecting Fluent Validation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Injecting UserRepositoryImplementation class with Interface
builder.Services.AddScoped<IUserRepository, UserRepositoryImplementation>();

/****************************************************************************************/

//Injecting Controller class
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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

// Using CORS Policy
app.UseCors("corspolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();

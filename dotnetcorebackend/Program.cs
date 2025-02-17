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
using dotnetcorebackend.Infrastructure.Middleware;
using Serilog;
using Serilog.Events;
using dotnetcorebackend.Application.Repositories.PinsRepository;
using dotnetcorebackend.Application.Repositories.EmailRepository;
using dotnetcorebackend.Application.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
/*************************************************************************************/

// Injecting Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Logs to Console
    .WriteTo.File("Logs/user_activity.log",
                  rollingInterval: RollingInterval.Day,  // Creates a new log file every day
                  retainedFileCountLimit: 7)  // Keeps logs for the last 7 days
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

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

// Injecting AutoMapper (Multiple files)
builder.Services.AddAutoMapper(typeof(Program));

// Injecting IMemoryCache to store OTP in Memory
builder.Services.AddMemoryCache();

// Injecting Fluent Validation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<TokenCreationHelper>();

// Injecting UserRepositoryImplementation class with Interface
builder.Services.AddScoped<IUserRepository, UserRepositoryImplementation>();

// Injecting PinsRepositoryImplementation class with Interface
builder.Services.AddScoped<IPinsRepository,PinsRepositoryImplementation>();

// Injecting EmailRepositoryImplementation class with Interface
builder.Services.AddScoped<IEmailService, EmailServiceRepository>();

/****************************************************************************************/

//Injecting Controller class
builder.Services.AddControllers();
builder.Services.AddLogging();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corspolicy", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
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

// Use User activity Middleware
app.UseMiddleware<UserActivityMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

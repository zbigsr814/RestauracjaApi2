using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestauracjaApi2.Controllers;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Services;
using NLog.Web;
using RestauracjaApi2.Middleware;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using RestauracjaApi2.Models;
using System.ComponentModel.DataAnnotations;
using RestauracjaApi2.Models.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RestauracjaApi2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var authenticationSettings = new AuthenticationSettings();
            builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
            builder.Services.AddSingleton(authenticationSettings);

            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
                option.DefaultScheme = "Bearer";            // schemat który musi znajdowaæ siê od klienta w nag³ówku auth.
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;       // nie wymuszamy na kliencie metadanych
                cfg.SaveToken = true;       // token powinien byæ zapisany po stronie serwera
                cfg.TokenValidationParameters = new TokenValidationParameters       // parametry walidacji, czy token klient-serwer zgodny
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,     // okreœlony issuer (wydawca danego tokenu)
                    ValidAudience = authenticationSettings.JwtIssuer,       // audience - jakie podmioty mog¹ u¿ywaæ tego tokenu
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),  // primary key, zgodny z appsetings.json
                };
            });

            builder.Services.AddAuthorization(opion =>
            {
                opion.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "Polish", "German"));
            });

            // Add services to the container.
            builder.Services.AddControllers().AddFluentValidation();
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddScoped<RestaurantSeeder>();
            builder.Services.AddScoped<IRestaurantService, RestaurantService>();
            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<ErrorHandlingMiddleware>();
            builder.Services.AddScoped<TimeProcessMiddleware>();
            builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<RestaurantSeeder>();
                seeder.Seed();
            }

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restauracja API");
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<TimeProcessMiddleware>();
            app.Run();
        }
    }
}
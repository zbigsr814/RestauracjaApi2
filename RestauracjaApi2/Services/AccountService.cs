using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestauracjaApi2.Entities;
using RestauracjaApi2.Exceptions;
using RestauracjaApi2.Interfaces;
using RestauracjaApi2.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestauracjaApi2.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;

        public AccountService(RestaurantDbContext dbContext, IPasswordHasher<User> passwordHasher, AuthenticationSettings authenticationSettings)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }

        public string GenerateJwt(LoginDto dto)
        {
            // sprawdzenie loginu i hasła
            var user = dbContext
                .Users
                .Include(r => r.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user == null) 
                throw new BadRequestException("Nieprawidłowy login lub hasło");   // nie ma takiego użytkownika z tym Emailem

            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed) 
                throw new BadRequestException("Nieprawidłowy login lub hasło");   // nie ma takiego użytkownika z tym Emailem / hasłem

            // klient podał prawidłowe email i hasło / tworzymy claimy
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("dd-MM-yyyy"))
            };

            if (!user.Nationality.IsNullOrEmpty())
            {
                claims.Add(new Claim("Nationality", user.Nationality));
            }

            // tworzenie primary key na podstawie pliku appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));

            //  tworzenie credencials do podpisania klucza
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //  obliczanie daty aktywnego klucza
            var expires = DateTime.Now.AddDays(authenticationSettings.JwtExpireDays);

            // tworzenie tokenu
            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims: claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPassword = passwordHasher.HashPassword(newUser, dto.Password);
            newUser.PasswordHash = hashedPassword;

            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
        }
    }
}

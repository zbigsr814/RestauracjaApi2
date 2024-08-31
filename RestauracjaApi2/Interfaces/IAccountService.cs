using Microsoft.AspNetCore.Mvc;
using RestauracjaApi2.Models;

namespace RestauracjaApi2.Interfaces
{
    public interface IAccountService
    {
        public void RegisterUser(RegisterUserDto dto);
        public string GenerateJwt(LoginDto dto);
    }
}

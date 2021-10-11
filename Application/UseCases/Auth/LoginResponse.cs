using System;

namespace Application.UseCases.Auth
{
    public class LoginResponse
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpirationDate { get; set; }
    }
}
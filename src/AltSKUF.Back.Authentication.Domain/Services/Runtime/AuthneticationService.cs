using System.Security.Claims;
using AltSKUF.Back.Authentication.Domain.Extensions;
using AltSKUF.Back.Authentication.Infrastracture.Entety.Responces;

namespace AltSKUF.Back.Authentication.Domain.Services.Runtime
{
    public class AuthneticationService : IAuthenticationService
    {
        public TokensResponce GetTokens(Guid userId)
        {
            List<Claim> claims = [
                new("userId", userId.ToString())
                ];

            DateTime now = DateTime.Now;
            DateTime accessExpirationTime = now.Add(
                TimeSpan.FromMinutes(
                    int.Parse(Configuration.Singleton.AccesExpirationTimeInMinutes)));
            DateTime refreshExpirationTime = now.Add(
                TimeSpan.FromMinutes(
                    int.Parse(Configuration.Singleton.RefreshExpirationTimeInMinutes)));

            string aToken = JwtExtensions.GenerateToken(new()
            {
                Audience = "AltSKUF.Front",
                Issuer = "AltSKUF.Back",
                Claims = claims,
                Expires = accessExpirationTime,
                SymSecret = TokensSingleton.Singleton.AccessTokenSecret
            });
            string rToken = JwtExtensions.GenerateToken(new()
            {
                Audience = "AltSKUF.Front",
                Issuer = "AltSKUF.Back",
                Claims = [new("userId", userId.ToString())],
                Expires = refreshExpirationTime,
                SymSecret = TokensSingleton.Singleton.RefreshTokenSecret
            });

            return new()
            {
                AccessToken = aToken,
                RefreshToken = rToken
            };
        }

        public string GetServiceToken()
        {
            DateTime now = DateTime.Now;
            TimeSpan.FromMinutes(
                int.Parse(Configuration.Singleton.AccesExpirationTimeInMinutes));

            string token = JwtExtensions.GenerateToken(new()
            {
                Audience = "AltSKUF.Back",
                Issuer = "AltSKUF.Back",
                Claims = [],
                Expires = DateTime.Now.Add(TimeSpan.FromHours(1000)),
                Secret = Configuration.Singleton.ServicesSercret
            });

            return token;
        }
    }
}

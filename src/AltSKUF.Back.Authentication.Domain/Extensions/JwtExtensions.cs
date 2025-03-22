using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AltSKUF.Back.Authentication.Domain.Entety;
using AltSKUF.Back.Authentication.Domain.Extensions.CustomExceptions;
using Microsoft.IdentityModel.Tokens;

namespace AltSKUF.Back.Authentication.Domain.Extensions
{
    public static class JwtExtensions
    {
        public static string GenerateToken(GenerateTokenModel options)
        {
            SymmetricSecurityKey key;
            if (options.SymSecret != null)
                key = options.SymSecret;
            else key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret 
                ?? throw new NoFindSecretException()));

            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
            claims: options.Claims,
            expires: options.Expires,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha384));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public static string GetServicesToken(List<Claim> claims)
        {
            DateTime now = DateTime.Now;
            DateTime expirationTime = now.Add(
                TimeSpan.FromMinutes(
                    int.Parse(Configuration.Singleton.ServicesExpirationTimeInMinutes)));
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Configuration.Singleton.ServicesSercret));

            var jwt = new JwtSecurityToken(
            issuer: "AltSKUF.Back",
            audience: "AltSKUF.Back",
            claims: claims,
            expires: expirationTime,
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha384));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static string GenerateSecret()
        {
            var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var bytes = new byte[36];
            rng.GetBytes(bytes);

            var base64Secret = Convert.ToBase64String(bytes);
            var urlEncoded = base64Secret.TrimEnd('=').Replace('+', '-').Replace('/', '_');

            return urlEncoded;
        }
        
        public static Guid GetUserId(this IEnumerable<Claim> claims)
        {
            var strUserId = claims.FirstOrDefault(_ => _.Type == "userId")
                ?? throw new BrokenTokenException();

            return Guid.Parse(strUserId.Value);
        }
    }
}

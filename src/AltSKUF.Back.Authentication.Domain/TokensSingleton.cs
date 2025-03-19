using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AltSKUF.Back.Authentication.Domain
{
    public class TokensSingleton
    {
        public static TokensSingleton Singleton { get; set; } = new();

        public SymmetricSecurityKey? AccessTokenSecret { get; private set; }
        public SymmetricSecurityKey? PreviousAccessTokenSecret { get; private set; }

        public SymmetricSecurityKey? RefreshTokenSecret { get; private set; }
        public SymmetricSecurityKey? PreviousRefreshTokenSecret { get; private set; }

        public void UpdateAccessSecret(string secret)
        {
            var key = ToSymmetricSecurityKey(secret);
            if (PreviousAccessTokenSecret == null)
            {
                PreviousAccessTokenSecret = key;
                AccessTokenSecret = key;
            }
            else
            {
                PreviousAccessTokenSecret = AccessTokenSecret;
                AccessTokenSecret = key;
            }
        }
        
        public void UpdateRefreshSecret(string secret)
        {
            var key = ToSymmetricSecurityKey(secret);
            if (PreviousRefreshTokenSecret == null)
            {
                PreviousRefreshTokenSecret = key;
                RefreshTokenSecret = key;
            }
            else
            {
                PreviousRefreshTokenSecret = RefreshTokenSecret;
                RefreshTokenSecret = key;
            }
        }

        private static SymmetricSecurityKey ToSymmetricSecurityKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }
    }

}
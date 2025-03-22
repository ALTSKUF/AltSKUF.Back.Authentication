using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AltSKUF.Back.Authentication.Domain.Extensions
{
    public static class SecretExtensions
    {
        public static SymmetricSecurityKey? AccessTokenSecret { get; private set; }
        public static SymmetricSecurityKey? PreviousAccessTokenSecret { get; private set; }

        public static SymmetricSecurityKey? RefreshTokenSecret { get; private set; }
        public static SymmetricSecurityKey? PreviousRefreshTokenSecret { get; private set; }

        public static void UpdateAccessSecret(string secret)
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

        public static void UpdateRefreshSecret(string secret)
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

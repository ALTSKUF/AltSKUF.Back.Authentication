using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AltSKUF.Back.Authentication.Domain.Entety
{
    public class GenerateTokenModel
    {
        public DateTime Expires { get; set; }
        
        public List<Claim> Claims { get; set; } = [];

        public string? Secret { get; set; }
        public SymmetricSecurityKey? SymSecret { get; set; }

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}

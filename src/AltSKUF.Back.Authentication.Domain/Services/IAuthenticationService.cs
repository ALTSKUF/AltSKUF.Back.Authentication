using AltSKUF.Back.Authentication.Infrastracture.Entety.Responces;

namespace AltSKUF.Back.Authentication.Domain.Services
{
    public interface IAuthenticationService
    {
        public TokensResponce GetTokens(Guid userId);
        public string GetServiceToken();
    }
}

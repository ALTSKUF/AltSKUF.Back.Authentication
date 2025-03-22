using Refit;

namespace AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users.Controller
{
    public interface ISecretController
    {
        [Put("/Secret/Refresh")]
        public Task RefreshToken();
    }
}

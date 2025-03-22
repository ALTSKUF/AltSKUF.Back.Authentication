using AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users.Controller;
using Refit;

namespace AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users.Runtime
{
    public class UserService(HttpClient httpClient) : IUserService
    {
        public Uri? Uri => httpClient.BaseAddress;

        public HttpClient HttpClient => httpClient;

        public ISecretController SercretController { get; } = RestService.For<ISecretController>(httpClient);
    }
}

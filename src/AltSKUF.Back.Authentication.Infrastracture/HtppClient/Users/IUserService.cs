using AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users.Controller;

namespace AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users
{
    public interface IUserService
    {
        public Uri? Uri { get; }
        public System.Net.Http.HttpClient HttpClient { get; }

        public ISecretController SercretController { get; }
    }
}

using AltSKUF.Back.Authentication.Domain.Extensions;
using AltSKUF.Back.Authentication.Infrastracture.HtppClient.Users;
using Quartz;

namespace AltSKUF.Back.Authentication.Domain.Scheduler.Jobs
{
    public class AccessSecretJob(
        IUserService userService) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var newSecret = JwtExtensions.GenerateSecret();

            SecretExtensions.UpdateAccessSecret(newSecret);

            userService.HttpClient.DefaultRequestHeaders.Authorization =
                new("Bearer", JwtExtensions.GetServicesToken(
                    [
                        new("secret", newSecret )
                    ]));

            try
            {
                await userService.SercretController.RefreshToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
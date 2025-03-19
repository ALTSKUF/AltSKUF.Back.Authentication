using AltSKUF.Back.Authentication.Domain.Extensions;
using Quartz;

namespace AltSKUF.Back.Authentication.Domain.Scheduler.Jobs
{
    public class AccessSecretJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var newSecret = JwtExtensions.GenerateSecret();

            TokensSingleton.Singleton.UpdateAccessSecret(newSecret);

            Console.WriteLine("access");
            return Task.CompletedTask;
        }
    }
}
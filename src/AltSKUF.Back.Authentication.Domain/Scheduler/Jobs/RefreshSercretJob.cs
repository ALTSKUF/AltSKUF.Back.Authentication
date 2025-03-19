using AltSKUF.Back.Authentication.Domain.Extensions;
using Microsoft.Extensions.Caching.Memory;
using Quartz;

namespace AltSKUF.Back.Authentication.Domain.Scheduler.Jobs
{
    public class RefreshSercretJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var newSecret = JwtExtensions.GenerateSecret();

            TokensSingleton.Singleton.UpdateRefreshSecret(newSecret);

            Console.WriteLine("refresh");
            return Task.CompletedTask;
        }
    }
}

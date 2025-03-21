using AltSKUF.Back.Authentication.Domain.Extensions;
using Quartz;

namespace AltSKUF.Back.Authentication.Domain.Scheduler.Jobs
{
    public class RefreshSercretJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            var newSecret = JwtExtensions.GenerateSecret();

            TokensSingleton.Singleton.UpdateRefreshSecret(newSecret);

            return Task.CompletedTask;
        }
    }
}

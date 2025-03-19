using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using Quartz;
using AltSKUF.Back.Authentication.Domain.Scheduler.Jobs;

namespace AltSKUF.Back.Authentication.Domain.Scheduler
{
    public class JobFactory(
        IServiceScopeFactory serviceScopeFactory) 
        : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            if (job == null) throw new NullReferenceException($"Job from type: {bundle.JobDetail.JobType.Name} is null");
            return job;
        }

        public void ReturnJob(IJob job)
        {
            //Do something if need
        }
    }
}

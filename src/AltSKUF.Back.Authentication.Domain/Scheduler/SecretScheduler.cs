using Quartz.Impl;
using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Quartz.Spi;
using AltSKUF.Back.Authentication.Domain.Scheduler.Jobs;

namespace AltSKUF.Back.Authentication.Domain.Scheduler
{
    public class SecretScheduler
    {
        public static async Task Start(IServiceProvider serviceProvider)
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            var factory = serviceProvider.GetService<IJobFactory>();
            if (factory == null) throw new NullReferenceException("JobFactory in FG.Server.Notification is null");
            scheduler.JobFactory = factory;
            await scheduler.Start();

            IJobDetail accessJob = JobBuilder.Create<AccessSecretJob>().Build();
            ITrigger accessTrigger = TriggerBuilder.Create()  
                .WithIdentity("accessTrigger", "secrets")     
                .StartNow()                            
                .WithSimpleSchedule(_ => _
                    .WithIntervalInMinutes(
                        int.Parse(Configuration.Singleton.AccesExpirationTimeInMinutes) * 5)
                    .RepeatForever())                   
                .Build();                               

            IJobDetail refreshJob = JobBuilder.Create<RefreshSercretJob>().Build();
            ITrigger refreshTrigger = TriggerBuilder.Create()  
                .WithIdentity("refreshTrigger", "secrets")     
                .StartNow()                            
                .WithSimpleSchedule(_ => _            
                    .WithIntervalInMinutes(
                        int.Parse(Configuration.Singleton.RefreshExpirationTimeInMinutes))
                    .RepeatForever())                   
                .Build();                               

            await scheduler.ScheduleJob(accessJob, accessTrigger);        
            await scheduler.ScheduleJob(refreshJob, refreshTrigger);        
        }
    }
}

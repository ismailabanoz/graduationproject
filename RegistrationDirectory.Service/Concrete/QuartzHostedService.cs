using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using RegistrationDirectory.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class QuartzHostedService :IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IJobFactory _jobFactory;
        private readonly IEnumerable<MyJobWeekly> _myWeeklyJobs;
        private readonly IEnumerable<MyJobMonthly> _myMonthlyJobs;

        public QuartzHostedService(ISchedulerFactory schedulerFactory, IEnumerable<MyJobWeekly> myJobs, IJobFactory jobFactory, IEnumerable<MyJobMonthly> myMonthlyJobs)
        {
            _schedulerFactory = schedulerFactory;
            _myWeeklyJobs = myJobs;
            _jobFactory = jobFactory;
            _myMonthlyJobs = myMonthlyJobs;
        }
        public IScheduler Scheduler { get; set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Common.Logs($"StartAsync at " + DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), "StartAsync " + DateTime.UtcNow.ToString("hhmmss"));

            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;
            foreach (var myJob in _myWeeklyJobs)
            {
                var job = CreateJobWeekly(myJob);
                var trigger = CreateTriggerWeekly(myJob);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
            foreach (var myJob in _myMonthlyJobs)
            {
                var job = CreateJobMonthly(myJob);
                var trigger = CreateTriggerMontly(myJob);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }
            await Scheduler.Start(cancellationToken);

        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Common.Logs($"StopAsync at " + DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), "StopAsync " + DateTime.UtcNow.ToString("hhmmss"));
            await Scheduler.Shutdown(cancellationToken);
        }

        private static IJobDetail CreateJobWeekly(MyJobWeekly myJob)
        {
            var type = myJob.Type;
            return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
        }
        private static IJobDetail CreateJobMonthly(MyJobMonthly myJob)
        {
            var type = myJob.Type;
            return JobBuilder.Create(type).WithIdentity(type.FullName).WithDescription(type.Name).Build();
        }

        private static ITrigger CreateTriggerWeekly(MyJobWeekly myJob)
        {
            return TriggerBuilder.Create().WithIdentity($"{myJob.Type.FullName}.trigger").WithCronSchedule(myJob.Expression).WithDescription(myJob.Expression).Build();
        }
        private static ITrigger CreateTriggerMontly(MyJobMonthly myJob)
        {
            return TriggerBuilder.Create().WithIdentity($"{myJob.Type.FullName}.trigger").WithCronSchedule(myJob.Expression).WithDescription(myJob.Expression).Build();
        }
    }
}

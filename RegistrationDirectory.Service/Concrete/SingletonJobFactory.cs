using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class SingletonJobFactory :IJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IJob NewJob(TriggerFiredBundle triggerFiredBundle, IScheduler scheduler)
        {
            

            
            

            Common.Logs($"NewJob at" + DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), "NewJob" + DateTime.UtcNow.ToString("hhmmss"));
            return _serviceProvider.GetRequiredService(triggerFiredBundle.JobDetail.JobType) as IJob; 
        }
        public void ReturnJob(IJob job)
        {

        }
    }
}

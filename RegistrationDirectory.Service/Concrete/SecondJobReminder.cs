using Microsoft.Extensions.DependencyInjection;
using Quartz;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class SecondJobReminder:IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public SecondJobReminder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SecondJobReminder()
        {

        }
        public Task Execute(IJobExecutionContext jobExecutionContext)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IReportService>();
                context.CreateExcelForMonthlyReport();
            }
            Common.Logs($"SecondJobReminders at " + DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), "SecondJobReminders " + DateTime.UtcNow.ToString("hhmmss"));
            return Task.CompletedTask;
        }
    }
}

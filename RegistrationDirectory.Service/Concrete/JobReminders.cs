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
    public class JobReminders :IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public JobReminders(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public JobReminders()
        {

        }
        public Task Execute(IJobExecutionContext jobExecutionContext)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IReportService>();
                context.CreateExcelForWeeklyReport();
            }
            Common.Logs($"JobReminders at " + DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), "JobReminders " + DateTime.UtcNow.ToString("hhmmss"));
            return Task.CompletedTask;
        }
    }
}

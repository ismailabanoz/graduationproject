using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Absract
{
    public interface IReportService
    {
        void CreateExcelForWeeklyReport();
        void CreateExcelForMonthlyReport();
        void SenMail(string path, string period);
    }
}

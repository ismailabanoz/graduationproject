using ClosedXML.Excel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class ReportManager :IReportService
    {
        private readonly ICommercialActivityService _commercialActivityService;
        private readonly ICustomerService _customerService;
        private readonly AppDbContext _appDbContext;

        public ReportManager(ICommercialActivityService commercialActivityService, ICustomerService customerService, AppDbContext appDbContext)
        {
            _commercialActivityService = commercialActivityService;
            _customerService = customerService;
            _appDbContext = appDbContext;
        }
        public void CreateExcelForMonthlyReport()
        {
            var table = GetMonthlyReport();
            using var ms = new MemoryStream();
            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(table);
            var rnd = Guid.NewGuid().ToString();
            string now = DateTime.Today.ToShortDateString().ToString();
            string path = @$"..\RegistrationDirectory.API\Excel\Monthly Report_{now}.xlsx";
            string filePath = string.Format(path);
            wb.Worksheets.Add(ds);
            wb.SaveAs(filePath);
            SenMail(path,"Monthly Report");
        }

        public void SenMail(string path,string period)
        {
            var email = new MimeMessage();
            var users = _appDbContext.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToList();
            users.ForEach(x =>
            {
                email.To.Add(MailboxAddress.Parse(x.Email));

            });
            email.From.Add(MailboxAddress.Parse("ismailabanoz1213@gmail.com"));
            email.Subject = period;
            email.Body = new TextPart(TextFormat.Html) { Text = "<h1>Example HTML Message Body</h1>" };
            var builder = new BodyBuilder();
            builder.TextBody = @$"The {period} is attached.";
            builder.Attachments.Add(path);
            email.Body = builder.ToMessageBody();
            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ismailabanoz1213@gmail.com", "The application password created from google will come here"); // The application password created from google will come here
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void CreateExcelForWeeklyReport()
        {
            var table=GetWeeklyReport();
            using var ms=new MemoryStream();
            var wb = new XLWorkbook();
            var ds = new DataSet();
            ds.Tables.Add(table);
            var rnd = Guid.NewGuid().ToString();
            string now= DateTime.Today.ToShortDateString().ToString();
            string path = @$"..\RegistrationDirectory.API\Excel\Weekly Report_{now}.xlsx";
            string filePath = string.Format(path);
            wb.Worksheets.Add(ds);
            wb.SaveAs(filePath);
            SenMail(path,"Weekly Report");
        }
        public DataTable GetMonthlyReport()
        {
            var commercialActivities = _commercialActivityService.GetAll();
            var customer = _customerService.GetAll();

            var result = from cu in customer
                         join ca in commercialActivities on cu.Id equals ca.CustomerId
                         group cu by cu.City  into pg
                         select new MonthlyReportModel
                         {
                             City = pg.FirstOrDefault().City,
                             NumberOfCustomers = pg.Count()
                         };

            DataTable table = new DataTable { TableName = "Report_" + DateTime.Today.ToShortDateString().ToString() };
            table.Columns.Add("City", typeof(string));
            table.Columns.Add("Number Of Customer", typeof(int));

            result.ToList().ForEach(x =>
            {
                table.Rows.Add(x.City,x.NumberOfCustomers);
            });
            return table;
        }
        public DataTable GetWeeklyReport()
        {


            var commercialActivities = _commercialActivityService.GetAll();

            var count = from t in commercialActivities
                        group t by t.CustomerId into g
                        select new { CustomerId = g.Key, CustomerCount = g.Count() };

            List<WeeklyReportModel> weeklyReportModels = new List<WeeklyReportModel>();

            count.ToList().ForEach(x =>
            {
                var customer = _customerService.GetById(x.CustomerId);
                var totalPrice = _commercialActivityService.GetAll().Where(p => p.CustomerId == customer.Id).Sum(x => x.Price);
                weeklyReportModels.Add(new WeeklyReportModel
                {
                    TotalPrice = totalPrice,
                    TotalNumberOfCommercialActivities = x.CustomerCount,
                    CustomerName = customer.Name,
                    CustomerSurname = customer.Surname,
                    Phone = customer.Phone
                });
            });

            DataTable table = new DataTable { TableName = "Report_" + DateTime.Today.ToShortDateString().ToString() };
            table.Columns.Add("Customer Name", typeof(string));
            table.Columns.Add("Customer Surname", typeof(string));
            table.Columns.Add("Phone", typeof(string));
            table.Columns.Add("Total Number Of Commercial Activities", typeof(int));
            table.Columns.Add("Total Price", typeof(decimal));

            var list = (from t in weeklyReportModels
                        orderby t.TotalNumberOfCommercialActivities descending
                        select t).Take(5);
            list.ToList().ForEach(x =>
            {
                table.Rows.Add(x.CustomerName, x.CustomerSurname, x.Phone, x.TotalNumberOfCommercialActivities, x.TotalPrice);
            });
            return table;
        }
    }
}

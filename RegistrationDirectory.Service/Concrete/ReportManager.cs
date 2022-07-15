using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
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

        public ReportManager(ICommercialActivityService commercialActivityService, ICustomerService customerService)
        {
            _commercialActivityService = commercialActivityService;
            _customerService = customerService;
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
            string filePath = string.Format(@$"..\RegistrationDirectory.API\Excel\Monthly Report_{now}.xlsx");
            wb.Worksheets.Add(ds);
            wb.SaveAs(filePath);

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
            string filePath = string.Format(@$"..\RegistrationDirectory.API\Excel\Weekly Report_{now}.xlsx");
            wb.Worksheets.Add(ds);
            wb.SaveAs(filePath);



            //List<Customer> customer4 = _customerService.GetAll();
           // List<CommercialActivity> customerActivity = _commercialActivityService.GetAll();


            /*var q = (from pd in customer4
                     join od in customerActivity on pd.Id equals od.CustomerId
                     orderby od.CustomerId descending
                     select new
                     {
                         pd.Id,
                         pd.Name,
                         pd.Surname,
                         pd.Phone,
                         od.Price
                     }).ToList();*/


           /* var at=from ca in customerActivity
                   join c in customer4 on ca.Id equals c.Id into g
                   orderby g.Count() descending, ca.CustomerId
                   select new
                   {
                       Id=ca.CustomerId,
                       Name=ca.
                   }
           */


                   





            /*weeklyReportModels2 =
                 weeklyReportModels2.GroupBy(row => row.TotalNumberOfCommercialActivities)
                     .SelectMany(g => g.OrderBy(row => row.TotalNumberOfCommercialActivities).Take(5))
                     .ToList();*/

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
            /*var weeklyReportModels =  _commercialActivityService.GetAll();
            List<WeeklyReportModel> weeklyReportModels2= new List<WeeklyReportModel>();
            var count =  from c in weeklyReportModels
                        group c by c.CustomerId into g
                        select new { CustomerId = g.Key, CustomerCount = g.Count() };

            DataTable table = new DataTable { TableName = "testt" };

            table.Columns.Add("CustomerId", typeof(int));
            table.Columns.Add("TotalNumberOfCommercialActivities", typeof(int));

            weeklyReportModels2.ForEach(x =>
            {
                table.Rows.Add(x.TotalPrice, x.TotalNumberOfCommercialActivities);
            });
            return table;*/




            //foreach (var item in weeklyReportModels.GroupBy(x => x.CustomerId).Select(group => new {weeklyReportModels2.Capacity=group.Key,weeklyReportModels2.Count= group.Count() }) ;
            // select COUNT(CustomerId),CustomerId from CommercialActivities group by CustomerId having COUNT(CustomerId) > 1

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Models
{
    public class WeeklyReportModel
    {
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Phone { get; set; }
        public int TotalNumberOfCommercialActivities { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

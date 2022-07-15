using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Models
{
    public class MyJobWeekly
    {
        public MyJobWeekly(Type type, string expression)
        {
            Common.Logs($"MyJob at " + DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss"), "MyJob " + DateTime.UtcNow.ToString("hhmmss"));
            Type = type;
            Expression = expression;
        }
        public Type Type { get; set; }
        public string Expression { get; set; }
    }
}

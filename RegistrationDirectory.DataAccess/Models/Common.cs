using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Models
{
    public static class Common
    {
        public static void Logs(string message, string fileName)
        {
            string filePath = string.Format(@$"..\RegistrationDirectory.API\Logs");
            filePath = Path.Combine(filePath, fileName);
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            using TextWriter textWriter = new StreamWriter(fileStream);
            textWriter.Write(message);

        }
    }
}

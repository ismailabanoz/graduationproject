using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistirationDirectory.SharedLibrary
{
    public class CreatePictureWithWatermarkMessage
    {
        public int CustomerId { get; set; }
        public string ImageName { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistirationDirectory.SharedLibrary
{
    public class CreatePictureMessage
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public byte[] BytePhoto { get; set; }
    }
}

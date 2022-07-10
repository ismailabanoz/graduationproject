using RegistrationDirectory.DataAccess.Absract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Models
{
    public class CommercialActivity : IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Service { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
    }
}

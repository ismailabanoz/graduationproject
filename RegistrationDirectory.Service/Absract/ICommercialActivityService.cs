using RegistrationDirectory.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Absract
{
    public interface ICommercialActivityService
    {
        List<CommercialActivity> GetAll();
        void Delete(int commercialActivityId);
        void Update(CommercialActivity commercialActivity);
        void Create(CommercialActivity commercialActivity);
        CommercialActivity GetById(int commercialActivityId);
    }
}

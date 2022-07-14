using RegistrationDirectory.DataAccess.Absract;
using RegistrationDirectory.DataAccess.Concrete;
using RegistrationDirectory.DataAccess.Models;
using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.Service.Concrete
{
    public class CommercialActivityManager : ICommercialActivityService
    {
        private readonly IRepository<CommercialActivity> _commercialActivity;

        private readonly IUnitOfWork _unitOfWork;

        public CommercialActivityManager( IUnitOfWork unitOfWork, IRepository<CommercialActivity> commercialActivity)
        {
            _unitOfWork = unitOfWork;
            _commercialActivity= commercialActivity;
        }

        public void Create(CommercialActivity commercialActivity)
        {
            _commercialActivity.Add(commercialActivity);
            _unitOfWork.Commit();
        }

        public void Delete(int commercialActivityId)
        {
            _commercialActivity.Delete(commercialActivityId);
            _unitOfWork.Commit();
        }

        public List<CommercialActivity> GetAll()
        {
            return _commercialActivity.GetAll();
        }

        public CommercialActivity GetById(int commercialActivityId)
        {
            return _commercialActivity.GetById(commercialActivityId);
        }

        public void Update(CommercialActivity commercialActivity)
        {
            _commercialActivity.Update(commercialActivity);
            _unitOfWork.Commit();
        }
    }
}

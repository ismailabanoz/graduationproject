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
        private AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public CommercialActivityManager(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public void Create(CommercialActivity commercialActivity)
        {
            _context.Add(commercialActivity);
            _unitOfWork.Commit();
        }

        public void Delete(int commercialActivityId)
        {
            _context.Remove(GetById(commercialActivityId));
            _unitOfWork.Commit();
        }

        public List<CommercialActivity> GetAll()
        {
            return _context.CommercialActivities.ToList();
        }

        public CommercialActivity GetById(int commercialActivityId)
        {
            return _context.Set<CommercialActivity>().SingleOrDefault(p => p.Id == commercialActivityId);
        }

        public void Update(CommercialActivity commercialActivity)
        {
            _context.Update(commercialActivity);
            _unitOfWork.Commit();
        }
    }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegistrationDirectory.DataAccess.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationDirectory.DataAccess.Concrete
{
    public class GenericRepository<T> : IRepository<T>
        where T : class
    {
        private readonly DbSet<T> _dbSet;
        private readonly AppDbContext _appDbContext;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Add(T entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Added;
            _appDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _dbSet.Remove(GetById(id));
        }
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }



        public List<T> GetAll()
        {


            return _dbSet.ToList();


        }


        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}

using BLL.Interfaces;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private protected readonly HospitalDbContext _context;

        public GenericRepository(HospitalDbContext dbContext) => _context = dbContext;

        public void Add(T entity) => _context.Set<T>().Add(entity);

        public void Delete(T entity) => _context.Remove(entity);

        public T? Get(int id) => _context.Set<T>().Find(id);

        public IEnumerable<T> GetAll() => _context.Set<T>().AsNoTracking();

        public void Update(T entity) => _context.Set<T>().Update(entity);  

        public int Save() => _context.SaveChanges();
    }
}

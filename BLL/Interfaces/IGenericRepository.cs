using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    //Save changes 
    public interface IGenericRepository<T> where T : class
    {
        T? Get(int id);
        void Update(T entity);
        void Delete(T entity);
        void Add(T entity);
        IEnumerable<T> GetAll();
        int Save(); 
    }
}

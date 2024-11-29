using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Department? GetDepartmentWithHead(int id)
            => _context.Departments.Include(d => d.Head).FirstOrDefault(d => d.Id == id);

        public Department? GetDepartmentWithDoctors(int id)
            => _context.Departments.Include(d => d.Doctors).FirstOrDefault(d => d.Id == id);
    }
}

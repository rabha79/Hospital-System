using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class AdminRepository : GenericRepository<Admin> , IAdminRepository
    {
        public AdminRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Admin? GetByAspUsersId(string id)
        {
            return _context.Admins.FirstOrDefault(a => a.AspNetUsersId == id);
        }
    }
}

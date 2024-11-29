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
    public class NurseRepository : GenericRepository<Nurse>, INurseRepository
    {
        public NurseRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Nurse? GetNurseWithPatients(int nurseId)
           => _context.Nurses.Include( n => n.NursePatients ).FirstOrDefault(n => n.Id == nurseId);
    }
}

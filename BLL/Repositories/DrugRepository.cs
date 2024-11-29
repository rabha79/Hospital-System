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
    public class DrugRepository : GenericRepository<Drug>, IDrugRepository
    {
        public DrugRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Drug? GetDrugWithMedicalRecord(int drugId)
            => _context.Drugs.Include(d => d.MedicalRecord).FirstOrDefault(d => d.DrugId == drugId); 
    }
}

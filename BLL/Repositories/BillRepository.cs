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
    public class BillRepository : GenericRepository<Bill>, IBillRepository
    {
        public BillRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Bill? GetBillWithPatient(int billId)
            => _context.Bills.Include(b => b.Patient).FirstOrDefault(b => b.Id == billId);

        public IEnumerable<Bill>? GetBillsForPatient(int patientId)
            => _context.Bills.Where(b => b.PatientId == patientId); 

    }
}

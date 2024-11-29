using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Repositories
{
    public class NursePatientRepository : GenericRepository<NursePatient>, INursePatientRepository
    {
        public NursePatientRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public new NursePatient? Get(int id) 
            => _context.NursesPatients.FirstOrDefault(p => p.Id == id);

        public NursePatient? GetNursePatientWithNurse(int id)
            => _context.NursesPatients.Include(NP => NP.Nurse).FirstOrDefault(NP => NP.Id == id);

        public NursePatient? GetNursePatientWithNurseAndPatient(int id)
            => _context.NursesPatients.Include(NP =>  NP.Patient).Include(NP => NP.Nurse).FirstOrDefault(NP => NP.Id == id);


        public NursePatient? GetNursePatientWithPatient(int id)
            => _context.NursesPatients.Include(NP => NP.Patient).FirstOrDefault(NP => NP.Id == id);

        public IEnumerable<NursePatient>? GetUpcomingCares()
            => _context.NursesPatients.Where(np => np.CareDate >= DateOnly.FromDayNumber(0)); 

    }
}

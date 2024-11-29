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
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        public PatientRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Patient? GetPatientWithAppointments(int patientId)
        {
            return _context.Patients.Where(p => p.Id == patientId).Include(p => p.Appointments).FirstOrDefault();
        }

        public Patient? GetPatientWithMedicalRecords(int patientId)
        {
            return _context.Patients.Where(p => p.Id == patientId).Include(p => p.MedicalRecords).FirstOrDefault();
        }

        public Patient? GetPatientWithNurses(int patientId)
        {
            return _context.Patients.Where(p => p.Id == patientId).Include(p => p.NursePatients).FirstOrDefault();
        }

        public Patient? GetByAspUsersId(string id)
        { 
            return _context.Patients.FirstOrDefault(a => a.AspNetUsersId == id);
        }

       
    }
}

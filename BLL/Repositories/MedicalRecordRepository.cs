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
    public class MedicalRecordRepository : GenericRepository<MedicalRecord> , IMedicalRecordRepository
    {
        public MedicalRecordRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<MedicalRecord>? GetPatientRecords(int patientId)
            => _context.MedicalRecords.Where(record => record.PatientId == patientId).AsNoTracking();

        public MedicalRecord? GetRecordWithDrugs(int recordId)
            => _context.MedicalRecords.Include(r => r.Drugs).FirstOrDefault(r => r.Id == recordId);

        public MedicalRecord? GetRecordWithPatient(int recordId)
            => _context.MedicalRecords.Include(r => r.Patient).FirstOrDefault(r => r.Id == recordId);

    }
}

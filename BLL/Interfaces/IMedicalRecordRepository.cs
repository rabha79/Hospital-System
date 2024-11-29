using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IMedicalRecordRepository : IGenericRepository<MedicalRecord>
    {
        MedicalRecord? GetRecordWithPatient(int recordId);
        MedicalRecord? GetRecordWithDrugs(int recordId); 
        IEnumerable<MedicalRecord>? GetPatientRecords(int patientId);
    }
}

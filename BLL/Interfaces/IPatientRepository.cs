using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Patient? GetPatientWithMedicalRecords(int patientId);
        Patient? GetPatientWithAppointments(int patientId);
        Patient? GetPatientWithNurses(int patientId);

        Patient? GetByAspUsersId(string id);
    }
}

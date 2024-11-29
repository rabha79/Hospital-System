using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INursePatientRepository : IGenericRepository<NursePatient>
    {
        NursePatient? GetNursePatientWithNurse(int id);
        NursePatient? GetNursePatientWithPatient(int id);
        NursePatient? GetNursePatientWithNurseAndPatient(int id);
        IEnumerable<NursePatient>? GetUpcomingCares();
    }
}

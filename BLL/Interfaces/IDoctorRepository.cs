using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BLL.Interfaces
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        Doctor? GetDoctorWithAppointments(int doctorId);

        Doctor? GetDoctorWithDepartment(int doctorId);

        IEnumerable<Doctor> GetDoctorsInDept(int departmentId);

        IEnumerable<Doctor> GetDoctorsInDept(int departmentId, Shift shift);
         Doctor? GetByAspUsersId(string id);
    }
}

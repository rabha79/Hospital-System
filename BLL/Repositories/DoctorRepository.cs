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
    public class DoctorRepository : GenericRepository<Doctor>,  IDoctorRepository
    {
        public DoctorRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public Doctor? GetDoctorWithAppointments(int doctorId)
        {
             return _context.Doctors.Where(d => d.Id == doctorId).Include(d => d.Appointments).FirstOrDefault();
        }

        public Doctor? GetDoctorWithDepartment(int doctorId)
        {
            return _context.Doctors.Where(d => d.Id == doctorId).Include(d => d.Department).FirstOrDefault();
        }

        public IEnumerable<Doctor> GetDoctorsInDept(int departmentId, Shift shift)
            => _context.Doctors.Where(d => d.DepartmentId == departmentId && d.Shift == shift);         
        
        public IEnumerable<Doctor> GetDoctorsInDept(int departmentId)
            => _context.Doctors.Where(d => d.DepartmentId == departmentId);

        public Doctor? GetByAspUsersId(string id) 
        {
            return _context.Doctors.Where(d => d.AspNetUsersId == id).FirstOrDefault();
        }
    }
}

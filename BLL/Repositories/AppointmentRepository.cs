using Azure.Core;
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
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(HospitalDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<Appointment>? GetAppointments(int doctorId, int patientId)
            => _context.Appointments.Where(a => a.DoctorId == doctorId && a.PatientId == patientId);  


        public IEnumerable<Appointment>? GetAllAppointmentsForDoctor(int doctorId)
            => _context.Appointments.Where(a => a.DoctorId == doctorId); 

        public IEnumerable<Appointment>? GetAppointmentsForPatient(int patientId)
            => _context.Appointments.Where(a => a.PatientId == patientId);

        public IEnumerable<Appointment>? GetDailyAppointmentsForDoctor(int doctorId)
            => _context.Appointments.Where(a => a.DoctorId == doctorId && a.Date >= DateTime.Today); 
        
        public Appointment? GetAppointmentWithPatient(int appointmentId)
            => _context.Appointments.Include(a => a.Patient).Where(a => a.Id == appointmentId).FirstOrDefault();

        public IEnumerable<Appointment>? GetUpcomingAppointments()
            => _context.Appointments.Where(a => a.Date >= DateTime.Today);

        public IEnumerable<Appointment> GetAppointmentsByDate(DateTime date)
           => _context.Appointments.Where(a => a.Date.HasValue && a.Date.Value.Date == date.Date);

        public Appointment? GetAppointmentWithDoctor(int appointmentId)
            => _context.Appointments.Include(a => a.Doctor).FirstOrDefault(a => a.Id == appointmentId);
    }
}

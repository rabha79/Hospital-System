using DAL.Models;

namespace PL.ViewModels
{
    public class DoctorScheduleViewModel
    {
        public Doctor Doctor { get; set; }
        public IEnumerable<Appointment> Appointments { get; set; }
        public int TotalAppointments { get; set; }
        public int FinishedAppointments { get; set; }
        public int PendingAppointments { get; set; }
    }
}

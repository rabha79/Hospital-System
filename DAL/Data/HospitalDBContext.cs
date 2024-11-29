using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class HospitalDbContext : IdentityDbContext <AppUser>
    {
        public HospitalDbContext(DbContextOptions options) : base(options)
        {
            
        }
        public HospitalDbContext(): base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Appointment>()
	        .Property(a => a.Id)
	        .ValueGeneratedOnAdd();
		}

		public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<NursePatient> NursesPatients { get;set; }
    }
}

using BLL.Interfaces;
using BLL.Repositories;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


			#region register user-defined services
			// Add services to the container.
			builder.Services.AddControllersWithViews();

            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<HospitalDbContext>(option => option.UseSqlServer(connection));

            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>(); 
            builder.Services.AddScoped<IPatientRepository, PatientRepository>(); 
            builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>(); 
            builder.Services.AddScoped<INurseRepository, NurseRepository>(); 
            builder.Services.AddScoped<INursePatientRepository, NursePatientRepository>(); 
            builder.Services.AddScoped<IBillRepository, BillRepository>(); 
            builder.Services.AddScoped<IDrugRepository, DrugRepository>(); 
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); 
            builder.Services.AddScoped<IAdminRepository, AdminRepository>(); 
            builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();

            #endregion

            #region register identity services

            builder.Services.AddIdentity<AppUser,IdentityRole>(options => 
            {
                options.Password.RequiredLength = 6; 
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false; 
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<HospitalDbContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication("Cookies")
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";  
                options.AccessDeniedPath = "/Account/AccessDenied"; 
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(12);  // Set expiration for the cookie
                options.SlidingExpiration = true;  
            });

            #endregion 


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

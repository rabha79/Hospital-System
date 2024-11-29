using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace PL.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Patient"))
                    return RedirectToAction("Index", "Patient");

                if (User.IsInRole("Doctor"))
                    return RedirectToAction("Index", "Doctor");

                if (User.IsInRole("Admin"))
                    return RedirectToAction("Dashboard", "Admin");

                return View("Dashboard"); 
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

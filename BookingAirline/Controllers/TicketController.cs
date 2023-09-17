using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingAirline.Models;

namespace BookingAirline.Controllers
{
    public class TicketController : Controller
    {
        BookingAirLightEntities database = new BookingAirLightEntities();
        // GET: Ticket
        public ActionResult Index()
        {
            return View(database.Ves.ToList());
        }

        //  CREATE TICKET
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Ve ve)
        {
            try
            {
                database.Ves.Add(ve);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Error Create New");
            }
        }

        //DETAIL TICKET
        public ActionResult Details(string maVe)
        {
            return View(database.Ves.Where(s => s.MaVe.Contains(maVe)).ToList());
        }


        //EDIT TICKET
        public ActionResult Edit(string maVe)
        {
            return View(database.Ves.Where(s => s.MaVe.Contains(maVe)).ToList());
        }

        [HttpPost]
        public ActionResult Edit(string maVe, Ve ve)
        {
            database.Entry(ve).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        //DELETE TICKET
        public ActionResult Delete(string maVe)
        {
            return View(database.Ves.Where(s => s.MaVe.Contains(maVe)).ToList());
        }
    }
}

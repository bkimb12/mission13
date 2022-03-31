using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using mission13.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace mission13.Controllers
{
    //Home Controller
    public class HomeController : Controller
    {
        //Using Context
        private BowlersDbContext context { get; set; }
        public HomeController(BowlersDbContext temp)
        {
            context = temp;
        }

        public IActionResult Index(int id = 0)
        {
            //Return all Bowlers
            if (id == 0)
            {
                var bowlers = context.Bowlers.OrderBy(x => x.BowlerLastName).ToList();
                ViewBag.Teams = context.Teams.ToList();
                ViewBag.CurrentTeamID = id;
                return View(bowlers);
            }
            //Return Bowlers for X team
            else
            {
                var bowlers = context.Bowlers.Include(x => x.Team).Where(x => x.TeamID == id).ToList();
                ViewBag.Teams = context.Teams.ToList();
                ViewBag.CurrentTeamID = id;
                return View(bowlers);
            }

        }



        [HttpGet]
        public IActionResult DeleteBowler()
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);
            var bowler = context.Bowlers.Single(x => x.BowlerID == id);

            return View(bowler);
        }

        [HttpPost]
        public IActionResult DeleteBowler(Bowler b)
        {
            context.Remove(b);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit()
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);

            var blah = context.Bowlers.Single(x => x.BowlerID == id);

            ViewBag.Teams = context.Teams.ToList();

            return View("AddBowler", blah);
        }

        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            context.Update(b);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Teams = context.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                context.Add(b);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Teams = context.Teams.ToList();

                return View(b);
            }
        }
    }
}

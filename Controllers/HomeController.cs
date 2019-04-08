using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dojodachi.Models;
using Microsoft.AspNetCore.Http;

namespace Dojodachi.Controllers
{
    public class HomeController : Controller
    {
        int fullness = 20;
        int happiness = 20;
        int energy = 50;
        int meal = 3;

        Random rand = new Random();

        [HttpPost("Restart")]
        public IActionResult Restart()
        {
            HttpContext.Session.Remove("fullness");
            HttpContext.Session.Remove("happiness");
            HttpContext.Session.Remove("energy");
            HttpContext.Session.Remove("meal");

            return RedirectToAction("Index");
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("fullness", fullness);
            HttpContext.Session.SetInt32("happiness", happiness);
            HttpContext.Session.SetInt32("energy", energy);
            HttpContext.Session.SetInt32("meal", meal);

            ViewBag.fullness = HttpContext.Session.GetInt32("fullness");
            ViewBag.happiness = HttpContext.Session.GetInt32("happiness");
            ViewBag.energy = HttpContext.Session.GetInt32("energy");
            ViewBag.meal = HttpContext.Session.GetInt32("meal");

            ViewBag.message = "";
            ViewBag.gameEnd = false;
            return View();
        }

        [HttpGet("Result")]
        public IActionResult Result(string message)
        {
            fullness = (int)HttpContext.Session.GetInt32("fullness");
            ViewBag.fullness = fullness;

            happiness = (int)HttpContext.Session.GetInt32("happiness");
            ViewBag.happiness = happiness;

            energy = (int)HttpContext.Session.GetInt32("energy");
            ViewBag.energy = energy;

            meal = (int)HttpContext.Session.GetInt32("meal");
            ViewBag.meal = meal;

            if(fullness >= 100 && happiness >= 100 && energy >= 100)
            {
                ViewBag.message = "Congratulations! You won!";
                ViewBag.gameEnd = true;                
            }
            else if(fullness <= 0 || happiness <= 0)
            {
                ViewBag.message = "Your Dojodachi has passed away...";
                ViewBag.gameEnd = true;                
            }
            else
            {
                ViewBag.message = message;
                ViewBag.gameEnd = false;
            }
            
            return View("Index");
        }

        [HttpPost("FeedPet")]
        public IActionResult FeedPet()
        {
            int val = rand.Next(0, 4);
            string message = "You played with your Dojodachi!";

            meal = (int)HttpContext.Session.GetInt32("meal")-1;

            if(meal > 0)
            {
                HttpContext.Session.SetInt32("meal", meal);
                message += " Meal -1";
            }
            if(val != 1)
            {
                int addition = rand.Next(5, 10);
                fullness = (int)HttpContext.Session.GetInt32("fullness") + addition;
                HttpContext.Session.SetInt32("fullness", fullness);

                message += " Fullness +" + addition;               
            }

            return RedirectToAction("Result", new { Message = message });
        }

        [HttpPost("PlayPet")]
        public IActionResult PlayPet()
        {
            string message = "You played with your Dojodachi!";

            energy = (int)HttpContext.Session.GetInt32("energy") - 5;

            if(energy >= 5)
            {
                HttpContext.Session.SetInt32("energy", energy);

                int addition = rand.Next(5,10);                
                happiness = (int)HttpContext.Session.GetInt32("happiness") + addition;
                HttpContext.Session.SetInt32("happiness", happiness);

                message += " Happiness +" + addition + ", Energy -5";
            }

            return RedirectToAction("Result", new { Message = message });
        }

        [HttpPost("WorkPet")]
        public IActionResult WorkPet()
        {
            string message = "You played with your Dojodachi!";

            energy = (int)HttpContext.Session.GetInt32("energy") - 5;

            if(energy >= 5)
            {
                HttpContext.Session.SetInt32("energy", energy);

                int addition = rand.Next(1, 4);
                meal = (int)HttpContext.Session.GetInt32("meal") + addition;
                HttpContext.Session.SetInt32("meal", meal);

                message += " Energy -5, Meal +" + addition;
            }

            return RedirectToAction("Result", new { Message = message });
        }

        [HttpPost("SleepPet")]
        public IActionResult SleepPet()
        {
            string message = "You played with your Dojodachi!";

            energy = (int)HttpContext.Session.GetInt32("energy") + 15;
            HttpContext.Session.SetInt32("energy", energy);

            fullness = (int)HttpContext.Session.GetInt32("fullness") - 5;
            HttpContext.Session.SetInt32("fullness", fullness);

            happiness = (int)HttpContext.Session.GetInt32("happiness") - 5;
            HttpContext.Session.SetInt32("happiness", happiness);

            return RedirectToAction("Result", new { Message = message });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WordSearchMVC5.Models;

namespace WordSearchMVC5.Controllers
{
    public class HomeController : Controller
    {
        private List<WordGrid> db = new List<WordGrid>();
        public ActionResult Index()
        {
            Console.WriteLine("In index.");
            return View();
        }
        [HttpPost]
        public PartialViewResult WordSearch(WordGrid item)
        {

            db.Add(item);
            return PartialView("_WordSearch",db);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
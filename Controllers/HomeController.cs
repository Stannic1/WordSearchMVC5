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
        private WordGrid db = new WordGrid();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public PartialViewResult WordSearch(WordUserInput item)
        {

            db.InitWordGrid(item);
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ActionResult WordSearch(int userNumbers, string userWordList)
        {
            // TODO: Place these into another function or helper folder to keep the controller slim.
            Guid randkey = Guid.NewGuid();
            string sessionKey = Convert.ToBase64String(randkey.ToByteArray());
            sessionKey = sessionKey.Replace("=", "");
            sessionKey = sessionKey.Replace("+", "");
            if (db.InitWordGrid(userNumbers, userWordList, sessionKey))
            {
                Session[sessionKey] = db;
                return PartialView("_WordSearch", db);
            } else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult WordSearchReroll(string SessionKey) {
            db = Session[SessionKey] as WordGrid;
            db.ReInitWordGrid();
            Session[SessionKey] = db;
            return PartialView("_WordSearch", db);
        }
        [HttpPost]
        public ActionResult WordSearchUserFind(string input, string key)
        {
            db = Session[key] as WordGrid;
            return db.ValidWord(input) ? Json(new { success = true }) : Json(new { success = false });
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
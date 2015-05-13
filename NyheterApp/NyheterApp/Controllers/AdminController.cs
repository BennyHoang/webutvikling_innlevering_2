using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NyheterApp.Controllers
{
    public class AdminController : Controller
    {

        //LAgnyhet siden

        public ActionResult LagNyNyhet()
        {

            return View();
        }


        //Vis nyhet
        public ActionResult VisAlleNyheter()
        {
            return View();
        }


        //Vis en nyhet

        public ActionResult VisEnNyhet() 
        {
            return View();
        }
        //Rediger nyehter
        public ActionResult RedigerNyheter()
        {

            return View();
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
	}
}
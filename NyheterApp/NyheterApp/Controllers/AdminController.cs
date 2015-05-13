using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NyheterApp.Models;

namespace NyheterApp.Controllers
{
    public class AdminController : Controller
    {

        //LAgnyhet siden

        public ActionResult LagNyNyhet()
        {

            return View();
        }



        //NB DENNE FUNKER IKKE!!!!!!!!
        //Vis nyheter   
        public ActionResult VisAlleNyheter()
        {
            //LINQ
            //1. koble til ORM (DB)
            using (DataAuthorOrmDataContext DataAuthor = new DataAuthorOrmDataContext())
            {

                //2. LINQ-spørringen
                List<News> newsListe = (from News in DataAuthor.Authors
                                            select Tekst).ToList();

                //3. Sende resultat av LINQ-spørring til View

                return View(newsListe);
            }
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
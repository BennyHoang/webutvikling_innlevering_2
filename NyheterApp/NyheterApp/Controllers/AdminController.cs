using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NyheterApp.Models;
using System.IO;

namespace NyheterApp.Controllers
{
    public class AdminController : Controller
    {

        //Lagnyhet siden
        [HttpGet]
        public ActionResult LagNyNyhet()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LagNyNyhet(New news, HttpPostedFileBase bildefil)
        {

            try
            {
                //Lagre bildefil
                String bildenavn = Path.GetFileName(bildefil.FileName);
                String bildefilsti = Path.Combine(Server.MapPath("~/Content/Bilder"), bildenavn);
                bildefil.SaveAs(bildefilsti);

                 using (DataAuthorOrmDataContext DataAuthor = new DataAuthorOrmDataContext())
                {
                    news.BildeSrc = bildenavn;
                    DataAuthor.News.InsertOnSubmit(news);
                    DataAuthor.SubmitChanges();
                }
                ViewBag.LastetOpp = true;

            }
            catch (Exception ex)
            {
                ViewBag.LastetOpp = false;
                ViewBag.Feilmelding = ex.Message;
            }
            return View();
        }

       /* [HttpPost]
        public ActionResult LagNyNyhet(new bilde, HttpPostedFileBase bildefil)
        {
            try
            {
                //Lagre bildefil
                String bildenavn = Path.GetFileName(bildefil.FileName);
                String bildefilsti = Path.Combine(Server.MapPath("~/Content/Bilder"), bildenavn);
                bildefil.SaveAs(bildefilsti);

                //Lagre ny bildeentitet
                using (BilderOrmDataContext bilderOrm = new BilderOrmDataContext())
                {
                    bilde.BildeSrc = bildenavn;

                    bilderOrm.Bildes.InsertOnSubmit(bilde);
                    bilderOrm.SubmitChanges();
                }

                ViewBag.LastetOpp = true;

            }
            catch (Exception ex)
            {
                ViewBag.LastetOpp = false;
                ViewBag.Feilmelding = ex.Message;
            }


            return View();
        }
        */


        //Vis nyheter   
        public ActionResult VisAlleNyheter()
        {
            //LINQ
            //1. koble til ORM (DB)
            using (DataAuthorOrmDataContext DataAuthor = new DataAuthorOrmDataContext())
            {

                //2. LINQ-spørringen
                List<New> newsListe = (from New in DataAuthor.News
                                            select New).ToList();

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
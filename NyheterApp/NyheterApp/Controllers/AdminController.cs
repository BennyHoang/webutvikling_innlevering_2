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
        public ActionResult LagNyNyhet(Nyhet Nyhets, HttpPostedFileBase bildefil)
        {

            try
            {
                //Lagre bildefil
                String bildenavn = Path.GetFileName(bildefil.FileName);
                String bildefilsti = Path.Combine(Server.MapPath("~/Content/Bilder"), bildenavn);
                bildefil.SaveAs(bildefilsti);

                 using (DataAuthorOrmDataContext DataAuthor = new DataAuthorOrmDataContext())
                {
                    Nyhets.BildeSrc = bildenavn;
                    DataAuthor.Nyhets.InsertOnSubmit(Nyhets);
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

     

        //Vis nyheter   
        public ActionResult VisAlleNyheter()
        {
            //LINQ
            //1. koble til ORM (DB)
            using (DataAuthorOrmDataContext DataAuthor = new DataAuthorOrmDataContext())
            {

                //2. LINQ-spørringen
                List<Nyhet> NyhetsListe = (from Nyhet in DataAuthor.Nyhets
                                            select Nyhet).ToList();

                //3. Sende resultat av LINQ-spørring til View

                return View(NyhetsListe);
            }
        }

        //Vis en nyhet

        public ActionResult VisEnNyhet(int? id) 
        {
            if (id != null)
            {
                using (DataAuthorOrmDataContext nyhetOrm = new DataAuthorOrmDataContext())
                {
                    Nyhet valgtArtikkel = (from Nyhets in nyhetOrm.Nyhets
                                        where Nyhets.Id == id
                                        select Nyhets).SingleOrDefault();
                    ViewBag.Artikkel = valgtArtikkel.Tittel;
                    return View(valgtArtikkel);
                }

            }
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
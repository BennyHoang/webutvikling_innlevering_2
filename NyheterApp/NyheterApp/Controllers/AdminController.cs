﻿using System;
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
                    Nyhets.DatoPostet = DateTime.Now;
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
                List<Nyhet> NyhetsListe = (from nyheter in DataAuthor.Nyhets
                                            select nyheter).ToList();

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
                    Nyhet valgtArtikkel = (from nyheter in nyhetOrm.Nyhets
                                        where nyheter.Id == id
                                        select nyheter).SingleOrDefault();


                    return View(valgtArtikkel);
                }

            }
            return View();  
        }

        //Rediger nyehter
        /*
       public ActionResult RedigerNyhet(int? id)
        {
            if (id != null)
            {
                using (DataAuthorOrmDataContext nyhetOrm = new DataAuthorOrmDataContext())
                {
                    Nyhet valgtArtikkel = (from Nyhets in nyhetOrm.Nyhets
                                           where Nyhets.Id == id
                                           select Nyhets).SingleOrDefault();
                    return View(valgtArtikkel);
                }

            }

            return View();
        }
         */
       

      public ActionResult RedigerNyheter()
        {
            //LINQ
            //1. koble til ORM (DB)
            using (DataAuthorOrmDataContext DataAuthor = new DataAuthorOrmDataContext())
            {

                //2. LINQ-spørringen
                List<Nyhet> NyhetsListe = (from nyheter in DataAuthor.Nyhets
                                           select nyheter).ToList();

                //3. Sende resultat av LINQ-spørring til View
                return View(NyhetsListe);
            }
        }






      public ActionResult RedigerNyhet(int? id)
        {
            if (id != null)
            {
                using (DataAuthorOrmDataContext nyhetOrm = new DataAuthorOrmDataContext())
                {
                    Nyhet valgtArtikkel = (from nyheter in nyhetOrm.Nyhets
                                           where nyheter.Id == id
                                           select nyheter).SingleOrDefault();
                    return View(valgtArtikkel);
                }

            }
            return View();
        }

      [HttpPost]
      public ActionResult RedigerNyhet(Nyhet nyhet)
      {
          using (DataAuthorOrmDataContext nyheterOrm = new DataAuthorOrmDataContext())
          {
              Nyhet valgtNyhet = (from nyheter in nyheterOrm.Nyhets
                                  where nyheter.Id == nyhet.Id
                                  select nyheter).SingleOrDefault();

              valgtNyhet.Tittel = nyhet.Tittel;
              valgtNyhet.Id = nyhet.Id;
              valgtNyhet.Tekst = nyhet.Tekst;



              nyheterOrm.SubmitChanges();

              //return View(valgtBilde);
              //URL redirecting
              return RedirectToAction("RedigerNyheter");

          }
      }

        //Denne er for å vise infoen i slettnyhet før submit klikkes

        [HttpGet]
        public ActionResult SlettNyhet(int? id)
        {
            if (id != null)
            {
                using (DataAuthorOrmDataContext nyhetOrm = new DataAuthorOrmDataContext())
                {
                    Nyhet valgtArtikkel = (from nyheter in nyhetOrm.Nyhets
                                           where nyheter.Id == id
                                           select nyheter).SingleOrDefault();
                    return View(valgtArtikkel);
                }

            }
            return View();
        }
      

         
        [HttpPost]
        public ActionResult SlettNyhet(Nyhet nyhet)
        {
            using (DataAuthorOrmDataContext nyheterOrm = new DataAuthorOrmDataContext())
            {
                Nyhet valgtNyhet = (from nyheter in nyheterOrm.Nyhets
                                    where nyheter.Id == nyhet.Id
                                    select nyheter).SingleOrDefault();
                nyheterOrm.Nyhets.DeleteOnSubmit(valgtNyhet);


                nyheterOrm.SubmitChanges();

                //URL redirecting

            }
            return RedirectToAction("RedigerNyheter");

        }
        //Her kommer biten som tar seg av innlogging

        public ActionResult LogIn() {

            return View();
        }

        public ActionResult LogOut() {

            return View();
        }

        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
	}
}
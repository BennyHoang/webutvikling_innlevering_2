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
            //denne sjekker om bruker er innlogget
            if (Session["innlogget"] != null)
            {
                ViewBag.Brukernavn = (string)Session["innlogget"];
                return View();
            }
            //Dersom bruker ikke er innlogget må han logge inn 
            else
            {
                return RedirectToAction("LogIn");
            }
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

                using (NyhetOrmDbDataContext nyhetOrm = new NyhetOrmDbDataContext())
                {
                    Nyhets.DatoPostet = DateTime.Now;
                    Nyhets.BildeSrc = bildenavn;
                    Nyhets.ForfatterId = (int)Session["brukerId"];
                    nyhetOrm.Nyhets.InsertOnSubmit(Nyhets);
                    nyhetOrm.SubmitChanges();
                }
                ViewBag.LastetOpp = true;

            }
            catch (Exception ex)
            {
                ViewBag.LastetOpp = false;
                ViewBag.FeilmeldingNyNyhet = "Databasefeil skjedde, kontakt admin dersom feilen vedvarer ";
            }
            return View();
        }



        //Vis alle nyheter fra databasen   
        public ActionResult VisAlleNyheter()
        {
            HentAlleNyheter();
            return View();
        }

        //Vis en nyhet

        public ActionResult VisEnNyhet(int? id)
        {
            HentEnNyhet(id);
            return View();
        }

        //Rediger nyehter
 
        public ActionResult RedigerNyheter()
        {
            //denne sjekker om bruker er innlogget
            if (Session["innlogget"] != null)
            {
                ViewBag.Brukernavn = (string)Session["innlogget"];
                HentAlleNyheter();
                return View();
            }
            else
            {
                return RedirectToAction("LogIn");
            }
        }


        public ActionResult RedigerNyhet(int? id)
        {
            if (Session["innlogget"] != null)
            {
                ViewBag.Brukernavn = (string)Session["innlogget"];
                HentEnNyhet(id);
                return View();
            }

            else
            {
                return RedirectToAction("LogIn");
            }
        }

        


        [HttpPost]
        public ActionResult RedigerNyhet(Nyhet nyhet)
        {
            try {
                using (NyhetOrmDbDataContext nyheterOrm = new NyhetOrmDbDataContext())
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
            catch
            {
                ViewBag.feilRedigerNyhet = "Databasefeil skjedde, kontakt admin dersom feilen vedvarer ";
            }

            return View();
        }

        //Denne er for å vise infoen i slettnyhet før submit klikkes

        [HttpGet]
        public ActionResult SlettNyhet(int? id)
        {
            HentEnNyhet(id);
            return View();
        }



        [HttpPost]
        public ActionResult SlettNyhet(Nyhet nyhet)
        {
            try {
                using (NyhetOrmDbDataContext nyheterOrm = new NyhetOrmDbDataContext())
                {
                    Nyhet valgtNyhet = (from nyheter in nyheterOrm.Nyhets
                                        where nyheter.Id == nyhet.Id
                                        select nyheter).SingleOrDefault();
                    nyheterOrm.Nyhets.DeleteOnSubmit(valgtNyhet);


                    nyheterOrm.SubmitChanges();

                    //URL redirecting

                }
            }
            catch 
            {
                ViewBag.feilSlettNyhet = "Databasefeil skjedde, kontakt admin dersom feilen vedvarer ";
            }
            return RedirectToAction("RedigerNyheter");

        }
        //Her kommer biten som tar seg av innlogging
        [HttpGet]
        public ActionResult LogIn() {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Forfatter bruker, Forfatter passord)
        {

            try
            {
                using (NyhetOrmDbDataContext brukerOrm = new NyhetOrmDbDataContext())
                {

                    List<Forfatter> brukerData = (from forfattere in brukerOrm.Forfatters
                                               select forfattere).ToList();

                    String brukernavnPost = bruker.Brukernavn;
                    String passordPost = bruker.Passord;
                    //løkken kjører gjennom alle brukere for å se om brukernavnet stemmer med en bruker
                    foreach (var brukere in brukerData)
                    {
                        if (brukere.Brukernavn == brukernavnPost && brukere.Passord == passordPost)
                        {
                            Session.Add("innlogget", bruker.Brukernavn);
                            Session.Add("brukerId", brukere.Id);
                            return RedirectToAction("RedigerNyheter");

                        }
                        else
                        {
                            ViewBag.innlogging = "Feil passord eller brukernavn, prøv igjen.";

                        }

                    }
                }
            }catch
            {
                ViewBag.feilKobleTilDatabaseInnlogging = "Det skjedde en feil, prøv igjen, eller kontakt admin dersom feilen vedvarer.";

            }


            return View();

        }
    

        public ActionResult LogOut() {

            //Fjerner session ved log ut
            Session.Remove("innlogget");
            return RedirectToAction("VisAlleNyheter");
            
        }

        //Denne henter ut alle nyheter fra databasen
        public ActionResult HentAlleNyheter() {
            try
            {
                using (NyhetOrmDbDataContext nyhetOrm = new NyhetOrmDbDataContext())
                {
                    List<Nyhet> NyhetsListe = (from nyheter in nyhetOrm.Nyhets
                                               select nyheter).ToList();
                    return View(NyhetsListe);

                }
            }
            catch
            {
                ViewBag.feilVisAlleNyheter = "Det skjedde en feil, prøv igjen, eller kontakt admin dersom feilen vedvarer.";

            }

            return View();
        }

        //Denne brukes for å hene nyhet fra databasen når kun en nyhet skal vises
        public ActionResult HentEnNyhet(int? id)
        {

            if (id != null)
            {
                try
                {
                    using (NyhetOrmDbDataContext nyhetOrm = new NyhetOrmDbDataContext())
                    {
                        Nyhet valgtArtikkel = (from nyheter in nyhetOrm.Nyhets
                                               where nyheter.Id == id
                                               select nyheter).SingleOrDefault();
                        return View(valgtArtikkel);
                    }
                }
                catch
                {
                    ViewBag.feilEnNyhet = "Databasefeil skjedde, kontakt admin dersom feilen vedvarer ";
                }

            }
            return View();
        }
 
	}
}

using FitnesCentarJovana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnesCentarJovana.Controllers
{
    public class PrijavljivanjeController : Controller
    {
        // GET: Prijavljivanje
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PokusajPrijave(string korisnickoime, string lozinka)
        {
            List<Korisnik> korisnici = Pomocna.Ucitaj<Korisnik>("Korisnik");
            Korisnik k = korisnici.FirstOrDefault(kor => kor.KorisnickoIme == korisnickoime && kor.Lozinka == lozinka && kor.Obrisan == false);
            if (k != null)
            {
                Session["KORISNIK"] = k;
                return RedirectToAction("Opcije");
            }

            TempData["Poruka"] = "Niste dobro uneli korisnicko ime ili sifru";
            return View("Index");
        }

        public ActionResult OdjaviSe()
        {
            Session["KORISNIK"] = null;
            return View("Index");
        }

        public ActionResult Opcije()
        {
            return View("");
        }
    }
}
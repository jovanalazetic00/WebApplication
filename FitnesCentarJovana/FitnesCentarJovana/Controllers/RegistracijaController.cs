using FitnesCentarJovana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnesCentarJovana.Controllers
{
    public class RegistracijaController : Controller
    {
        // GET: Registracija
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PokusajRegistracije(Korisnik korisnik, string fitnes_centar)
        {
            List<Korisnik> listaKorisnika = (List<Korisnik>)HttpContext.Application["KORISNICI"];
            if(listaKorisnika.Find(k => k.KorisnickoIme == korisnik.KorisnickoIme) == null)
            {
                if(korisnik.Uloga == ULOGE.TRENER)
                {
                    korisnik.Fitnes_Centar_Koji_Trenira = fitnes_centar;
                    korisnik.Grupni_Treninzi_Trener_Angazovan = new List<string>(); 
                }
                else if(korisnik.Uloga == ULOGE.VLASNIK)
                {
                    korisnik.Moji_Fitnes_Centri = new List<string>();
                }
                else
                {
                    korisnik.Grupni_Treninzi_Posetilac_Pohadja = new List<string>();
                }
               
                listaKorisnika.Add(korisnik);
                Pomocna.Upisivanje(listaKorisnika,"Korisnika");
                return View("../Prijavljivanje/Index");
            }

            TempData["Poruka"] = "Molim vas unesite drugo korisicko ime, ovo vec postoji";
            
            if(korisnik.Uloga == ULOGE.TRENER)
            {
                List<string> vlasnikovi_fitnes_centri = ((Korisnik)Session["KORISNIK"]).Moji_Fitnes_Centri;
                TempData["fitnes_centri"] = vlasnikovi_fitnes_centri;
                return View("RegistracijaTrenera");
            }
            return View("Index");
        }

        public ActionResult Blokiranje(string korisnicko_ime_trenera)
        {
            if (Request.HttpMethod.ToString() == "GET")
            {
                List<string> vlasnikovi_fitnes_centri = ((Korisnik)Session["KORISNIK"]).Moji_Fitnes_Centri;
                TempData["treneri"] = ((List<Korisnik>)HttpContext.Application["KORISNICI"]).FindAll(t => t.Uloga == ULOGE.TRENER && t.Obrisan == false && vlasnikovi_fitnes_centri.Contains(t.Fitnes_Centar_Koji_Trenira));
                return View();
            }
            else
            {
                List<Korisnik> listaKorisnika = (List<Korisnik>)HttpContext.Application["KORISNICI"];
                listaKorisnika.FirstOrDefault(t => t.KorisnickoIme == korisnicko_ime_trenera).Obrisan = true;
                Pomocna.Upisivanje(listaKorisnika, "Korisnika");
                return View("../Prijavljivanje/Index");

            }

        }

        public ActionResult RegistracijaTrenera()
        {
            List<string> vlasnikovi_fitnes_centri = ((Korisnik)Session["KORISNIK"]).Moji_Fitnes_Centri;
            if(vlasnikovi_fitnes_centri.Count == 0)
            {
                TempData["Poruka"] = "Nije moguce dodati trenera jer nemate dodate fitnes centre";
                return View("../Prijavljivanje/Opcije");
            }
            TempData["fitnes_centri"] = vlasnikovi_fitnes_centri;
            return View();
        }
    }
}
using FitnesCentarJovana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnesCentarJovana.Controllers
{
    public class FitnesCentarController : Controller
    {
        public ActionResult DodavanjeFitnesCentra()
        {
            return View();
        }

        public ActionResult PokusajDodavanja(Fitnes_Centar fitnes_centar)
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            if (lista_Fitnes_Centara.Find(f => f.Naziv == fitnes_centar.Naziv) == null)
            {
                string korisnicko_ime_vlasnika = ((Korisnik)Session["KORISNIK"]).KorisnickoIme;
                fitnes_centar.Vlasnik = korisnicko_ime_vlasnika;
                Korisnik vlasnik = ((List<Korisnik>)HttpContext.Application["KORISNICI"]).FirstOrDefault(k => k.KorisnickoIme == korisnicko_ime_vlasnika);
                vlasnik.Moji_Fitnes_Centri.Add(fitnes_centar.Naziv);
                Pomocna.Upisivanje((List<Korisnik>)HttpContext.Application["KORISNICI"], "Korisnika");

                lista_Fitnes_Centara.Add(fitnes_centar);
                Pomocna.Upisivanje(lista_Fitnes_Centara, "Fitnes_Centra");
                return RedirectToAction("PrikazVlasnikovihFitnesCentara");
            }

            TempData["Poruka"] = "Ime Fitnes Centra Vec Postoji";
            //TempData["fitnes_centar_naziv"] = fitnes_centar.Naziv;
            return View("DodavanjeFitnesCentra");

        }

        public ActionResult ModifikujFitnesCentar(string fitnes_centar_naziv)
        {
            TempData["fitnes_centar_naziv"] = fitnes_centar_naziv;
            return View();
        }

        public ActionResult PrikazVlasnikovihFitnesCentara()
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            string korisnicko_ime_vlasnika = ((Korisnik)Session["KORISNIK"]).KorisnickoIme;
            TempData["fitnes_centri"] = lista_Fitnes_Centara.FindAll(f => f.Vlasnik == korisnicko_ime_vlasnika && f.Obrisan == false);
            return View("PrikazFitnesCentaraOsnovno");
        }

        public ActionResult PrikazSvih()
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            TempData["fitnes_centri"] = lista_Fitnes_Centara.FindAll(f => f.Obrisan == false).OrderBy(f => f.Naziv).ToList();
            List<Komentar> lista_Komentara  = (List<Komentar>)HttpContext.Application["KOMENTARI"];
            TempData["komentari"] = lista_Komentara.FindAll(k => k.Obrisan == false);
            return View("PrikazFitnesCentaraOsnovno");
        }

        public ActionResult PrikazFitnesCentaraDetaljno(string fitnes_centar_naziv)
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            TempData["fitnes_centar"] = lista_Fitnes_Centara.FirstOrDefault(f => f.Naziv == fitnes_centar_naziv);
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => g.Obrisan == false && g.FitnesCentar == fitnes_centar_naziv && DateTime.Parse(g.DatumVreme) > DateTime.Now);
            List<Komentar> lista_Komentara = (List<Komentar>)HttpContext.Application["KOMENTARI"];
            TempData["komentari"] = lista_Komentara.FindAll(k => k.Obrisan == false && k.FitnesCentar ==  fitnes_centar_naziv);
            return View();

        }

        public ActionResult Pretraga(Fitnes_Centar fitnes_centar, int? minGodinaOtvaranja = 0, int? maxGodinaOtvaranja = 0)
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];

            if (minGodinaOtvaranja != 0) //moze sa HasValue
            {
                lista_Fitnes_Centara = lista_Fitnes_Centara.FindAll(f => f.GodinaOtvaranja > minGodinaOtvaranja && f.Obrisan == false);
            }
            if (maxGodinaOtvaranja != 0)
            {
                lista_Fitnes_Centara = lista_Fitnes_Centara.FindAll(f => f.GodinaOtvaranja < maxGodinaOtvaranja && f.Obrisan == false);
            }
            if (fitnes_centar.Adresa != null)
            {
                lista_Fitnes_Centara = lista_Fitnes_Centara.FindAll(f => f.Adresa == fitnes_centar.Adresa && f.Obrisan == false);
            }
            if (fitnes_centar.Naziv != null)
            {
                lista_Fitnes_Centara = lista_Fitnes_Centara.FindAll(f => f.Naziv == fitnes_centar.Naziv && f.Obrisan == false);
            }
       
            TempData["fitnes_centri"] = lista_Fitnes_Centara;

            return View("PrikazFitnesCentaraOsnovno");
        }

        public ActionResult Sortiranje(Fitnes_Centar fitnes_centar, string GodinaOtvaranja)
        {      
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            TempData["fitnes_centri"] = lista_Fitnes_Centara;

            if (fitnes_centar.Naziv == "O")
            {
                TempData["fitnes_centri"] = lista_Fitnes_Centara.OrderByDescending(f => f.Naziv).ToList();
            }
            else if (fitnes_centar.Naziv == "R")
            {
                TempData["fitnes_centri"] = lista_Fitnes_Centara.OrderBy(f => f.Naziv).ToList();
            }

            if (fitnes_centar.Adresa == "O")
            {
                TempData["fitnes_centri"] = lista_Fitnes_Centara.OrderByDescending(f => f.Adresa).ToList();
            }
            else if (fitnes_centar.Adresa == "R")
            {
                TempData["fitnes_centri"] = lista_Fitnes_Centara.OrderBy(f => f.Adresa).ToList();
            }

            if (GodinaOtvaranja == "O")
            {
                TempData["fitnes_centri"] = lista_Fitnes_Centara.OrderByDescending(f => f.GodinaOtvaranja).ToList();
            }
            else if (GodinaOtvaranja == "R")
            {
                TempData["fitnes_centri"] = lista_Fitnes_Centara.OrderBy(f => f.GodinaOtvaranja).ToList();
            }


            return View("PrikazFitnesCentaraOsnovno");
        }

        public ActionResult ObrisiFitnesCentar(string fitnes_centar_naziv)
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            Fitnes_Centar fitn = lista_Fitnes_Centara.FirstOrDefault(f => f.Naziv == fitnes_centar_naziv);
            if (lista_Grupnih_Treninga.FirstOrDefault(g => g.FitnesCentar == fitn.Naziv && DateTime.Parse(g.DatumVreme) > DateTime.Now) != null)
            {
                TempData["Poruka"] = "Postoji Grupni Trening u Buducnosti Koji Ce se odrzati";
                lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
                string korisnicko_ime_vlasnika = ((Korisnik)Session["KORISNIK"]).KorisnickoIme;
                TempData["fitnes_centri"] = lista_Fitnes_Centara.FindAll(f => f.Vlasnik == korisnicko_ime_vlasnika && f.Obrisan == false);
                return View("PrikazFitnesCentaraOsnovno");
            }

            lista_Fitnes_Centara.FirstOrDefault(f => f.Naziv == fitnes_centar_naziv).Obrisan = true;
            

            Pomocna.Upisivanje(lista_Fitnes_Centara, "Fitnes_Centra");

            List<Korisnik> lista_trenera = ((List<Korisnik>)HttpContext.Application["KORISNICI"]).FindAll(t => t.Uloga == ULOGE.TRENER && t.Fitnes_Centar_Koji_Trenira == fitnes_centar_naziv);

            foreach(Korisnik trener in lista_trenera)
            {
                trener.Obrisan = true;
            }

            Pomocna.Upisivanje((List<Korisnik>)HttpContext.Application["KORISNICI"], "Korisnika");

            return RedirectToAction("PrikazVlasnikovihFitnesCentara");
        }

        public ActionResult PokusajModifikacije(Fitnes_Centar fitnes_centar , string fitnes_centar_naziv)
        {
            List<Fitnes_Centar> lista_Fitnes_Centara = (List<Fitnes_Centar>)HttpContext.Application["FITNES_CENTRI"];
            List<Korisnik> lista_Korisnika = (List<Korisnik>)HttpContext.Application["KORISNICI"];
            Korisnik korisnik_iz_liste = lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme);

            Fitnes_Centar fitnes_centar_iz_liste = lista_Fitnes_Centara.FirstOrDefault(f => f.Naziv == fitnes_centar_naziv);

            if (fitnes_centar.Naziv != String.Empty) {
                if(lista_Fitnes_Centara.Exists(f => f.Naziv == fitnes_centar.Naziv))
                {
                    TempData["Poruka"] = "Ime Fitnes Centra Vec Postoji";
                    TempData["fitnes_centar_naziv"] = fitnes_centar_naziv;
                    return View("ModifikujFitnesCentar");
                }

                              
            }
            if (fitnes_centar.Naziv != " ") {
                fitnes_centar_iz_liste.Naziv = fitnes_centar.Naziv;
            }
            if (fitnes_centar.Adresa != " ") {
                fitnes_centar_iz_liste.Adresa = fitnes_centar.Adresa;
            }
            if (fitnes_centar.GodinaOtvaranja.ToString() != " ") {
                fitnes_centar_iz_liste.GodinaOtvaranja = fitnes_centar.GodinaOtvaranja;
            }
            if (fitnes_centar.CMesecne != 0) {
                fitnes_centar_iz_liste.CMesecne = fitnes_centar.CMesecne;
            }
            if (fitnes_centar.CGodisnje != 0) {
                fitnes_centar_iz_liste.CGodisnje = fitnes_centar.CGodisnje;
            }
            if (fitnes_centar.CJednogObicnog != 0) {
                fitnes_centar_iz_liste.CJednogObicnog = fitnes_centar.CJednogObicnog;
            }
            if (fitnes_centar.CGrupnog != 0) {
                fitnes_centar_iz_liste.CGrupnog = fitnes_centar.CGrupnog;
            }
            if (fitnes_centar.CJednogPerosnalnog != 0) {
                fitnes_centar_iz_liste.CJednogPerosnalnog = fitnes_centar.CJednogPerosnalnog;
            }

            int index = 0;

            foreach (var i in lista_Korisnika)
            {
                if (i.Fitnes_Centar_Koji_Trenira != null && i.Fitnes_Centar_Koji_Trenira.Contains(fitnes_centar_naziv))
                {
                    i.Fitnes_Centar_Koji_Trenira = fitnes_centar.Naziv;
                }
            }

            if (korisnik_iz_liste.Moji_Fitnes_Centri.Contains(fitnes_centar_naziv))
            {
                index = lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme).Moji_Fitnes_Centri.FindIndex(f => f == fitnes_centar_naziv);
                lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme).Moji_Fitnes_Centri[index] = fitnes_centar.Naziv;
            }

            Pomocna.Upisivanje(lista_Korisnika, "Korisnika");
            Pomocna.Upisivanje(lista_Fitnes_Centara, "Fitnes_Centra");
            Session["KORISNIK"] = lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme);

            return RedirectToAction("PrikazVlasnikovihFitnesCentara");

        }



    }
}
using FitnesCentarJovana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FitnesCentarJovana.Controllers
{
    public class GrupniTreningController : Controller
    {
        // GET: GrupniTrening
        public ActionResult DodavanjeGrupnogTreninga()
        {
            return View();
        }

        public ActionResult PokusajDodavanja(Grupni_Trening grupni_trening)
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            if (lista_Grupnih_Treninga.Find(g => g.Naziv == grupni_trening.Naziv) == null && DateTime.Parse(grupni_trening.DatumVreme) > DateTime.Now.AddDays(3))
            {
                string korisnicko_ime_trenera = ((Korisnik)Session["KORISNIK"]).KorisnickoIme;
                string fitnes_centar = ((Korisnik)Session["KORISNIK"]).Fitnes_Centar_Koji_Trenira;
                grupni_trening.FitnesCentar = fitnes_centar;
                grupni_trening.SpisakPosetilaca = new List<string>();
                Korisnik trener = ((List<Korisnik>)HttpContext.Application["KORISNICI"]).FirstOrDefault(k => k.KorisnickoIme == korisnicko_ime_trenera);
                trener.Grupni_Treninzi_Trener_Angazovan.Add(grupni_trening.Naziv);
                Session["KORISNIK"] = trener;
                Pomocna.Upisivanje((List<Korisnik>)HttpContext.Application["KORISNICI"], "Korisnika");

                lista_Grupnih_Treninga.Add(grupni_trening);
                Pomocna.Upisivanje(lista_Grupnih_Treninga, "Grupnih_Treninga");
                return RedirectToAction("PrikazTrenerovihGrupnihTreninga");
            }

            TempData["Poruka"] = "Ime Grupnog Treninga Vec Postoji ili Ga Niste Kreirali Najmanje 3 Dana Unapred";
            //TempData["fitnes_centar_naziv"] = fitnes_centar.Naziv;
            return View("DodavanjeGrupnogTreninga");

        }

        public ActionResult PrikazTrenerovihGrupnihTreninga()
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            List<string> grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Trener_Angazovan;
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => grupni_treninzi_trenera.Contains(g.Naziv) && g.Obrisan == false);
            return View("PrikazGrupnihTreninga");
        }

        public ActionResult PrikazTrenerovihGrupnihTreningaUProslosti()
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            List<string> grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Trener_Angazovan;
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => grupni_treninzi_trenera.Contains(g.Naziv) && g.Obrisan == false && DateTime.Parse(g.DatumVreme) < DateTime.Now);
            return View("PrikazGrupnihTreninga");
        }

        public ActionResult PrikazSvihPredstojecih(string fitnes_centar_naziv)
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => g.FitnesCentar == fitnes_centar_naziv && DateTime.Parse(g.DatumVreme) > DateTime.Now && g.Obrisan == false);
            return View("PrikazGrupnihTreninga");
        }

        public ActionResult PrikazSvihRanijih()
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            Korisnik posetilac = (Korisnik)Session["KORISNIK"];
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) < DateTime.Now && g.SpisakPosetilaca.Contains(posetilac.KorisnickoIme) && g.Obrisan == false);
            TempData["komentar"] = true;
            return View("PrikazGrupnihTreninga");
        }

        public ActionResult OstaviKomentar(string grupni_trening_naziv)
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            TempData["fitnes_centar_naziv"] = lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv).FitnesCentar;
            return View("OstaviKomentar");
        }

        public ActionResult PokusajDodavanjaKomentara(Komentar komentar, string fitnes_centar_naziv)
        {
            List<Komentar> lista_Komentara= (List<Komentar>)HttpContext.Application["KOMENTARI"];

            komentar.FitnesCentar = fitnes_centar_naziv;
            komentar.Posetilac = ((Korisnik)Session["KORISNIK"]).KorisnickoIme;
            komentar.Obrisan = true;
            lista_Komentara.Add(komentar);
            Pomocna.Upisivanje(lista_Komentara, "Komentara");
            
            return RedirectToAction("PrikazSvih", "FitnesCentar");

        }

        public ActionResult OpcijeKomentar()
        {
            List<Komentar> lista_Komentara = (List<Komentar>)HttpContext.Application["KOMENTARI"];
            List<string> vlasnikovi_fitnes_centri = ((Korisnik)Session["KORISNIK"]).Moji_Fitnes_Centri;
            TempData["komentari"] = lista_Komentara.FindAll(k => vlasnikovi_fitnes_centri.Contains(k.FitnesCentar));
            return View();
        }

        public ActionResult OdobriBlokirajKomentar(string tekst_komentara, string naziv_fitnes_centra)
        {
            List<Komentar> lista_Komentara = (List<Komentar>)HttpContext.Application["KOMENTARI"];
            lista_Komentara.FirstOrDefault(k => k.Tekst == tekst_komentara && k.FitnesCentar == naziv_fitnes_centra).Obrisan = false;
            Pomocna.Upisivanje(lista_Komentara, "Komentara");
            return RedirectToAction("OpcijeKomentar");
        }

        public ActionResult ModifikujGrupniTrening(string grupni_trening_naziv)
        {
            TempData["grupni_trening_naziv"] = grupni_trening_naziv;
            return View();
        }

        public ActionResult PokusajModifikacije(Grupni_Trening grupni_trening, string grupni_trening_naziv)
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            List<Korisnik> lista_Korisnika = (List<Korisnik>)HttpContext.Application["KORISNICI"];


            Grupni_Trening grupni_trening_iz_liste = lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv);
            Korisnik korisnik_iz_liste = lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme);


            if (grupni_trening.Naziv != null)
            {
                if (lista_Grupnih_Treninga.Exists(g => g.Naziv == grupni_trening.Naziv) || DateTime.Parse(grupni_trening_iz_liste.DatumVreme) < DateTime.Now)
                {
                    TempData["Poruka"] = "Ime Fitnes Centra Vec Postoji Ili Modifkijujete Grupni Trening Koji Je Prosao";
                    TempData["grupni_trening_naziv"] = grupni_trening_naziv;
                    return View("ModifikujGrupniTrening");
                }
            }
            if (grupni_trening.Naziv != null) {
                grupni_trening_iz_liste.Naziv = grupni_trening.Naziv;
            }
            if (grupni_trening.TipTreninga.ToString() != null) {
                grupni_trening_iz_liste.TipTreninga = grupni_trening.TipTreninga;
            }
            if (grupni_trening.Trajanje != 0) {
                grupni_trening_iz_liste.Trajanje = grupni_trening.Trajanje;
            }
            if (grupni_trening.DatumVreme != null) {
                grupni_trening_iz_liste.DatumVreme = grupni_trening.DatumVreme.ToString();
            }
            if (grupni_trening.MaksimumPosetialca != 0) {
                grupni_trening_iz_liste.MaksimumPosetialca = grupni_trening.MaksimumPosetialca;
            }

            int index = 0;

            foreach (var i in lista_Korisnika)
            {
                if(i.Grupni_Treninzi_Posetilac_Pohadja != null && i.Grupni_Treninzi_Posetilac_Pohadja.Contains(grupni_trening_naziv))
                {
                    index = i.Grupni_Treninzi_Posetilac_Pohadja.FindIndex(g => g == grupni_trening_naziv);
                    i.Grupni_Treninzi_Posetilac_Pohadja[index] = grupni_trening.Naziv;
                }
            }

            
            index = lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme).Grupni_Treninzi_Trener_Angazovan.FindIndex(g => g == grupni_trening_naziv);

            if (korisnik_iz_liste.Grupni_Treninzi_Trener_Angazovan.Contains(grupni_trening_naziv))
            {
                lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme).Grupni_Treninzi_Trener_Angazovan[index] = grupni_trening.Naziv;
            }
            Pomocna.Upisivanje(lista_Korisnika, "Korisnika");
            Pomocna.Upisivanje(lista_Grupnih_Treninga, "Grupnih_Treninga");

            Session["KORISNIK"] = lista_Korisnika.FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme);
            return RedirectToAction("PrikazTrenerovihGrupnihTreninga");

        }

        public ActionResult ObrisiGrupniTrening(string grupni_trening_naziv)
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            Grupni_Trening grupni_trening_iz_liste = lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv);

            if (lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv).SpisakPosetilaca.Count > 0 || DateTime.Parse(grupni_trening_iz_liste.DatumVreme) < DateTime.Now)
            {
                TempData["Poruka"] = "Nije Moguce Obrisati Ovaj Grupni Trening Jer Ima Posetilaca Ili se Zavesio";

                List<string> grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Trener_Angazovan;
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => grupni_treninzi_trenera.Contains(g.Naziv) && g.Obrisan == false);
                return View("PrikazGrupnihTreninga");
            }
            lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv).Obrisan = true;
            Pomocna.Upisivanje(lista_Grupnih_Treninga, "Grupnih_Treninga");

            return RedirectToAction("PrikazTrenerovihGrupnihTreninga");
        }
        
        public ActionResult Pretraga(Grupni_Trening grupni_trening, string TipTreninga, DateTime? minDatumVreme = null, DateTime? maxDatumVreme = null )
        {
            Korisnik posetilac = (Korisnik)Session["KORISNIK"];
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            List<string> grupni_treninzi_trenera = new List<string>();
            if (((Korisnik)Session["KORISNIK"]).Uloga == ULOGE.POSETILAC)
            {
                 grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Posetilac_Pohadja;
            }
            else if(((Korisnik)Session["KORISNIK"]).Uloga == ULOGE.TRENER)
            {
                 grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Trener_Angazovan;
            }            
            lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) < DateTime.Now && grupni_treninzi_trenera.Contains(g.Naziv) && g.Obrisan == false);

            if (((Korisnik)Session["KORISNIK"]).Uloga == ULOGE.POSETILAC)
            {
                lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) < DateTime.Now && g.SpisakPosetilaca.Contains(posetilac.KorisnickoIme) && g.Obrisan == false);
                TempData["komentar"] = true;
            }


            if (minDatumVreme != null) //moze sa HasValue
            {
                lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) > minDatumVreme && g.Obrisan == false);
            }
            if (maxDatumVreme != null)
            {
                lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) < maxDatumVreme && g.Obrisan == false);
            }
            if(grupni_trening.Naziv != null)
            {
                lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => g.Naziv == grupni_trening.Naziv && g.Obrisan == false);
            }
            if (TipTreninga != "")
            {
                lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => g.TipTreninga.ToString() == TipTreninga && g.Obrisan == false);
            }

            TempData["grupni_treninzi"] = lista_Grupnih_Treninga;

            return View("PrikazGrupnihTreninga");
        }

        public ActionResult Sortiranje(Grupni_Trening grupni_trening, string TipTreninga)
        {
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            List<string> grupni_treninzi_trenera = new List<string>();
            if (((Korisnik)Session["KORISNIK"]).Uloga == ULOGE.POSETILAC)
            {
                grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Posetilac_Pohadja;
            }
            else if (((Korisnik)Session["KORISNIK"]).Uloga == ULOGE.TRENER)
            {
                grupni_treninzi_trenera = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Trener_Angazovan;
            }
            lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) < DateTime.Now && grupni_treninzi_trenera.Contains(g.Naziv) && g.Obrisan == false);
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga;

            if (((Korisnik)Session["KORISNIK"]).Uloga == ULOGE.POSETILAC)
            {
                Korisnik posetilac = (Korisnik)Session["KORISNIK"];
                lista_Grupnih_Treninga = lista_Grupnih_Treninga.FindAll(g => DateTime.Parse(g.DatumVreme) < DateTime.Now && g.SpisakPosetilaca.Contains(posetilac.KorisnickoIme) && g.Obrisan == false);
                TempData["komentar"] = true;
            }

            if (grupni_trening.Naziv == "O")
            {
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.OrderByDescending(g => g.Naziv).ToList();
            }
            else if(grupni_trening.Naziv == "R")
            {
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.OrderBy(g => g.Naziv).ToList();
            }

            if (TipTreninga == "O")
            {
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.OrderByDescending(g => g.TipTreninga).ToList();
            }
            else if (TipTreninga == "R")
            {
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.OrderBy(g => g.TipTreninga).ToList();
            }

            if (grupni_trening.DatumVreme == "O")
            {
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.OrderByDescending(g => DateTime.Parse(g.DatumVreme)).ToList();
            }
            else if (grupni_trening.DatumVreme == "R")
            {
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.OrderBy(g => DateTime.Parse(g.DatumVreme)).ToList();
            }


            return View("PrikazGrupnihTreninga");

        }

        public ActionResult PrijaviSe(string grupni_trening_naziv)
        {         
            List<Grupni_Trening> lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            List<string> grupni_treninzi_posetioca = ((Korisnik)Session["KORISNIK"]).Grupni_Treninzi_Posetilac_Pohadja;
            Grupni_Trening odabrani_Grupni = lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv);

            if (odabrani_Grupni.SpisakPosetilaca.Count == odabrani_Grupni.MaksimumPosetialca || grupni_treninzi_posetioca.Contains(odabrani_Grupni.Naziv))
            {
                TempData["Poruka"] = "Nije Moguce Da Se Prijavite jer Ste Se Vec Prijavili Ili Su Popunjena Sva Mesta";
                lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
                TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => g.FitnesCentar == odabrani_Grupni.FitnesCentar && DateTime.Parse(g.DatumVreme) > DateTime.Now && g.Obrisan == false);
                return View("PrikazGrupnihTreninga");
            }

            Korisnik posetilac = ((List<Korisnik>)HttpContext.Application["KORISNICI"]).FirstOrDefault(k => k.KorisnickoIme == ((Korisnik)Session["KORISNIK"]).KorisnickoIme);
            posetilac.Grupni_Treninzi_Posetilac_Pohadja.Add(grupni_trening_naziv);
            Session["KORISNIK"] = posetilac;
            Pomocna.Upisivanje((List<Korisnik>)HttpContext.Application["KORISNICI"], "Korisnika");

            lista_Grupnih_Treninga.FirstOrDefault(g => g.Naziv == grupni_trening_naziv).SpisakPosetilaca.Add(posetilac.KorisnickoIme);
            Pomocna.Upisivanje(lista_Grupnih_Treninga, "Grupnih_Treninga");

            lista_Grupnih_Treninga = (List<Grupni_Trening>)HttpContext.Application["GRUPNI_TRENINZI"];
            TempData["grupni_treninzi"] = lista_Grupnih_Treninga.FindAll(g => g.FitnesCentar == odabrani_Grupni.FitnesCentar && DateTime.Parse(g.DatumVreme) > DateTime.Now && g.Obrisan == false);

            return View("PrikazGrupnihTreninga");
        }

    }
}
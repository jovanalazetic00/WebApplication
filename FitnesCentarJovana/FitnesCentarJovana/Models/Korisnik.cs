using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentarJovana.Models
{
    public enum POLOVI
    {
        M,
        Z
    }

    public enum ULOGE
    {
        VLASNIK,
        POSETILAC,
        TRENER
    }
   
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public POLOVI Pol { get; set; }
        public string Email { get; set; }
        public string DatumRodjenja { get; set; }
        public ULOGE Uloga { get; set; }
        public List<string> Moji_Fitnes_Centri { get; set; }
        public string Fitnes_Centar_Koji_Trenira { get; set; }
        public List<string> Grupni_Treninzi_Trener_Angazovan { get; set; }
        public List<string> Grupni_Treninzi_Posetilac_Pohadja { get; set; }

        public bool Obrisan { get; set; }
    }
}
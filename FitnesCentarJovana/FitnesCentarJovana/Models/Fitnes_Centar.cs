using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentarJovana.Models
{
    public class Fitnes_Centar
    {
        public bool Obrisan { get; set; }
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        public int GodinaOtvaranja { get; set; }
        public string Vlasnik { get; set; }
        public double CMesecne { get; set; }
        public double CGodisnje { get; set; }
        public double CJednogObicnog { get; set; }
        public double CGrupnog { get; set; }
        public double CJednogPerosnalnog { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentarJovana.Models
{
    public class Komentar
    {
        public bool Obrisan { get; set; }
        public string Posetilac { get; set; }
        public string FitnesCentar { get; set; }
        public string Tekst { get; set; }
        public int Ocena { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnesCentarJovana.Models
{
    public enum TIP
    {
        YOGA,
        LES_MILLS_TONE,
        BODY_PUMP

    }

    public class Grupni_Trening
    {
        public bool Obrisan { get; set; }
        public string Naziv { get; set; }
        public TIP TipTreninga { get; set; }
        public string FitnesCentar { get; set; }
        public int Trajanje { get; set; }
        public string DatumVreme { get; set; }
        public int MaksimumPosetialca { get; set; }
        public List<string> SpisakPosetilaca { get; set; }
    }
}
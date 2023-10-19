using FitnesCentarJovana.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FitnesCentarJovana
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            HttpContext.Current.Application["KORISNICI"] = Pomocna.Ucitaj<Korisnik>("Korisnik");
            HttpContext.Current.Application["FITNES_CENTRI"] = Pomocna.Ucitaj<Fitnes_Centar>("Fitnes_Centar");
            HttpContext.Current.Application["GRUPNI_TRENINZI"] = Pomocna.Ucitaj<Grupni_Trening>("Grupni_Trening");
            HttpContext.Current.Application["KOMENTARI"] = Pomocna.Ucitaj<Komentar>("Komentar");

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}

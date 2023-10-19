using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FitnesCentarJovana.Models
{
    public static class Pomocna
    {
        //generic method
        public static List<T> Ucitaj<T>(string klasa)
        {
            if(klasa == "Korisnik")
            {
                try
                {
                    string ime_fajla = HttpContext.Current.Server.MapPath("/App_Data/KORISNICI.json");
                    string jsonString = File.ReadAllText(ime_fajla);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);
                }
                catch
                {
                    return new List<T>();
                }
              
            }
            else if (klasa == "Fitnes_Centar")
            {
                try
                {
                    string ime_fajla = HttpContext.Current.Server.MapPath("/App_Data/FITNES_CENTRI.json");
                    string jsonString = File.ReadAllText(ime_fajla);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);
                }
                catch
                {
                    return new List<T>();
                }

            }
            else if (klasa == "Grupni_Trening")
            {
                try
                {
                    string ime_fajla = HttpContext.Current.Server.MapPath("/App_Data/GRUPNI_TRENINZI.json");
                    string jsonString = File.ReadAllText(ime_fajla);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);
                }
                catch
                {
                    return new List<T>();
                }
            }
            else if(klasa == "Komentar")
            {
                try
                {
                    string ime_fajla = HttpContext.Current.Server.MapPath("/App_Data/KOMENTARI.json");
                    string jsonString = File.ReadAllText(ime_fajla);
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);
                }
                catch
                {
                    return new List<T>();
                }
            }

            return null;
        }

        public static void Upisivanje<T>(List<T> lista, string cega)
        {
            if (cega == "Korisnika")
            {
                String putanja_do_fajla = HttpContext.Current.Server.MapPath("/App_Data/KORISNICI.json"); 
                File.WriteAllText(putanja_do_fajla, Newtonsoft.Json.JsonConvert.SerializeObject(lista));
                HttpContext.Current.Application["KORISNICI"] = lista;

            }
            else if (cega == "Fitnes_Centra")
            {
                String putanja_do_fajla = HttpContext.Current.Server.MapPath("/App_Data/FITNES_CENTRI.json");
                File.WriteAllText(putanja_do_fajla, Newtonsoft.Json.JsonConvert.SerializeObject(lista));
                HttpContext.Current.Application["FITNES_CENTRI"] = lista;
            }
            else if(cega == "Grupnih_Treninga")
            {
                String putanja_do_fajla = HttpContext.Current.Server.MapPath("/App_Data/GRUPNI_TRENINZI.json");
                File.WriteAllText(putanja_do_fajla, Newtonsoft.Json.JsonConvert.SerializeObject(lista));
                HttpContext.Current.Application["GRUPNI_TRENINZI"] = lista;
            }
            else if (cega == "Komentara")
            {
                String putanja_do_fajla = HttpContext.Current.Server.MapPath("/App_Data/KOMENTARI.json");
                File.WriteAllText(putanja_do_fajla, Newtonsoft.Json.JsonConvert.SerializeObject(lista));
                HttpContext.Current.Application["KOMENTARI"] = lista;
            }
        }
    }
}
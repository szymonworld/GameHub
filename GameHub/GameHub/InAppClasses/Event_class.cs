using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GameHub
{
    public class Event_class
    {
        public int type = 0;
        public string wydarzenie_text;
        public string wydarzenie_tytul;
        public string wydarzenie_czas;
        public string etitle;
        public string efteam;
        public string esteam;
        public string edate;
        public string egame;
        public Event_class(string title, string team1, string team2, string date, string game)
        {
            etitle = title;
            efteam = team1;
            esteam = team2;
            edate = date;
            egame = game;
            type = 0;
            //Czas(godzina, minuta);
        }
        public Event_class(string title, string date, string game)
        {
            etitle = title;
            //desc = description;
            edate = date;
            egame = game;
            //Czas(godzina, minuta);
            type = 1;
        }

        public Event_class(string Text, string tytul, int godzina, int minuta)
        {
            //czy_turniej = czyturniej;
            wydarzenie_text = Text;
            wydarzenie_tytul = tytul;
            Czas(godzina, minuta);
        }

        private void Czas(int godzina, int minuta)
        {
            // wydarzenie_czas = Convert.ToString(godzina) + ":" + Convert.ToString(minuta);
            wydarzenie_czas = Convert.ToString(godzina) + ":" + Convert.ToString(minuta);
        }
    }
}
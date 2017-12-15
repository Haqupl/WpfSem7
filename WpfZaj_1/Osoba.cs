using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfZaj_1
{
    class Osoba
    {
        public Osoba(string Imie, string Nazwisko, int Wiek)
        {
            this.Imie = Imie;
            this.Nazwisko = Nazwisko;
            this.Wiek = Wiek;
        }

        public string Imie { get; private set; }
        public string Nazwisko { get; private set; }
        public int Wiek { get; private set; }
    }
}

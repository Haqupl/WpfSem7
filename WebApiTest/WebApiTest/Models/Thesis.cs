using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiTest.Models
{
    public class Thesis
    {
        public string Nr { get; set; }
        public string Rok { get; set; }
        public string Autor { get; set; }
        public string Tytul { get; set; }
        public string Promotor { get; set; }
        public string Student { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiDHL.Models
{
    public class Aggiornamenti
    {
        public int Aggiornamento_ID { get; set; }
        public string Stato { get; set; }
        public string Luogo { get; set; }
        public string Descrizione { get; set; }
        public DateTime Data_Agg { get; set; }
        public int Spedizione_ID { get; set; }
    }
}
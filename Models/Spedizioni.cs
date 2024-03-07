using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiDHL.Models
{
    public class Spedizioni
    {
        public int Spedizione_ID { get; set; }
        public DateTime Data_Spedizione { get; set; }
        public int Cod_Sped { get; set; }
        public decimal Peso { get; set; }
        public string Citta_Dest { get; set; }
        public string Indirizzo { get; set; }
        public string Destinatario { get; set; }
        public decimal Costo { get; set; }
        public DateTime Data_Prev { get; set; }
        public int Cliente_ID { get; set; }
    }
}
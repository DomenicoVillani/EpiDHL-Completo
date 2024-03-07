using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiDHL.Models
{
    public class Cliente
    {
        [Key]
        public int Cliente_ID { get; set; }

        // Azienda
        [Required(ErrorMessage = "Campo Obbligatorio")]
        [Display(Name = "Nome Azienda")]
        public bool Azienda { get; set; }

        // Codice Fiscale
        [Display(Name = "Codice Fiscale")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Il codice fiscale deve avere 16 caratteri")]
        public string Cod_Fisc {  get; set; }

        // Partita IVA
        [Display(Name = "Partita IVA")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "la partita IVA deve avere 11 caratteri")]
        public string PI { get; set; }

        // E-mail
        [Required(ErrorMessage = "Email Campo Obbligatorio")]
        [Display(Name = "E-mail")]
        [StringLength(255, ErrorMessage = "L'email deve avere massimo 255 caratteri")]
        [RegularExpression(@"^[\w-.]+@([\w-]+.)+[\w-]{2,4}$", ErrorMessage = "Inserisci un indirizzo email valido")]
        public string Email { get; set; }
        

        // Telefono/Cellulare
        [Display(Name = "Telefono/Cellulare")]
        [StringLength(20, MinimumLength = 7, ErrorMessage = "Minimo 7 cifre, massimo 20")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Il campo Tel accetta solo numeri.")]
        public string Tel {  get; set; }

        // Nome
        [Required(ErrorMessage = " Nome Campo Obbligatorio")]
        [Display(Name = "Nome")]
        [StringLength(50, MinimumLength= 2,  ErrorMessage = "il nome deve contenere meno di 50 caratteri")]

        public string Nome { get; set; }

        // Cognome
        [Display(Name = "Cognome")]
        [StringLength(50, MinimumLength= 2, ErrorMessage = "il cognome deve contenere massimo 50 caratteri")]

        public string Cognome { get; set;}

    }
}
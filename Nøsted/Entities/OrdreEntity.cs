using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nøsted.Entities
{

    [Table("Ordre")]
    public class OrdreViewModel
    {
        [Key] [Column("OrdreNr")] public int OrdreNr { get; set; }
        public string Navn { get; set; }
        public int TelefonNr { get; set; }
        public string Epost { get; set; }
        public string Type { get; set; }
        public string Gjelder { get; set; }
        public string Adresse { get; set; }

    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Nøsted.Entities
{

    [Table("ArbeidsDokument")]
    public class ArbeidsDokumentViewModel
    {
        
        public string KundeInfo { get; set; }
        public int Uke { get; set; }
        public DateTime Registrert { get; set; }
        public string Type { get; set; }
        public string Bestilling { get; set; }
        public DateTime AvtaltLevering { get; set; }
        public DateTime ProduktMotatt { get; set; }
        public DateTime AvtaltFerdigStillelse { get; set; }
        public DateTime ServiceFerdig { get; set; }
        public int AnatallTimer { get; set; }
        [Key] [Column("OrdreID")] public int OrdreID { get; set; }
    }
}
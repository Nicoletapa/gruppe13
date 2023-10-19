using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nøsted.Models;

public class ArbeidsDokumentViewModel
{
    [Key]
    public int ArbeidsdokumentID { get; set; }
    public string KundeInfo { get; set; }
    public int Uke { get; set; }
    public DateTime Registrert { get; set; }
    public string Type { get; set; }
    public string Bestilling { get; set; }
    public DateTime AvtaltLevering { get; set; }
    public DateTime ProduktMotatt { get; set; }
    public DateTime AvtaltFerdigstillelse  { get; set; }
    public DateTime ServiceFerdig { get; set; }
    public decimal AntallTimer { get; set; }
    public int OrdreID { get; set; }
    public OrdreViewModel Ordre { get; set; }

}
    

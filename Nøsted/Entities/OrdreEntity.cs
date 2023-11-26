using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nøsted.Models;



[Table("Ordre1")]
public class OrdreViewModel
{
    [Key]
    [ValidateNever]
    public int OrdreNr { get; set; }
    public string Navn { get; set; }
    public int TelefonNr { get; set; }
    public string Adresse { get; set; }
    public string?  Type { get; set; }
    public string Gjelder { get; set; }
    public string Epost { get; set; }
    public int? Uke { get; set; }
    public DateTime Registrert { get; set; }
    public string Bestilling { get; set; }
    public DateTime AvtaltLevering { get; set; }
    public DateTime? ProduktMotatt { get; set; }
    public DateTime? AvtaltFerdigstillelse  { get; set; }
    public DateTime? ServiceFerdig { get; set; }
    public decimal? AntallTimer { get; set; }
    [Range(0, 100, ErrorMessage = "Value for {0} must be between {0} and {1}.")]
    public int Status { get; set; }
    
}
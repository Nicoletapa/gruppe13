using System.ComponentModel.DataAnnotations;

namespace Nøsted.Models;

public class OrdreViewModel
{
    [Key]
    public int OrdreNr { get; set; }
    public string Navn { get; set; }
    public int TelefonNr { get; set; }
    public string Adresse { get; set; }
    public string  Type { get; set; }
    public string Gjelder { get; set; }
    public string Epost { get; set; }
    
}
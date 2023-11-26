using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nøsted.Entities;


[Table("Kategori")]              
public class Kategori
            {
                [Key]
                public int KategoriID { get; set; }
                public string KategoriNavn { get; set; }
            }


[Table("Sjekkpunkt")]
public class Sjekkpunkt 
            {
                [Key] 
                public int SjekkpunktID { get; set; }
                                
                [ValidateNever]
                public string SjekkpunktNavn { get; set; }
                                
                [ForeignKey("KategoriID")]
                public int KategoriID { get; set; } // Foreign key

                public Kategori? Kategori { get; set; }
                                
            }



[Table("SjekklisteSjekkpunkt")]
public class SjekklisteSjekkpunkt
            {
                [Key] 
                [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
                public int SjekklisteSjekkpunktID { get; set; }
                    
                public Guid SjekklisteID { get; set; }
                    
                [ForeignKey("SjekkpunktID")] 
                public int SjekkpunktID { get; set; }
                    
                [ForeignKey("OrdreNr")] 
                public int OrdreNr { get; set; }
                   
                public string? Status { get; set; }
                   
                public Sjekkpunkt sjekkpunkt { get; set; } 
                  
            }


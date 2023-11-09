using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nøsted.Models;


[Table("Sjekkliste")]
public class Sjekkliste
                {
                    [Key]
                    public int SjekklisteID { get; set; }
                    
                    [ForeignKey("OrdreNr")]
                    public int OrdreNr { get; set; }
                    
                    [ValidateNever] // Specify the column name in the database
                    public OrdreViewModel Ordre { get; set; }
                   
                   
                    public IEnumerable<SjekklisteSjekkpunkt>? Sjekkpunkter { get; set; } // Make sure this property is correctly defined
                   
                    
                }
[Table("Kategori")]              
public class Kategori
                {
                    [Key]
                    public int KategoriID { get; set; }
                    public string KategoriNavn { get; set; }
                }

[Table("Sjekkpunkt2")]
public class Sjekkpunkt 
{
                    [Key] 
                    public int SjekkpunktID { get; set; }
                    public string SjekkpunktNavn { get; set; }
                    
                    [ForeignKey("KategoriID")]
                    public int KategoriID { get; set; } // Foreign key

                     public Kategori? Kategori { get; set; }
                     // public IEnumerator GetEnumerator()
                     // {
                     //     throw new NotImplementedException();
                     // }
}



[Table("SjekklisteSjekkpunkt")]
    public class SjekklisteSjekkpunkt
    {
        [Key] 
        public int SjekklisteSjekkpunktID { get; set; }

        [ForeignKey("SjekklisteID")] 
        public int SjekklisteID { get; set; }
        
        [ForeignKey("SjekkpunktID")] 
        public int SjekkpunktID { get; set; }
        
        public string Status { get; set; }
        public Sjekkliste sjekkliste { get; set; } // Navigation property for the related Sjekkliste entity
        public Sjekkpunkt sjekkpunkt { get; set; } // Navigation for Sjekkpunkt 

      
    }



public class CreateSjekklisteSjekkpunktViewModel
{

    // public List<Sjekkliste> sjekkliste { get; set; } //SjeklisteID
    // public List<SjekklisteSjekkpunkt> sjekklisteSjekkpunkt { get; set; } //STAtus
    // public List<Sjekkpunkt> sjekkpunkt { get; set; } //sjekkpunktID / KATEGORIID / SJEKPUNKTNAVN
    // public List<Kategori> kategorier { get; set; }

    public Sjekkliste sjekkliste { get; set; }
    public IEnumerable<Sjekkpunkt> Sjekkpunkter { get; set; }
    public SjekklisteSjekkpunkt sjekklisteSjekkpunkt { get; set; } 
    public Kategori kategori { get; set; }
    public int SjekklisteId { get; set; }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nøsted.Models;



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
                  //  public virtual Kategori Kategori { get; set; } // Navigation property

                    
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
       
        public string Status { get; set; }
       
        public Sjekkpunkt sjekkpunkt { get; set; } // Navigation for Sjekkpunkt 
        

      
    }

// public class SjekkpunktGroup
// {
//     [ValidateNever]
//     public string KategoriNavn { get; set; }
//     [ValidateNever]
//     public int Rowspan { get; set; }
//     [ValidateNever]
//     public List<SjekkpunktWithStatus> Sjekkpunkter { get; set; } // Changed to include status
//
// }

public class SjekkpunktWithStatus
{
    public Sjekkpunkt Sjekkpunkt { get; set; }
    public string Status { get; set; }
}


public class CreateSjekklisteSjekkpunktViewModel
{

   
    [ValidateNever]
    public List<Sjekkpunkt> Sjekkpunkter { get; set; } = new();

    // public List<SjekkpunktGroup> GroupedSjekkpunkter { get; set; } = new();

    [ValidateNever]
    public SjekklisteSjekkpunkt sjekklisteSjekkpunkt { get; set; } 
    [ValidateNever]
    public Kategori kategori { get; set; }
    public Guid SjekklisteId { get; set; }
    public List<SjekkpunktWithStatus> SjekkpunkterWithStatus { get; set; } = new List<SjekkpunktWithStatus>();

    public int OrdreNr { get; set; }
}
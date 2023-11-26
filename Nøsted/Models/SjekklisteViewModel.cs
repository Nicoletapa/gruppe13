using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Nøsted.Entities;

namespace Nøsted.Models;


public class SjekkpunktWithStatus
{
    public Sjekkpunkt Sjekkpunkt { get; set; }
    public string? Status { get; set; }
}



public class SjekkpunktGroup
{
    [ValidateNever]
    public string KategoriNavn { get; set; }
    
    public List<SjekkpunktWithStatus> Sjekkpunkter { get; set; } // Changed to include status

}

public class CreateSjekklisteSjekkpunktViewModel
{

   
    [ValidateNever]
    public List<Sjekkpunkt> Sjekkpunkter { get; set; } = new();
    public List<SjekkpunktGroup> GroupedSjekkpunkter { get; set; } = new List<SjekkpunktGroup>();
    [ValidateNever]
    public SjekklisteSjekkpunkt sjekklisteSjekkpunkt { get; set; } 
    [ValidateNever]
    public Kategori kategori { get; set; }
    public Guid SjekklisteId { get; set; } 
    public List<SjekkpunktWithStatus> SjekkpunkterWithStatus { get; set; } = new List<SjekkpunktWithStatus>();
    [ValidateNever]
    public List<Kategori> Kategorier { get; set; } // Add this line
    public int OrdreNr { get; set; }
} 
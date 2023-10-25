using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Nøsted.Models;

public class SjekklisteViewModel
{
    [Key] public int SjekklisteID { get; set; }
    public List<SjekklisteMekanisk> sjekklisteMekanisk { get; set; }
    public List<SjekklisteHydraulisk> sjekklisteHydraulisk { get; set; }
    public List<SjekklisteElektro> sjekklisteElektro { get; set; }
    public List<SjekklisteFunksjonsTest> SjekklisteFunksjonsTests { get; set; }
    public List<SjekklisteKommentarer> sjekklisteKommentarer { get; set; }
    public List<SjekklisteTrykkSettinger> sjekklisteTrykkSettinger { get; set; }
}


public class SjekklisteMekanisk
{
    [Key]
    public int SjekklisteMekaniskID { get; set; }
    public string SjekkClutchLamellerForSlitasje { get; set; }
    public string SjekkBremserBåndPål { get; set; }
    public string SjekkLagerForTrommel { get; set; }
    public string SjekkPTOOgOpplagring { get; set; }
    public string SjekkKjedeStrammer { get; set; }
    public string SjekkWire { get; set; }
    public string SjekkPinionLager { get; set; }
    public string SjekkKilePåKjedehjul { get; set; }

}

public class SjekklisteHydraulisk
{
    [Key]
    public int SjekklisteHydrauliskID { get; set; }
    public string SjekkHydraulikkSylinderForLekkasje { get; set; }
    public string SjekkSlangerForSkaderOgLekkasje { get; set; }
    public string TestHydraulikkBlokkITestbenk { get; set; }
    public string SkiftOljeITank { get; set; }
    public string SkiftOljePåGirBoks { get; set; }
    public string SjekkRingsylinderÅpneOgSkiftTetninger { get; set; }
    public string SjekkBremseSylinderÅpneOgSkiftTetninger { get; set; }

}

public class SjekklisteElektro
{
    [Key]
    public int SjekklisteElektroID { get; set; }
    public string SjekkLedningsnettPåVinsj { get; set; }
    public string SjekkOgTestRadio { get; set; }
    public string SjekkOgTestKnappekasse { get; set; }

}

public class SjekklisteTrykkSettinger
{
    [Key]
    public int SjekklisteTrykkSettingerID { get; set; }
    public string xx_Bar { get; set; }
}

public class SjekklisteFunksjonsTest
{
    public int SjekklisteFunksjonsTestID { get; set; }
    public string TestVinsjOgKjørAlleFunksjoner { get; set; }
    public string TrekkkraftKN { get; set; }
    public string BremseKraftKN { get; set; }

}
public class SjekklisteKommentarer 
{
    [Key]
    public int SjekklisteKommentarerID { get; set; }
    public string Kommentar { get; set; }


}


 









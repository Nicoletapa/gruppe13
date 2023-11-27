using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Nøsted.Models;



public class OrdreCompletionViewModel
{
    public OrdreEntity Ordre { get; set; }
    public float CompletionPercentage { get; set; }
}


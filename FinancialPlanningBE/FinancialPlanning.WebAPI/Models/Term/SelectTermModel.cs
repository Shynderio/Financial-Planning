

using System.ComponentModel.DataAnnotations;

namespace FinancialPlanning.WebAPI.Models.Term;
public class SelectTermModel
{
    [Required] public Guid Id { get; set; }
    [Required] public string TermName { get; set; } = string.Empty;   
}
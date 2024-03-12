using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinancialPlanning.Data.Entities;

namespace FinancialPlanning.WebAPI.Models.Term;

public class TermListModel
{
    [Required] public Guid Id { get; set; }
    [Required] public string TermName { get; set; } = string.Empty;
    [Required] public int Duration { get; set; }
    [Required] public DateTime StartDate { get; set; }
    [Required] public int Status { get; set; }
}
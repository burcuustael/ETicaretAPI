using System.ComponentModel.DataAnnotations.Schema;
using ETicaret.Domain.Entities.Common;

namespace ETicaret.Domain.Entities;

public class File : BaseEntity
{
    public string FileName { get; set; } 
    public string Path { get; set; } 
    public string Storage { get; set; } 
    [NotMapped]
    public override DateTime UpdateDate {get; set; }
}
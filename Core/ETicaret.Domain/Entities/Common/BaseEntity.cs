namespace ETicaret.Domain.Entities.Common;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedDate {get; set; }
   virtual public DateTime UpdateDate  {get; set; }
}
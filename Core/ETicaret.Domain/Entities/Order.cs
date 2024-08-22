namespace ETicaret.Domain.Entities;

public class Order
{
    public string Description { get; set; }
    public string Adress { get; set; }
    public ICollection<Product> Products { get; set; }
    public Customer Customer { get; set; }
}
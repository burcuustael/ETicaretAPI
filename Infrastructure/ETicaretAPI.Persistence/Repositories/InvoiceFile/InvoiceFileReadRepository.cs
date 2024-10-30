using ETicaret.Domain.Entities;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class InvoiceFileReadRepository : ReadRepository<InvoiceFile>,IInvoiceFileReadRepository
{
    public InvoiceFileReadRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}
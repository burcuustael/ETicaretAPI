using ETicaret.Domain.Entities;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;

namespace ETicaretAPI.Persistence.Repositories;

public class InvoiceFileWriteRepository : WriteRepository<InvoiceFile>, IInvoiceFileWriteRepository
{
    public InvoiceFileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}
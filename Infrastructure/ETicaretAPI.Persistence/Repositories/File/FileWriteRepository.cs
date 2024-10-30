using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using File = ETicaret.Domain.Entities.File;

namespace ETicaretAPI.Persistence.Repositories;

public class FileWriteRepository : WriteRepository<File>, IFileWriteRepository
{
    public FileWriteRepository(ETicaretAPIDbContext context) : base(context)
    {
    }
}
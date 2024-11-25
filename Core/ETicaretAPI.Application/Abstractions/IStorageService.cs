namespace ETicaretAPI.Application.Abstractions;

public interface IStorageService : IStorage
{
    public string StorageName { get; set; } 
}
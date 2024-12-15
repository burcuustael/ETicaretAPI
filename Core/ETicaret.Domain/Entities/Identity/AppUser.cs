using Microsoft.AspNetCore.Identity;

namespace ETicaret.Domain.Entities.Identity;

public class AppUser : IdentityUser<string>
{
    public string NameSurname { get; set; }
}
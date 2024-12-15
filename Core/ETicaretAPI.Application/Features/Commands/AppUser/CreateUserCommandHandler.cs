using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest,CreateUserCommandResponse>
{
    private readonly UserManager<ETicaret.Domain.Entities.Identity.AppUser> _userManager;


    public CreateUserCommandHandler(UserManager<ETicaret.Domain.Entities.Identity.AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
    {
        IdentityResult result = await _userManager.CreateAsync(new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = request.Username,
            Email = request.Email,
            NameSurname = request.NameSurname,
        },request.Password);

        CreateUserCommandResponse response = new() { Succeeded = result.Succeeded };
        
        if (result.Succeeded)
            response.Message = "Kullanıcı başarıyla oluştu";
        
        else
            foreach (var error  in result.Errors)
                response.Message += $"{error.Code}-{error.Description}";
        return response;
    }
}
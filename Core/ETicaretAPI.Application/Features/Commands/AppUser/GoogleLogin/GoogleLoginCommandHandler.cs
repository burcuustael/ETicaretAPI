using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;

public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommandRequest,GoogleLoginCommandResponse>
{
    private readonly UserManager<ETicaret.Domain.Entities.Identity.AppUser> _userManager;
    private readonly ITokenHandler _tokenHandler;
    
    public GoogleLoginCommandHandler(UserManager<ETicaret.Domain.Entities.Identity.AppUser> userManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<GoogleLoginCommandResponse> Handle(GoogleLoginCommandRequest request, CancellationToken cancellationToken)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings()
        {
            Audience = new List<string>{"952543029542-tp8abkgr8hvb8gvr1ri7a0m3ochl8sgu.apps.googleusercontent.com"}
        };

        var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, settings);

        var info = new UserLoginInfo(request.Provider, payload.Subject, request.Provider);

        ETicaret.Domain.Entities.Identity.AppUser user =
            await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        
        bool result = user != null;

        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);
            if (user==null)
            {
                user = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = payload.Email,
                    UserName = payload.GivenName,
                    NameSurname = payload.Name
                };
               
                var identityResult = await _userManager.CreateAsync(user);
                
                if (!identityResult.Succeeded)
                {
                    var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
                    throw new Exception($"Kullanıcı oluşturulamadı: {errors}");
                }

                result = identityResult.Succeeded;
            }
        }

        if (result)
            await _userManager.AddLoginAsync(user, info);
        else
            throw new Exception("Invalid external auth.");

        Token token = _tokenHandler.CreateAccessToken(5);

        return new()
        {
            Token = token
        };
    }
}
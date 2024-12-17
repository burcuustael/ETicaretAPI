using ETicaretAPI.Application.Abstractions.Token;
using ETicaretAPI.Application.DTOs;
using ETicaretAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest,LoginUserCommandResponse>
{
    private readonly UserManager<ETicaret.Domain.Entities.Identity.AppUser> _userManager;
    private readonly SignInManager<ETicaret.Domain.Entities.Identity.AppUser> _signInManager;
    private readonly ITokenHandler _tokenHandler;


    public LoginUserCommandHandler(UserManager<ETicaret.Domain.Entities.Identity.AppUser> userManager, SignInManager<ETicaret.Domain.Entities.Identity.AppUser> signInManager, ITokenHandler tokenHandler)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenHandler = tokenHandler;
    }

    public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
    {
       ETicaret.Domain.Entities.Identity.AppUser user = await _userManager.FindByNameAsync(request.UsernameOrEmail);
       if (user==null)
           user = await _userManager.FindByEmailAsync(request.UsernameOrEmail); 

       if (user == null)
           throw new NotFoundUserException();

       SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

       if (result.Succeeded)
       {
           Token token = _tokenHandler.CreateAccessToken(5);

           return new LoginUserSuccessCommandResponse()
           {
               Token = token
           };
       }
       throw new AuthenticationErrorException();

    }
}
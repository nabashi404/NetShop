using FluentResults;
using NetShop.Models;

namespace NetShop.Interfaces;

public interface IAuthService
{
    Task<Result<LoginResponse>> Login(LoginForm loginForm);
}

using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetShop.Data;
using NetShop.Interfaces;
using NetShop.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetShop.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IOptions<JWTSettings> jwtSettings) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IOptions<JWTSettings> _jwtSettings = jwtSettings;

    public async Task<Result<LoginResponse>> Login(LoginForm loginForm)
    {
        var user = await _userManager.FindByEmailAsync(loginForm.Email);

        if (user == null) return Result.Fail<LoginResponse>("Incorrect email or password");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginForm.Password);

        if (!isPasswordValid) return Result.Fail<LoginResponse>("Incorrect email or password");

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = GenerateJwtToken(claims);

        return Result.Ok(new LoginResponse { Token = token });
    }

    private string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var jwt = _jwtSettings.Value;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwt.Issuer,
            audience: jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

using MadramiShop.Api.Data;
using MadramiShop.Api.Data.Entities;
using MadramiShop.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public class AuthService
{
    private readonly DataContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IConfiguration _configuration;
    public AuthService(DataContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        this._configuration = configuration;
    }

    public async Task<ApiResult> RegisterAsync(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
        {
            return ApiResult.Fail("Email already exists");
        }
        var user = new User
        {
            Email = dto.Email,
            Mobile = dto.Mobile,
            Name = dto.Name
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return ApiResult.Success();
        }
        catch (Exception ex)
        {
            //Log the exception
            //Send some userfriendly message to the client
            return ApiResult.Fail(ex.Message);
        }


    }

    public async Task<ApiResult<LoggedInUser>>LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Username);
        if(user is null)
        {
            return ApiResult<LoggedInUser>.Fail("User does not exist");

        }
        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (verificationResult!=PasswordVerificationResult.Success)
        {
            return ApiResult<LoggedInUser>.Fail("Incorrect password");
        }

        //generate JWT token
        var jwt = GenerateToken(user);
        var loggedInUser = new LoggedInUser(user.Id, user.Name, user.Email, jwt);
        return ApiResult<LoggedInUser>.Success(loggedInUser);
    }
    private string GenerateToken(User user)
    {
        Claim[] claims = [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email)
            ];

        var secretKey = _configuration.GetValue<string>("Jwt:SecretKey");
        var securityKey = System.Text.Encoding.UTF8.GetBytes(secretKey);
        var symetricKey = new SymmetricSecurityKey(securityKey);

        var signinCreds = new SigningCredentials(symetricKey, SecurityAlgorithms.HmacSha256);
        var expireInMinutes = _configuration.GetValue<int>("Jwt:ExpireInMinutes");

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("Jwt: Issuer"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireInMinutes),
            signingCredentials: signinCreds
            );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}

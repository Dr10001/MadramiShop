using MadramiShop.Api.Data;
using MadramiShop.Api.Data.Entities;
using MadramiShop.Shared.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService
{
    private readonly DataContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserService(DataContext context, IPasswordHasher<User> passwordHasher)
    {
        this._context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResult> SaveAddressAsync(AddressDto dto, int userId)
    {
        UserAddress? userAddress = null;
        if (dto.Id == 0)
        {
            userAddress = new UserAddress
            {
                Address = dto.Address,
                IsDefault = dto.IsDefault,
                Id = dto.Id,
                Name = dto.Name,
                UserId = userId
            };
            _context.UserAddresses.Add(userAddress);
        }
        else 
        {
            userAddress = await _context.UserAddresses.FindAsync(dto.Id);
            if(userAddress == null) 
            {
                return ApiResult.Fail("Invalid request");
            }
            userAddress.Address = dto.Address;
            userAddress.Name = dto.Name;
            userAddress.IsDefault = dto.IsDefault;
            _context.UserAddresses.Update(userAddress);
        }
        try
        {
            if (dto.IsDefault)
            {
                var defaultAdress = await _context.UserAddresses
                    .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault && a.Id != dto.Id);//recheck logic
                if (defaultAdress is not null)
                {
                    defaultAdress.IsDefault = false;
                }
            }
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
    public async Task<AddressDto[]> GetAddressesAsync(int userId) =>
        await _context.UserAddresses
                .AsNoTracking()
                .Where(a => a.Id == userId)
                .Select(a => new AddressDto
                {
                    Id = a.Id,
                    Address = a.Address,
                    IsDefault = a.IsDefault,    
                    Name = a.Name
                })
                .ToArrayAsync();

    public async Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto, int UserId)
    {
        try
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
                return ApiResult.Fail("User does not exist");
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if (verificationResult != PasswordVerificationResult.Success)
                ApiResult.Fail("Incorrect password");
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return ApiResult.Success();
        }
        catch (Exception ex) 
        { 
            return ApiResult.Fail(ex.Message);
        }
    }

}

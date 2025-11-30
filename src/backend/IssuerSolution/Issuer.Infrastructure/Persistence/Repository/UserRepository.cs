using System;
using Issuer.Core.Data;
using Issuer.Core.Interfaces;
using Issuer.Infrastructure.Model;
using Microsoft.EntityFrameworkCore;

namespace Issuer.Infrastructure.Repository;

public class UserRepository : IUserRepository
{
    // user db
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddUser(User user)
    {
        try
        {
            // adding user
            await _context.AddAsync(user);
            return true;
        } catch (DbUpdateException)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string email)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (user == null) throw new Exception("User not found");

            _context.Users.Remove(user);

            return true;
        } catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<User> FindUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> UpdatePasswordAsync(User user, string hashed_password)
    {
        var findUser = await _context.Users.FindAsync(user);

        findUser.UpdatePassword(hashed_password);
    
        
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

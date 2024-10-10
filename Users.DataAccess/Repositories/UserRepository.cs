﻿using Microsoft.EntityFrameworkCore;
using Users.Domain.Models;

namespace Users.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserById(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
    }
   
    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
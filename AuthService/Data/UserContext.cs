using Microsoft.EntityFrameworkCore;
using AuthService.Models;
using System.Collections.Generic;

namespace AuthService.Data;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ElVisionLibrary.Models.Identity;

namespace ElVision.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : IdentityDbContext<User>(options)
    {
       public DbSet<User> Users { get; set; }
    }
}

using WebT.Models;
using System.Data.Entity;

namespace WebT.Data
{
    public class UserRelContext : DbContext
    {
            public UserRelContext()
                : base("DbConnection")
            { }

            public DbSet<User> Users { get; set; }

            public DbSet<Order> Orders { get; set; }
    }
}
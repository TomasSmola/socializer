using Microsoft.EntityFrameworkCore;
using socializer.Data.Domain.Entities;

namespace socializer.Data.Sqlite;

public class DataContext : DbContext
{
    public DbSet<SocialConnection> SocialConnections { get; set; }

    public DbSet<Dataset> Datasets { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
}

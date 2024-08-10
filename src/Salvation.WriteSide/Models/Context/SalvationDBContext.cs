using Microsoft.EntityFrameworkCore;
using Salvation.WriteSide.Models.Entities;

namespace Salvation.WriteSide.Models.Context;

public class SalvationDBContext : DbContext
{
    public SalvationDBContext(DbContextOptions<SalvationDBContext> options) : base(options)
    {
    }

    public DbSet<MainTable> MainTables { get; set; }
}

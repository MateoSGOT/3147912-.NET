using Crud_Nativo.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud_Nativo.Data
{
    public class ApplicationDbContext : DbContext
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { }
       public DbSet<Producto> Producto { get; set; }

    }
}

using DatingApp.API.Model;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DataContext:DbContext
    {
       public DataContext(DbContextOptions<DataContext>option):base(option){}   
       public DbSet<value> values { get; set; }
       public DbSet<User> Users{get;set;}
    }
}
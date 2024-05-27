using AccessManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Context
{
    public class ContetxtDb:DbContext
    {
        public ContetxtDb(DbContextOptions<ContetxtDb> options):base(options)
        {
            
        }
        public DbSet<Userregister> Userregisters { get; set; }

    }
}

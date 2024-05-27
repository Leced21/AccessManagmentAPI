using AccessManagmentAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessManagmentAPI.Context
{
    public class ContetxtDb:DbContext
    {
        public ContetxtDb(DbContextOptions dbContextOptions):base(dbContextOptions)
        {
            
        }
        DbSet<Userregister> userregisters { get; set; }

    }
}

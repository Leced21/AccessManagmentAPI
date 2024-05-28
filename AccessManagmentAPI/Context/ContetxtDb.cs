using AccessManagmentAPI.Models;
using AccessManagmentAPI.Repos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AccessManagmentAPI.Context
{
    public partial class ContetxtDb:DbContext
    {
        public ContetxtDb(DbContextOptions<ContetxtDb> options):base(options)
        {
            
        }

        public virtual DbSet<TblCustomer> TblCustomers { get; set; }

        public virtual DbSet<TblMenu> TblMenus { get; set; }

        public virtual DbSet<TblOtpManager> TblOtpManagers { get; set; }

        public virtual DbSet<TblProduct> TblProducts { get; set; }

        public virtual DbSet<TblProductimage> TblProductimages { get; set; }
        public virtual DbSet<TblPwdManger> TblPwdMangers { get; set; }

        public virtual DbSet<TblRefreshtoken> TblRefreshtokens { get; set; }

        public virtual DbSet<TblRole> TblRoles { get; set; }

        public virtual DbSet<TblRolepermission> TblRolepermissions { get; set; }

        public virtual DbSet<TblTempuser> TblTempusers { get; set; }

        public virtual DbSet<TblUser> TblUsers { get; set; }
        public virtual DbSet<Customermodal> customerdetail { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblTempuser>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("tbl_tempuser1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}

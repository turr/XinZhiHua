using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class MyDbContext : DbContext
{
    public MyDbContext()
        : base("Default")
    {
    }

    public DbSet<Manager> manager_data { get; set; }
    public DbSet<Setting> setting_data { get; set; }
    public DbSet<NewAndProduct> new_product_data { get; set; }



    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
    }
}

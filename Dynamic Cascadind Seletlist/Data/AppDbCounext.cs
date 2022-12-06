namespace Dynamic_Cascadind_Seletlist.Data
{
    public class AppDbCounext : DbContext
    {
        public AppDbCounext(DbContextOptions<AppDbCounext> options) : base(options)
        {

        }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
    }
}

namespace WebAPIwJWT.Models
{
    using System.Data.Entity;

    public partial class TestJWTModel : DbContext
    {
        public TestJWTModel()
            : base("name=TestJWTModel")
        {
        }

        public virtual DbSet<Socio> Socio { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Socio>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);
        }
    }
}

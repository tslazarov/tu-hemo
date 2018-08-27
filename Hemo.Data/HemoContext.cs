using Hemo.Data.Contracts;
using Hemo.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hemo.Data
{
    public class HemoContext : DbContext, IContext
    {
        public HemoContext() : base("Hemo")
        {
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<DonationsCenter> DonationsCenters { get; set; }

        public virtual IDbSet<DonationsRequest> DonationsRequests { get; set; }

        public virtual IDbSet<Donator> Donators { get; set; }

        public static DbContext Create()
        {
            return new HemoContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(9, 6));

            Database.SetInitializer<HemoContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        IDbSet<T> IContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}

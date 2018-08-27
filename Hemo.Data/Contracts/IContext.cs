using Hemo.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Hemo.Data.Contracts
{
    public interface IContext
    {
        IDbSet<User> Users { get; set; }

        IDbSet<DonationsCenter> DonationsCenters { get; set; }

        IDbSet<DonationsRequest> DonationsRequests { get; set; }

        IDbSet<Donator> Donators { get; set; }

        IDbSet<T> Set<T>() where T : class;

        DbEntityEntry<T> Entry<T>(T entity) where T : class;

        int SaveChanges();
    }
}

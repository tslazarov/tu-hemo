using Hemo.Models;

namespace Hemo.Data.Contracts
{
    public interface IData
    {
        IEfRepository<User> UsersRepository { get; }

        IEfRepository<DonationsCenter> DonationsCentersRepository { get; }

        IEfRepository<DonationsRequest> DonationsRequestsRepository { get; }

        IEfRepository<Donator> DonatorsRepository { get; }

        IEfRepository<UsersDonationTracking> UsersDonationTrackingsRepository { get; }

        void SaveChanges();
    }
}

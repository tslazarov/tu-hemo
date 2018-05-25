﻿using Bytes2you.Validation;
using Hemo.Data.Contracts;
using Hemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemo.Data
{
    public class HemoData : IData
    {
        private readonly IContext dbContext;
        private readonly IEfRepository<User> usersRepository;
        private readonly IEfRepository<DonationsCenter> donationsCentersRepository;
        private readonly IEfRepository<DonationsRequest> donationsRequestsRepository;
        private readonly IEfRepository<Donator> donatorsRepository;
        private readonly IEfRepository<UsersDonationTracking> usersDonationTrackingsRepository;

        public HemoData(IContext dbContext,
                        IEfRepository<User> usersRepository,
                        IEfRepository<DonationsCenter> donationsCentersRepository,
                        IEfRepository<DonationsRequest> donationsRequestsRepository,
                        IEfRepository<Donator> donatorsRepository,
                        IEfRepository<UsersDonationTracking> usersDonationTrackingsRepository)
        {
            Guard.WhenArgument<IContext>(dbContext, "Database context cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<User>>(usersRepository, "Users repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<DonationsCenter>>(donationsCentersRepository, "Donations center repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<DonationsRequest>>(donationsRequestsRepository, "Donations request repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<Donator>>(donatorsRepository, "Donators repository cannot be null.")
                .IsNull()
                .Throw();

            Guard.WhenArgument<IEfRepository<UsersDonationTracking>>(usersDonationTrackingsRepository, "Users donation trackings repository cannot be null.")
                .IsNull()
                .Throw();

            this.dbContext = dbContext;
            this.usersRepository = usersRepository;
            this.donationsCentersRepository = donationsCentersRepository;
            this.donationsRequestsRepository = donationsRequestsRepository;
            this.donatorsRepository = donatorsRepository;
            this.usersDonationTrackingsRepository = usersDonationTrackingsRepository;
        }

        public IEfRepository<User> UsersRepository
        {
            get
            {
                return this.usersRepository;
            }
        }

        public IEfRepository<DonationsCenter> DonationsCentersRepository
        {
            get
            {
                return this.donationsCentersRepository;
            }
        }

        public IEfRepository<DonationsRequest> DonationsRequestsRepository
        {
            get
            {
                return this.donationsRequestsRepository;
            }
        }

        public IEfRepository<Donator> DonatorsRepository
        {
            get
            {
                return this.donatorsRepository;
            }
        }

        public IEfRepository<UsersDonationTracking> UsersDonationTrackingsRepository
        {
            get
            {
                return this.usersDonationTrackingsRepository;
            }
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }
    }
}

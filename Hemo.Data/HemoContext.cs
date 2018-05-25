﻿using Hemo.Data.Contracts;
using Hemo.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Hemo.Data
{
    class HemoContext : DbContext, IContext
    {
        public HemoContext() : base("Hemo")
        {
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<DonationsCenter> DonationsCenters { get; set; }

        public virtual IDbSet<DonationsRequest> DonationsRequests { get; set; }

        public virtual IDbSet<Donator> Donators { get; set; }

        public virtual IDbSet<UsersDonationTracking> UsersDonationTrackings { get; set; }

        public static DbContext Create()
        {
            return new HemoContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        IDbSet<T> IContext.Set<T>()
        {
            return base.Set<T>();
        }
    }
}

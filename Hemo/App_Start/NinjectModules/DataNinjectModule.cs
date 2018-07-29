using Hemo.Assembly;
using Hemo.Data;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
using Hemo.Data.Managers;
using Hemo.Models;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject.Web.Common;

namespace Hemo.App_Start.NinjectModules
{
    public class DataNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(x => x.FromAssemblyContaining<IDataAssembly>().SelectAllClasses().BindDefaultInterface());

            this.Rebind<IContext>().To<HemoContext>().InRequestScope();
            this.Rebind<IData>().To<HemoData>();
            this.Bind<IDonationsCentersFactory>().ToFactory();
            this.Bind<IDonationsRequestsFactory>().ToFactory();
            this.Bind<IDonatorsFactory>().ToFactory();
            this.Bind<IUsersDonationTrackingsFactory>().ToFactory();
            this.Bind<IUsersFactory>().ToFactory();

            this.Rebind<IUsersManager>().To<UsersManager>().InRequestScope();
            this.Rebind<IDonationsCentersManager>().To<DonationsCentersManager>().InRequestScope();
            this.Rebind<IDonationsRequestsManager>().To<DonationsRequestsManager>().InRequestScope();
            this.Rebind<IDonatorsManager>().To<DonatorsManager>().InRequestScope();
            this.Rebind<IUsersDonationTrackingsManager>().To<UsersDonationTrackingsManager>().InRequestScope();


            this.Rebind<IEfRepository<User>>().To<EfRepository<User>>().InRequestScope();
            this.Rebind<IEfRepository<DonationsCenter>>().To<EfRepository<DonationsCenter>>().InRequestScope();
            this.Rebind<IEfRepository<DonationsRequest>>().To<EfRepository<DonationsRequest>>().InRequestScope();
            this.Rebind<IEfRepository<Donator>>().To<EfRepository<Donator>>().InRequestScope();
            this.Rebind<IEfRepository<UsersDonationTracking>>().To<EfRepository<UsersDonationTracking>>().InRequestScope();
        }
    }
}
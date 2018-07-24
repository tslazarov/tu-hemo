using Hemo.Assembly;
using Hemo.Data;
using Hemo.Data.Contracts;
using Hemo.Data.Factories;
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
        }
    }
}
using Hemo.SendGrid.Assembly;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Factory;
using Ninject.Modules;
using Ninject.Web.Common;


namespace Hemo.App_Start.NinjectModules
{
    public class SendGridNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(x => x.FromAssemblyContaining<ISendGridAssembly>().SelectAllClasses().BindDefaultInterface());

            //this.Rebind<IContext>().To<HemoContext>().InRequestScope();
        }
    }
}
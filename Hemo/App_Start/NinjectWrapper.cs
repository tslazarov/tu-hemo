using Hemo.App_Start;
using Hemo.Data.Contracts;
using Ninject;

namespace Hemo.App_Start
{
    public class NinjectWrapper
    {
        public IUsersManager UsersManager { get; set; }

        public NinjectWrapper()
        {
            this.UsersManager = NinjectWebCommon.Bootstrapper.Kernel.Get<IUsersManager>();
        }
    }
}
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemo
{
    public static class IoCContainer
    {
        private static IKernel kernel;

        public static IKernel Kernel
        {
            get
            {
                if (kernel == null)
                {
                    kernel = new StandardKernel();
                }

                return kernel;
            }
        }
    }
}
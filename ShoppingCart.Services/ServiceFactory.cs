using Ninject;
using Ninject.Modules;
using ShoppingCart.Services.Services;
using ShoppingCart.Services.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Services
{
    public static class ServiceFactory
    {
        static IKernel kernel = new StandardKernel(new ShoppingCartModule());
        static public Interface GetA<Interface>()
        {
            return kernel.Get<Interface>();
        }

    }

    class ShoppingCartModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISampleService>().To<SampleService>();
            Bind<ICatalogueServices>().To<CatalogueServices>();
            Bind<ICartOperationsServices>().To<CartOperationsServices>();
        }
    }

}

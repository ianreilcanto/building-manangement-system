using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;
using Umbraco.Web;

namespace MSD.SlattoFS.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            //register all controllers found in this assembly
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //register umbraco webapi controllers used by the admin site
            //builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);

            //register the types
            builder.RegisterType<object>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
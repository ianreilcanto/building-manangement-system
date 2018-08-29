using Umbraco.Core;

namespace MSD.SlattoFS.Handlers
{
    public class BMEventHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            BMDatabaseInitializer.Run(applicationContext);
            BMApplicationInitializer.Run(applicationContext);
            BMRouteConfiguration.Initialize(applicationContext);
            //BMCustomRoutes.RegisterRoutes();
        }
    }
}
using System.Web.Http;

namespace Eldert.EventGrid.Subscriber
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes 
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(name: "routes", routeTemplate: "api/{controller}/{action}");
        }
    }
}

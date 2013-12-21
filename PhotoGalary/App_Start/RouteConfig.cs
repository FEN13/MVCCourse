using System.Web.Mvc;
using System.Web.Routing;

namespace PhotoGalery
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//Redirect all requests to home page
			routes.MapRoute("Default", "{*url}", new { controller = "Home", action = "Index" });

			//routes.MapRoute(
			//	name: "Default",
			//	url: "{controller}/{action}/{id}",
			//	defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			//);
		}
	}
}
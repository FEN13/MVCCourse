using System.Web.Optimization;

namespace PhotoGalery
{
	public class BundleConfig
	{
		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/scripts/vendor")
				.Include("~/Scripts/jquery/jquery-{version}.js")
                .Include("~/Scripts/jquery/jquery.maskedinput-1.3.1.js")
                .Include("~/Scripts/jquery/jquery.fancybox-pack.js")
                .Include("~/Scripts/jquery/jquery.fancybox.js")
                .Include("~/Scripts/jquery/jquery.numeric.js")
                .Include("~/Scripts/knockout/knockout-{version}.js")
				.Include("~/Scripts/modernizr-*")
                .Include("~/Scripts/bootstrap/bootstrap.js")
                .Include("~/Scripts/bootstrap/bootstrap-datepicker.js")
                .Include("~/Scripts/toastr/toastr.js")
				.Include("~/Scripts/store.js"));

			bundles.Add(new StyleBundle("~/Content/css")
				.Include("~/Content/bootstrap/bootstrap.css")
				.Include("~/Content/bootstrap/bootstrap-theme.css")
                .Include("~/Content/bootstrap/bootstrap-datepicker.css")
				.Include("~/Content/font-awesome.css")
				.Include("~/Content/durandal.css")
				.Include("~/Content/toastr.css")
                .Include("~/Content/fancyboxStyle/jquery.fancybox.css")
				.Include("~/Content/site.css"));

		}
	}
}
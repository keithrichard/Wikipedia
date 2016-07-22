using System.Web.Optimization;

namespace Wikipedia
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/Toaster/main/javascript/Jquery.toastmessage.js",
                        "~/Scripts/jquery-ui-1.12.0.min.js"));
            

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(                      
                      "~/Scripts/bootstrap.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(                      
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/jquery.ui.autocomplete.css",
                      "~/Scripts/Toaster/main/resources/css/jquery.toastmessage.css"));
            

        }
    }
}

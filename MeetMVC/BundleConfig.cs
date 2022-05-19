using System.Web;
using System.Web.Optimization;

namespace MeetMVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/mdb.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                  "~/Content/bootstrap.css",
                                  "~/Content/site.css",
                                  "~/Content/mdb.min.css"));
        }

    }
}

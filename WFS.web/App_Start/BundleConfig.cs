using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WFS.web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region LayoutBundles

            bundles.Add(new StyleBundle("~/Content/layout/base/css")
            .Include("~/assets/vendors/select2/select2.min.css", new CssRewriteUrlTransform())
            .Include("~/assets/css/demo_1/style.css", new CssRewriteUrlTransform())
            .Include("~/assets/vendors/core/core.css", new CssRewriteUrlTransform()) 
            .Include(   
            "~/assets/vendors/datatables.net-bs4/dataTables.bootstrap4.css",
            "~/assets/vendors/bootstrap-datepicker/bootstrap-datepicker.min.css",
            "~/assets/vendors/tempusdominus-bootstrap-4/tempusdominus-bootstrap-4.min.css",
                
            "~/assets/vendors/jquery-tags-input/jquery.tagsinput.min.css", 
            "~/assets/vendors/font-awesome/css/font-awesome.min.css",
            "~/assets/fonts/feather-font/css/iconfont.css",
            "~/assets/vendors/jquery-steps/jquery.steps.css",
            "~/assets/vendors/sweetalert2/sweetalert2.min.css",
            "~/assets/vendors/simplemde/simplemde.min.css",
            "~/Content/toastr.css",
            "~/Content/toastr.min.css")
            );

            bundles.Add(new StyleBundle("~/Content/layout/base/css2")
            .Include("~/assets/vendors/select2/select2.min.css", new CssRewriteUrlTransform())
            .Include("~/assets/css/demo_2/style.css", new CssRewriteUrlTransform())
            .Include("~/assets/vendors/core/core.css", new CssRewriteUrlTransform())
            .Include(
            "~/assets/vendors/datatables.net-bs4/dataTables.bootstrap4.css",
            "~/assets/vendors/bootstrap-datepicker/bootstrap-datepicker.min.css",
            "~/assets/vendors/tempusdominus-bootstrap-4/tempusdominus-bootstrap-4.min.css",

            "~/assets/vendors/jquery-tags-input/jquery.tagsinput.min.css",
            "~/assets/vendors/font-awesome/css/font-awesome.min.css",
            "~/assets/fonts/feather-font/css/iconfont.css",
            "~/assets/vendors/jquery-steps/jquery.steps.css",
            "~/assets/vendors/sweetalert2/sweetalert2.min.css",
            "~/assets/vendors/simplemde/simplemde.min.css",
            "~/Content/toastr.css",
            "~/Content/toastr.min.css")
            );

            bundles.Add(new ScriptBundle("~/Content/layout/base/js").Include(
            "~/assets/vendors/core/core.js",

            "~/assets/vendors/chartjs/Chart.min.js",
            "~/assets/vendors/jquery-validation/jquery.validate.min.js",
            "~/assets/vendors/bootstrap-maxlength/bootstrap-maxlength.min.js", 
            "~/assets/vendors/inputmask/jquery.inputmask.bundle.min.js",
            "~/assets/vendors/select2/select2.min.js",
            "~/assets/vendors/simplemde/simplemde.min.js",
            "~/assets/vendors/typeahead.js/typeahead.bundle.min.js", 
            "~/assets/vendors/jquery-tags-input/jquery.tagsinput.min.js",
            "~/assets/vendors/jquery.flot/jquery.flot.js",
            "~/assets/vendors/jquery.flot/jquery.flot.resize.js",
            "~/assets/vendors/bootstrap-datepicker/bootstrap-datepicker.min.js",
            "~/assets/vendors/apexcharts/apexcharts.min.js",
            "~/assets/vendors/progressbar.js/progressbar.min.js", 
            "~/assets/vendors/datatables.net/jquery.dataTables.js",
            "~/assets/vendors/datatables.net-bs4/dataTables.bootstrap4.js",
            "~/assets/vendors/feather-icons/feather.min.js",
            "~/assets/vendors/sweetalert2/sweetalert2.min.js",
            "~/assets/vendors/moment/moment.min.js",
            "~/assets/vendors/tempusdominus-bootstrap-4/tempusdominus-bootstrap-4.js",
            "~/assets/vendors/simplemde/simplemde.min.js",

            "~/assets/js/template.js",
            "~/assets/js/bootstrap-maxlength.js", 
            "~/assets/js/dashboard.js",
            "~/assets/js/select2.js", 
            "~/assets/js/datepicker.js",  
            "~/assets/js/chartjs.js",
            "~/assets/js/inputmask.js",
            "~/assets/js/tags-input.js", 
            "~/assets/js/chat.js", 
            "~/assets/js/data-table.js",
            "~/assets/js/simplemde.js",
            "~/assets/js/email.js",  
            "~/Scripts/toastr.min.js"  
            ));
            #endregion 

            #region register js
            bundles.Add(new ScriptBundle("~/Content/register/jquery").Include(
            "~/assets/vendors/core/core.js",


            "~/assets/vendors/jquery-validation/jquery.validate.min.js",
            "~/assets/vendors/bootstrap-maxlength/bootstrap-maxlength.min.js",
            "~/assets/vendors/inputmask/jquery.inputmask.bundle.min.js",
            "~/assets/vendors/select2/select2.min.js",
            "~/assets/vendors/typeahead.js/typeahead.bundle.min.js",
            "~/assets/vendors/jquery-tags-input/jquery.tagsinput.min.js",
            "~/assets/vendors/moment/moment.min.js",
            "~/assets/vendors/jquery-steps/jquery.steps.min.js",
            "~/assets/vendors/feather-icons/feather.min.js",
            "~/assets/vendors/sweetalert2/sweetalert2.min.js", 
            "~/assets/vendors/tempusdominus-bootstrap-4/tempusdominus-bootstrap-4.js",

            "~/assets/vendors/feather-icons/feather.min.js",
            "~/assets/js/wizard.js",
            "~/assets/js/template.js",
            
            "~/assets/js/bootstrap-maxlength.js",
            "~/assets/js/inputmask.js",
            "~/assets/js/select2.js",
            "~/assets/js/typeahead.js",
            "~/assets/js/tags-input.js"
            ));
            #endregion

            #region HomeLayoutBundles
            bundles.Add(new StyleBundle("~/Content/Homelayout/base/css").Include(
            "~/assets/vendors/bootstrap-datepicker/bootstrap-datepicker.min.css",
            "~/OPassets/lib/font-awesome/css/font-awesome.min.css",
            "~/OPassets/lib/animate/animate.min.css",
            "~/OPassets/lib/ionicons/css/ionicons.min.css",
            "~/OPassets/lib/owlcarousel/assets/owl.carousel.min.css",
            "~/OPassets/lib/lightbox/css/lightbox.min.css", 
            "~/OPassets/css/style.css").Include(
            "~/OPassets/lib/bootstrap/css/bootstrap.min.css",new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/Content/Homelayout/base/js").Include(
            "~/OPassets/lib/jquery/jquery.min.js",
            "~/OPassets/lib/jquery/jquery-migrate.min.js",
            "~/OPassets/lib/bootstrap/js/bootstrap.bundle.min.js",
            "~/OPassets/lib/easing/easing.min.js",
            "~/OPassets/lib/superfish/hoverIntent.js",
            "~/OPassets/lib/superfish/superfish.min.js",
            "~/OPassets/lib/wow/wow.min.js",
            "~/OPassets/lib/waypoints/waypoints.min.js",
            "~/OPassets/lib/counterup/counterup.min.js",
            "~/OPassets/lib/owlcarousel/owl.carousel.min.js",
            "~/OPassets/lib/isotope/isotope.pkgd.min.js",
            "~/OPassets/lib/lightbox/js/lightbox.min.js",
            "~/OPassets/lib/touchSwipe/jquery.touchSwipe.min.js",
            "~/OPassets/contactform/contactform.js", 
            "~/OPassets/js/main.js"));
            #endregion

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

        }
    }
}
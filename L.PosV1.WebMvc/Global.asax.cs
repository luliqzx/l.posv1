using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using L.PosV1.WebMvc.App_Start;
using L.PosV1.WebMvc.Controllers;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using L.PosV1.DataAccess.Common;
using L.PosV1.DataAccess.Repo;
using System.Reflection;
using SimpleInjector.Integration.Web.Mvc;

namespace L.PosV1.WebMvc
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrap();
        }

        void Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            //container.Register(() => SessionProvider.SessionFactory, Lifestyle.Singleton);
            //container.Register<ISessionFactory>(() => SessionProvider.SessionFactory, Lifestyle.Singleton);
            //container.Register<ISessionFactory>(() => SessionProvider.SessionFactory2, Lifestyle.Singleton);

            container.Register<ISessionProvider, SessionProvider>(Lifestyle.Singleton);

            //var assemblies = new[] { SessionProvider.CreateSessionFactory("mssqlserverConn"), SessionProvider.CreateSessionFactory("mssqlserverConn2") };
            //container.RegisterCollection<ISessionFactory>(assemblies);
            //container.Register(typeof(IBaseRepo<>), new[] { typeof(BaseRepo<>).Assembly });
            container.Register<IUserRepo, UserRepo>(Lifestyle.Singleton);
            //container.Register<ICurrencyRepo, CurrencyRepo>(Lifestyle.Scoped);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            //IUserRepo repo = new UserRepo();
            //repo.GetOne();
            ////IList<User> lstUser = repo.GetAll();

            //ICurrencyRepo CurrencyRepo = new CurrencyRepo();
            //IList<Currency> lstCurrency = CurrencyRepo.GetAll();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpContext = ((HttpApplication)sender).Context;
            httpContext.Response.Clear();
            httpContext.ClearError();

            if (new HttpRequestWrapper(httpContext.Request).IsAjaxRequest())
            {
                return;
            }

            ExecuteErrorController(httpContext, exception as HttpException);
        }

        private void ExecuteErrorController(HttpContext httpContext, HttpException exception)
        {
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";

            if (exception != null && exception.GetHttpCode() == (int)HttpStatusCode.NotFound)
            {
                routeData.Values["action"] = "NotFound";
            }
            else
            {
                routeData.Values["action"] = "InternalServerError";
            }

            using (Controller controller = new ErrorController())
            {
                ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
            }
        }
    }
}
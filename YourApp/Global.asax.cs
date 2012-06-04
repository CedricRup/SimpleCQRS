using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Infrastructure;
using Infrastructure.Events;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Services.Handlers;
using Services.Queries;
using YourDomain;

namespace YourApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var documentStore = new DocumentStore { Url = "http://localhost:8080/databases/CQRSSimpleExample" };
            documentStore.Initialize();

            IndexCreation.CreateIndexes(typeof(UserByName).Assembly, documentStore);

            var builder = new ContainerBuilder();

            builder.RegisterInstance(documentStore).As<IDocumentStore>();
            builder.RegisterType<AccountHandler>().AsSelf();
            builder.RegisterType<UserHandler>().AsSelf();
            builder.RegisterGeneric(typeof (Repository<>)).As(typeof (IRepository<>));
            builder.RegisterType<UserQueries>().AsSelf();
            builder.Register(t => documentStore.OpenSession()).As<IDocumentSession>();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();

            var bus = new SimpleBus(container.Resolve);
            bus.RegisterHandlers(typeof(AccountHandler).Assembly);

            builder = new ContainerBuilder();
            builder.RegisterInstance(bus).AsImplementedInterfaces();
            builder.Update(container);

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using TodoApp.Models;
using ExoModel;
using System.Reflection;
using ExoModel.EntityFramework;

namespace TodoApp
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
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

			RegisterRoutes(RouteTable.Routes);

			Database.SetInitializer<TodoContext>(new DropCreateDatabaseIfModelChanges<TodoContext>());

			new ModelContextProvider().CreateContext += (source, args) =>
			{
				Assembly coreAssembly = typeof(MvcApplication).Assembly;
				args.Context = new ModelContext(new EntityFrameworkModelTypeProvider(() => new TodoContext()));

				ExoRule.Rule.RegisterRules(coreAssembly);
			};

			if (!TodoContext.Current.Priorities.Any())
			{
				var priority = ModelContext.Create<Priority>();
				priority.Sequence = 1;
				priority.Name = "High";

				priority = ModelContext.Create<Priority>();
				priority.Sequence = 2;
				priority.Name = "Medium";

				priority = ModelContext.Create<Priority>();
				priority.Sequence = 3;
				priority.Name = "Low";

				TodoContext.Current.SaveChanges();
			}
		}
	}
}
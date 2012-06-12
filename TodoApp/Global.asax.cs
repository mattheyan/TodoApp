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
using System.Web.Security;
using System.Security.Principal;
using System.Configuration;

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

		private string ConnectionDescription(ConnectionStringSettings connection)
		{
			return connection.Name + " = " + connection.ConnectionString.Substring(0, 25);
		}

		private void FailWithConnectionStrings()
		{
			var error = @"Connection strings are not properly configured: " +
				string.Join(",", ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>().Select(ConnectionDescription));
			throw new ApplicationException(error);
		}

		protected void Application_Start()
		{
			FailWithConnectionStrings();

			AreaRegistration.RegisterAllAreas();

			RegisterRoutes(RouteTable.Routes);

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

		void Application_PostAuthenticateRequest(object sender, EventArgs e)
		{
			HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				string encTicket = authCookie.Value;
				if (!String.IsNullOrEmpty(encTicket))
				{
					FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encTicket);

					var user = TodoContext.Current.Users.Where(p => p.OpenId == ticket.UserData).First();

					GenericPrincipal prin = new GenericPrincipal(user, null);

					HttpContext.Current.User = prin;
				}
			}
		}
	}
}
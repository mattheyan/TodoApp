using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Web.Security;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using TodoApp.Models;
using DotNetOpenAuth.Messaging;
using System.ComponentModel.DataAnnotations;
using ExoModel;

namespace TodoApp.Controllers
{
	public class UserController : Controller
	{
		private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

		public ActionResult Index()
		{
			if (HttpContext.User.Identity.IsAuthenticated)
				return RedirectToAction("Index", "Home");

			return View();
		}

		[HttpGet]
		public ActionResult LogOff()
		{
			Session.Abandon();
			FormsAuthentication.SignOut();

			return RedirectToAction("Index", "User");
		}

		[ValidateInput(false)]
		public ActionResult Authenticate()
		{
			string provider = Request.Form["openid_identifier"];

			var response = openid.GetResponse();
			if (response == null)
			{
				Identifier id = null;
				if (Identifier.TryParse(provider, out id))
				{
					// Redirect user to provider for authentication
					try
					{
						var request = openid.CreateRequest(id);
						var fields = new ClaimsRequest()
						{
							Email = DemandLevel.Require,
							FullName = DemandLevel.Require,
							Nickname = DemandLevel.Require
						};
						request.AddExtension(fields);

						var fetch = new FetchRequest();
						fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
						fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
						fetch.Attributes.AddRequired(WellKnownAttributes.Name.Alias);
						fetch.Attributes.AddRequired(WellKnownAttributes.Name.FullName);
						request.AddExtension(fetch);

						return request.RedirectingResponse.AsActionResult();
					}
					catch (ProtocolException e)
					{
						ViewData["Message"] = e.Message;
					}
				}
			}
			else
			{
				// Handle response from provider
				switch (response.Status)
				{
					case AuthenticationStatus.Authenticated:
						var claimedIdentifier = response.ClaimedIdentifier.ToString();
						var user = TodoContext.Current.Users.Where(u => u.OpenId == claimedIdentifier).FirstOrDefault();

						if (user == null)
						{
							var claim = response.GetExtension<ClaimsResponse>();
							var fetch = response.GetExtension<FetchResponse>();

							// Create the new user
							user = ModelContext.Create<User>();
							user.Name = fetch.Attributes[WellKnownAttributes.Name.First].Values[0];
							user.OpenId = claimedIdentifier;

							// Add a default list to the new user
							var defaultList = ModelContext.Create<List>();
							defaultList.Name = "My First List";
							user.Lists.Add(defaultList);

							// Add a default item to the default list 
							var defaultItem = ModelContext.Create<ListItem>();
							defaultItem.DateCreated = DateTime.Now;
							defaultItem.DueDate = DateTime.Now;
							defaultItem.Description = "Be Productive!";
							defaultList.Items.Add(defaultItem);

							// Save the new user
							TodoContext.Current.SaveChanges();
						}

						FormsAuthenticationTicket authTicket = new
							FormsAuthenticationTicket(1, //version
							response.ClaimedIdentifier, // user name
							DateTime.Now,             //creation
							DateTime.Now.AddMinutes(30), //Expiration
							false, //Persistent
							user.OpenId);

						string encTicket = FormsAuthentication.Encrypt(authTicket);

						this.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

						return RedirectToAction("Index", "Home");
					case AuthenticationStatus.Canceled:
						break;
					case AuthenticationStatus.Failed:
						break;
				}
			}

			return RedirectToAction("Index", "Home");
		}
	}
}

using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace ASPMVCProducts
{
	public class TokenValidationAttribute : System.Web.Http.Filters.ActionFilterAttribute
	{
		public const string AUTH_TOKEN_HEADER = "Authorization-Token"; 
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			string token;

			try
			{
				token = actionContext.Request.Headers.GetValues(AUTH_TOKEN_HEADER).First();
			}
			catch (Exception)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
				{
					Content = new StringContent("Missing Authorization-Token")
				};
				return;
			}

			try
			{
				var lMembership = (SimpleMembershipProvider)Membership.Provider;
				var lUserName =  RSA.Decrypt(token);
				if (lMembership.GetUser(lUserName, false) == null)
				{
					actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
					{
						Content = new StringContent("Unauthorized User")
					};
					return;
				}
				base.OnActionExecuting(actionContext); 
			}
			catch (Exception)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
				{
					Content = new StringContent("Unauthorized User")
				};
				return;
			}
		}
	}
}
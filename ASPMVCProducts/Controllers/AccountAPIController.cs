using ASPMVCProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Security;
using WebMatrix.WebData;

namespace ASPMVCProducts.Controllers
{
	public class AccountAPIController : ApiController
	{
		/* MUST HAVE A LOOK AT THIS
		 * http://codebetter.com/johnvpetersen/2012/04/02/making-your-asp-net-web-apis-secure/
		 * 
		 */
		ProductsDb m_tDb = new ProductsDb();
		public class RegisterUserDTO
		{
			public string UserName { get; set; }
			public string Password { get; set; }
		}

		public class UserDTO
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}

		// POST api/accountapi/register
		[HttpPost]
		[ActionName("register")]
		public HttpResponseMessage Register([FromBody]RegisterUserDTO aUser)
		{
			var lMembership = (SimpleMembershipProvider)Membership.Provider;
			
			if (lMembership.GetUser(aUser.UserName, false) == null)
			{
				lMembership.CreateUserAndAccount(aUser.UserName, aUser.Password);
				var lUserProfile = m_tDb.UserProfiles.FirstOrDefault(aUserProfile => aUserProfile.UserName == aUser.UserName);
				if (lUserProfile != null)
					return this.Request.CreateResponse<UserDTO>(HttpStatusCode.OK, new UserDTO { Id = lUserProfile.UserId, Name = lUserProfile.UserName });
			}
			return this.Request.CreateResponse(HttpStatusCode.NotModified); 
		}

		// POST api/accountapi
		[HttpPost]
		[ActionName("login")]
		public HttpResponseMessage Login([FromBody]RegisterUserDTO aUser)
		{
			var lUserProfile = m_tDb.UserProfiles.FirstOrDefault(aUserProfile => aUserProfile.UserName == aUser.UserName);
			if (lUserProfile != null && WebSecurity.Login(aUser.UserName, aUser.Password))
				return this.Request.CreateResponse<UserDTO>(HttpStatusCode.OK, new UserDTO { Id = lUserProfile.UserId, Name = lUserProfile.UserName });

			return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
		}

		protected override void Dispose(bool disposing)
		{
			if (m_tDb != null)
			{
				m_tDb.Dispose();
				m_tDb = null;
			}
			base.Dispose(disposing);
		}
	}
}

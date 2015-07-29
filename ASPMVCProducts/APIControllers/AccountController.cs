using ASPMVCProducts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Security;
using WebMatrix.WebData;

namespace ASPMVCProducts.APIControllers
{
    [APIVersionCheckActionFilter]
    public class AccountController : ApiController
    {
        ProductsDb m_tDb = new ProductsDb();
        public class LoginRegisterDTO
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
        public HttpResponseMessage Register([FromBody]LoginRegisterDTO aUser)
        {
            var lMembership = (SimpleMembershipProvider)Membership.Provider;
            var lVersion = this.Request.Headers.GetValues ( Resources.API_VERSION_HEADER ).FirstOrDefault();
            

            if (lMembership.GetUser(aUser.UserName, false) == null)
            {
                lMembership.CreateUserAndAccount(aUser.UserName, aUser.Password);
                var lUserProfile = m_tDb.UserProfiles.FirstOrDefault(aUserProfile => aUserProfile.UserName == aUser.UserName);
                if (lUserProfile != null && WebSecurity.Login(aUser.UserName, aUser.Password))
                {
                    var lResponse = this.Request.CreateResponse<UserDTO>(HttpStatusCode.OK, new UserDTO { Id = lUserProfile.UserId, Name = lUserProfile.UserName });
                    FormsAuthenticationTicket lTicket = new FormsAuthenticationTicket(aUser.UserName, false, 30);
                    var lCookieHeader = new CookieHeaderValue(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(lTicket));
                    lResponse.Headers.AddCookies(new CookieHeaderValue[] { lCookieHeader });
                    return lResponse;
                }
            }
            return this.Request.CreateResponse(HttpStatusCode.NotModified);
        }

        // POST api/accountapi
        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage Login([FromBody]LoginRegisterDTO aUser)
        {
            var lUserProfile = m_tDb.UserProfiles.FirstOrDefault(aUserProfile => aUserProfile.UserName == aUser.UserName);
            if (lUserProfile != null && WebSecurity.Login(aUser.UserName, aUser.Password))
            {
                var lResponse = this.Request.CreateResponse<UserDTO>(HttpStatusCode.OK, new UserDTO { Id = lUserProfile.UserId, Name = lUserProfile.UserName });
                FormsAuthenticationTicket lTicket = new FormsAuthenticationTicket(aUser.UserName, false, 30);
                var lCookieHeader = new CookieHeaderValue(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(lTicket));
                lResponse.Headers.AddCookies(new CookieHeaderValue[] { lCookieHeader });
                return lResponse;
            }
            return this.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        [HttpPost]
        [ActionName("logout")]
        [Authorize]
        public HttpResponseMessage Logout()
        {
            WebSecurity.Logout();
            var lResponse = this.Request.CreateResponse(HttpStatusCode.OK);
            return lResponse;
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

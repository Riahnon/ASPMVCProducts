using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace ASPMVCProducts
{
    public class APIVersionCheckActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //If no API version header is present or is not valid a Forbidden status code will be returned

            IEnumerable<string> lVersionHeaders = null;

            if( !actionContext.ControllerContext.Request.Headers.TryGetValues(Resources.API_VERSION_HEADER, out lVersionHeaders) ||lVersionHeaders == null)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            
            
            var lVersion = lVersionHeaders.FirstOrDefault();
            if (string.IsNullOrEmpty(lVersion) || lVersion != Resources.API_VERSION_VALUE)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden));
            
            base.OnActionExecuting(actionContext);
        }
    }
}
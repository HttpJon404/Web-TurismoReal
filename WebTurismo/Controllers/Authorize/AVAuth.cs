using EntidadServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTurismo.Controllers.Authorize
{
    public class AVAuth : AuthorizeAttribute
    {
        public String Page { get; set; }
        public bool MantenerImpersonificacionRTACliente { get; set; }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            CommonAuthentication(filterContext);
        }

        public string GetIEBrowserMode(AuthorizationContext filterContext)
        {
            string mode = "";
            string userAgent = filterContext.HttpContext.Request.UserAgent; //entire UA string
            string browser = filterContext.HttpContext.Request.Browser.Type; //Browser name and Major Version #
            if (userAgent.Contains("Trident/5.0"))
            { //IE9 has this token
                if (browser == "IE7")
                {
                    mode = "IE9 Vista Compatibilidad";
                }
                else
                {
                    mode = "IE9 Estándar";
                }
            }
            else if (userAgent.Contains("Trident/4.0"))
            { //IE8 has this token
                if (browser == "IE7")
                {
                    mode = "IE8 Vista Compatibilidad";
                }
                else
                {
                    mode = "IE8 Estándar";
                }
            }
            else if (!userAgent.Contains("Trident"))
            { //Earlier versions of IE do not contain the Trident token
                mode = browser;
            }
            return mode;
        }

        public bool ValidateBrowser(AuthorizationContext filterContext)
        {
            try
            {
                String browserType = GetIEBrowserMode(filterContext).ToUpper();
                if (browserType.Contains("IE8") || browserType.Contains("IE7") || browserType.Contains("IE6"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Valida primero que el browser no sea muy antiguo, se descartan versiones anteriores o iguales a IE8.
        /// Luego se valida si el usuario se ha logeado, si no lo está se le envía devuelta al login. Si esta logeado
        /// se valida que tenga privilegios para acceder a la página
        /// </summary>
        /// <param name="filterContext">Contiene informacion del Request HTTP</param>
        public void CommonAuthentication(AuthorizationContext filterContext)
        {
            try
            {
                if (ValidateBrowser(filterContext))
                {
                    HttpSessionStateBase Session = filterContext.HttpContext.Session;
                    AvUsuario user = (AvUsuario)Session["AVUsuario"];
                    String referrerUri = filterContext.RequestContext.HttpContext.Request.Url.PathAndQuery;
                    if (user == null)
                    {
                        filterContext.Result = new RedirectResult("~/Home/Index");
                        return;
                    }
                    else
                    {
                        if (Page != null && !("").Equals(Page) && !user.Paginas.Contains(Page))
                        {
                            filterContext.Result = new RedirectResult("~/Home/Index");
                            return;
                        }
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Auth/ErrorNav");
                    return;
                }
            }
            catch (Exception e)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }
            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}

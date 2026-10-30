using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;


namespace MVCWithSAML.Controllers
{
    public static class AuthTypes
    {
        public const string Cookie = CookieAuthenticationDefaults.AuthenticationType;
        public const string Saml2 = "Saml2";
    }

    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl = "/")
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);
            }

            var props = new AuthenticationProperties { RedirectUri = returnUrl };
            HttpContext.GetOwinContext().Authentication.Challenge(props, AuthTypes.Saml2);
             // Add this using directive at the top of the file

            // Replace the line causing the error in the Login method:
            HttpContext.GetOwinContext().Authentication.Challenge(props, AuthTypes.Saml2);

            // With the following code:
            var owinContext = HttpContext.GetOwinContext();
            owinContext.Authentication.Challenge(props, AuthTypes.Saml2);
            return new HttpUnauthorizedResult();
        }

        [Authorize]
        public ActionResult SignedIn()
        {
            var principal = User as ClaimsPrincipal;
            var email = principal?.FindFirst(ClaimTypes.Email)?.Value
                        ?? principal?.FindFirst("Email Address")?.Value
                        ?? principal?.Identity?.Name
                        ?? User.Identity.Name;

            var firstName = principal?.FindFirst(ClaimTypes.GivenName)?.Value
                            ?? principal?.FindFirst("First name")?.Value
                            ?? string.Empty;

            var lastName = principal?.FindFirst(ClaimTypes.Surname)?.Value
                           ?? principal?.FindFirst("Last name")?.Value
                           ?? string.Empty;

            var displayName = string.Join(" ", new[] { firstName, lastName }.Where(x => !string.IsNullOrWhiteSpace(x)));
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = email;
            }

            ViewBag.Email = email;
            ViewBag.DisplayName = displayName;
            ViewBag.Claims = principal?.Claims.Select(c => c.Type + " = " + c.Value).ToList();

            return View();
        }

        [AllowAnonymous]
        public ActionResult LoginFailure(string message = "SSO login failed.")
        {
            ViewBag.Message = message;
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(
                AuthTypes.Cookie,
                AuthTypes.Saml2);

            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
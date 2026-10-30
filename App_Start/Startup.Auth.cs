using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Owin;

[assembly: OwinStartup(typeof(SamlMvcSample.Startup))]

namespace SamlMvcSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                LoginPath = new PathString("/Account/Login")
            });

            app.UseSaml2Authentication(BuildSaml2Options());
        }


        private static Saml2AuthenticationOptions BuildSaml2Options()
        {
            var publicOrigin = ConfigurationManager.AppSettings["Saml:PublicOrigin"];
            var spEntityId = ConfigurationManager.AppSettings["Saml:SpEntityId"];
            var modulePath = ConfigurationManager.AppSettings["Saml:ModulePath"];
            var idpEntityId = ConfigurationManager.AppSettings["Saml:IdpEntityId"];
            var metadataPath = ConfigurationManager.AppSettings["Saml:IdpMetadataPath"];
            var signingCertPath = ConfigurationManager.AppSettings["Saml:SigningCertPath"];
            var signingCertPassword = ConfigurationManager.AppSettings["Saml:SigningCertPassword"];

            var spOptions = new SPOptions
            {
                EntityId = new EntityId(spEntityId),
                PublicOrigin = new Uri(publicOrigin),
                ModulePath = modulePath
            };

            var certPhysicalPath = HostingEnvironment.MapPath(signingCertPath);
            if (!string.IsNullOrWhiteSpace(certPhysicalPath) && File.Exists(certPhysicalPath))
            {
                var cert = string.IsNullOrEmpty(signingCertPassword)
                    ? new X509Certificate2(certPhysicalPath)
                    : new X509Certificate2(certPhysicalPath, signingCertPassword);

                spOptions.ServiceCertificates.Add(cert);
            }

            var options = new Saml2AuthenticationOptions(false)
            {
                SPOptions = spOptions
            };

            var metadataPhysicalPath = HostingEnvironment.MapPath(metadataPath);
            var idp = new IdentityProvider(new EntityId(idpEntityId), spOptions)
            {
                MetadataLocation = metadataPhysicalPath,
                LoadMetadata = true,
                AllowUnsolicitedAuthnResponse = true
            };

            options.IdentityProviders.Add(idp);
            return options;
        }

        
    }
}
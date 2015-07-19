namespace ASPMVCProducts.Migrations
{
    using ASPMVCProducts.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using WebMatrix.WebData;

    internal sealed class Configuration : DbMigrationsConfiguration<ASPMVCProducts.Models.ProductsDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ASPMVCProducts.Models.ProductsDb context)
        {
            SeedMembership();
            var lDefaultOwner = context.UserProfiles.Where(aUserProfile => aUserProfile.UserName == "gcastro").FirstOrDefault();
            var lRnd = new Random();
        }

        private void SeedMembership()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
            }
            var lRoles = (SimpleRoleProvider)Roles.Provider;
            var lMembership = (SimpleMembershipProvider)Membership.Provider;

            if (lMembership.GetUser("gcastro", false) == null)
            {
                lMembership.CreateUserAndAccount("gcastro", "gcastro");
            }

        }

        private const string APLHANUMERIC_CHARS_STR = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        private static string _GenerateRandomString(int aLength = -1, Random aRnd = null)
        {
            if (aRnd == null)
                aRnd = new Random();
            if (aLength == -1)
                aLength = aRnd.Next(10) + 5;

            var lResultChars = new char[aLength];

            for (int i = 0; i < lResultChars.Length; i++)
            {
                lResultChars[i] = APLHANUMERIC_CHARS_STR[aRnd.Next(APLHANUMERIC_CHARS_STR.Length)];
            }

            return new String(lResultChars);
        }
    }
}

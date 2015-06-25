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
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ASPMVCProducts.Models.ProductsDb context)
        {
            SeedMembership();
            var lDefaultOwner = context.UserProfiles.Where( aUserProfile => aUserProfile.UserName == "gcastro" ).FirstOrDefault();
            ProductCategory[] lProductCategories = null;
            var lRnd = new Random();
            if (context.ProductCategories.Count() == 0 && lDefaultOwner != null)
            {
                lProductCategories = new ProductCategory[lRnd.Next(10) + 5];
                for (int i = 0; i < lProductCategories.Length; ++i)
                {
                    lProductCategories[i] = new ProductCategory()
                    {
                        Name = _GenerateRandomString(10, lRnd),
                        Description = _GenerateRandomString(25, lRnd),
                        Owner = lDefaultOwner
                    };
                }
            }
            else
            {
                lProductCategories = context.ProductCategories.ToArray();
            }
            if (context.Products.Count() == 0 && lDefaultOwner != null)
            {
                var lProducts = new Product[lRnd.Next(10) + 5];
                for (int i = 0; i < lProducts.Length; ++i)
                {
                    //Amount of categories that the product will have
                    var lCategoryCnt = lRnd.Next(lProductCategories.Length + 1);
                    List<ProductCategory> lCategories = new List<ProductCategory>(lCategoryCnt);
                    //Categories that the product belongs to are randomly fetched
                    while(lCategories.Count < lCategoryCnt)
                    {
                        var lCategory = lProductCategories[lRnd.Next(lProductCategories.Length)];
                        if (!lCategories.Contains(lCategory))
                        {
                            if (lCategory.Products == null)
                                lCategory.Products = new List<Product>();

                            lCategories.Add(lCategory);
                        }
                    }
                    //Product is created
                    lProducts[i] = new Product()
                    {
                        Name = _GenerateRandomString(10, lRnd),
                        Description = _GenerateRandomString(25, lRnd),
                        Owner = lDefaultOwner,
                        Categories = lCategories
                    };
                    //Product is added to product list of the categories it belongs to
                    foreach (var lCategory in lCategories)
                    {
                        lCategory.Products.Add(lProducts[i]);
                    }
                }

                context.Products.AddOrUpdate(lProducts);
                context.ProductCategories.AddOrUpdate(lProductCategories);

            }

            
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);

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

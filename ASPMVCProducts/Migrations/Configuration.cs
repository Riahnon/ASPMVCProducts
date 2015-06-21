namespace ASPMVCProducts.Migrations
{
    using ASPMVCProducts.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ASPMVCProducts.Models.ProductsDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ASPMVCProducts.Models.ProductsDb context)
        {
            var lRnd = new Random();
            var lProducts = new Product[10];
            for (int i = 0; i < lProducts.Length; ++i)
            {
                lProducts[i] = new Product()
                {
                    Name = _GenerateRandomString(10, lRnd),
                    Description = _GenerateRandomString(25, lRnd)
                };
            }
            //  This method will be called after migrating to the latest version.
            context.Products.AddOrUpdate(lProducts);
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

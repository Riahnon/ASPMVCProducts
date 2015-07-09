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
				var lDefaultOwner = context.UserProfiles.Where(aUserProfile => aUserProfile.UserName == "gcastro").FirstOrDefault();
				var lRnd = new Random();
				Product[] lProducts = null;
				if (context.Products.Count() == 0)
				{
					lProducts = new Product[lRnd.Next(10) + 5];
					for (int i = 0; i < lProducts.Length; ++i)
					{
						//Product is created
						lProducts[i] = new Product()
						{
							Name = _GenerateRandomString(10, lRnd)
						};
					}

					context.Products.AddOrUpdate(lProducts);
				}
				else
				{
					lProducts = context.Products.ToArray();
				}


				if (lDefaultOwner != null && context.ProductLists.Count(aList => aList.Owner.UserId == lDefaultOwner.UserId) == 0)
				{
					var lProductList = new ProductList()
					{
						Name = "Default List",
						Owner = lDefaultOwner,
						Products = new List<ProductEntry>(),
					};

					var lProductCount = 3 + lRnd.Next(lProducts.Length - 3);
					var lProductsInList = lProducts.ToList();
					while (lProductsInList.Count > lProductCount)
						lProductsInList.RemoveAt(lRnd.Next(lProductsInList.Count));

					for (int i = 0; i < lProductsInList.Count; ++i)
					{
						var lEntry =  new ProductEntry()
						{
							Checked = false,
							Product = lProductsInList[i],
							Ammount = 0,
							List = lProductList,
						};
						lProductList.Products.Add(lEntry);
					}
					context.ProductLists.AddOrUpdate(lProductList);
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

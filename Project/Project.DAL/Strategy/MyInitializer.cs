using Project.DAL.Context;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DAL.Strategy
{
   public  class MyInitializer:CreateDatabaseIfNotExists<MyDBContext>
    {
        

        protected override void Seed(MyDBContext context)
        {
            



            #region AppUserAdmin
            
            AppUser ap = new AppUser();
            ap.UserName = "cihan";
            ap.Role = MODEL.Enums.Role.Admin;
            ap.Password = "1234";
            ap.IsActive = true;
            ap.Email = "cihanavci@gmail.com";
            #endregion

            #region AppUserDetailAdmin
            

            context.AppUsers.Add(ap);
            context.SaveChanges();
            

            AppUserDetail apd = new AppUserDetail();
            apd.FirstName = "Cihan";
            apd.LastName = "Avcı";
            apd.Address = "Bakırköy";
            apd.BirthDate = new DateTime(1994, 14, 3);
            apd.ID = ap.ID;

            context.AppUserDetails.Add(apd);
            context.SaveChanges();
            #endregion


            #region NormalUser
            
            for (int i = 0; i < 20; i++)
            {
                AppUser apMember = new AppUser();
                apMember.UserName = FakeData.NameData.GetFirstName();
                apMember.Password = FakeData.TextData.GetAlphaNumeric(8);
                apMember.IsActive = true;
                apMember.Email = FakeData.TextData.GetAlphabetical(6) + "@gmail.com";

                context.AppUsers.Add(apMember);
                context.SaveChanges();

                AppUserDetail apdMember = new AppUserDetail();
                apdMember.FirstName = FakeData.NameData.GetFirstName();
                apdMember.LastName = FakeData.NameData.GetSurname();
                apdMember.Address = FakeData.PlaceData.GetAddress();
                apdMember.ID = apMember.ID;
                apdMember.BirthDate = FakeData.DateTimeData.GetDatetime();

                context.AppUserDetails.Add(apdMember);
                context.SaveChanges();



            }
            #endregion


            

            for (int i = 0; i < 10; i++)
            {
                Category c = new Category();
                c.CategoryName = FakeData.NameData.GetFirstName();

                c.Description = FakeData.TextData.GetSentence();

                context.Categories.Add(c);

                for (int x = 0; x < 50; x++)
                {
                    Product p = new Product();
                    p.ProductName = FakeData.NameData.GetFirstName();
                    p.UnitPrice = FakeData.NumberData.GetNumber(10,50);
                    p.UnitsInStock =Convert.ToInt16( FakeData.NumberData.GetNumber(10, 100));
                    p.ImagePath = "/Pictures/49438975_2427802897234445_3907616440525520896_o.jpg";
                    c.Products.Add(p);
                }

            }
            context.SaveChanges();
        }



    }
}

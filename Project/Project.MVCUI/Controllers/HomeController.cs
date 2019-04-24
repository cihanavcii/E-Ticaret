using Projec.TOOLS.MyTools;
using Project.BLL.RepositoryPattern.ConcreteRepository;
using Project.DAL.Context;
using Project.MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Login()
        {
            AppUser girecekOlan = CheckCookie();
            if (girecekOlan == null) return View();
           

            return View(girecekOlan);
        }


        [HttpPost]
        
        public ActionResult Login(AppUser item, string Hatirla)
        {
            if (apUser_repository.Any(x => x.UserName == item.UserName && x.Role == MODEL.Enums.Role.Admin && x.Status != MODEL.Enums.DataStatus.Deleted))
            {
                

                AppUser girisYapan = apUser_repository.Default(x => x.UserName == item.UserName && x.Role == MODEL.Enums.Role.Admin);

                bool sonuc = Crypto.VerifyHashedPassword(girisYapan.Password, item.Password);

                if (sonuc)
                {
                    HatirlaKontrol(item, Hatirla);

                    Session["admin"] = girisYapan;

                    return RedirectToAction("CategoryList", "Category", new { area = "AdminSpecial" });
                }


            }
            else if (apUser_repository.Any(x => x.UserName == item.UserName && x.Role == MODEL.Enums.Role.Member))
            {
                AppUser girisYapanUye = apUser_repository.Default(x => x.UserName == item.UserName);

                bool sonuc = Crypto.VerifyHashedPassword(girisYapanUye.Password, item.Password);

                if (sonuc)
                {
                    HatirlaKontrol(item, Hatirla);
                    Session["member"] = girisYapanUye;
                    return RedirectToAction("ShoppingList","Member"); 
                }
                
            }



            return View();
        }

        AppUserRepository apUser_repository = new AppUserRepository();

        AppUserDetailRepository apDetail_repository = new AppUserDetailRepository();


       

        


        private void HatirlaKontrol(AppUser item, string Hatirla)
        {
            

            if (Hatirla == "true")
            {
                HttpCookie girisIsim = new HttpCookie("userName"); 
                girisIsim.Expires = DateTime.Now.AddMinutes(5);
                girisIsim.Value = item.UserName;
                

                Response.Cookies.Add(girisIsim);

                HttpCookie girisSifre = new HttpCookie("password");
                girisSifre.Expires = DateTime.Now.AddMinutes(5);
                girisSifre.Value = item.Password;
                Response.Cookies.Add(girisSifre);
            }
        }




       

        private AppUser CheckCookie()
        {
            string userName = string.Empty, password = string.Empty;

            AppUser cookiedeSaklanan = null;


            if (Request.Cookies["userName"] != null && Request.Cookies["password"] != null)
            {
                userName = Request.Cookies["userName"].Value;
                password = Request.Cookies["password"].Value;
            }

            
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                cookiedeSaklanan = new AppUser
                {
                    UserName = userName,
                    Password = password
                };
            }
            return cookiedeSaklanan;

        }


       





        public ActionResult Register(Guid? id)
        {

            
            if (id != null)
            {

                if (apUser_repository.Any(x => x.ActivationCode == id))
                {
                    AppUser aktifedilecek = Session["register"] as AppUser;
                    aktifedilecek.IsActive = true;

                    apUser_repository.Update(aktifedilecek); 

                    TempData["Tebrikler"] = "Tebrikler basarılı bir sekilde hesabınız aktif olmustur";


                }
                else
                {
                    
                    ViewBag.KullaniciYok = "Kullanıcı olarak giriş yapmadınız";
                }

            }
            else if (Session["register"] != null && id == null)
            {
                if ((Session["register"] as AppUser).IsActive == false)
                {
                    ViewBag.AktifDegil = "Hesabınızı aktif etmemişsiniz";
                }
            }

            return View();
        }

        [HttpPost]

        public ActionResult Register([Bind(Prefix = "Item1")] AppUser item1, [Bind(Prefix = "Item2")] AppUserDetail item2)
        {
            if (apUser_repository.Any(x => x.UserName == item1.UserName))
            {
                ViewBag.ZatenVar = "Böyle bir kullanıcı zaten kayıtlıdır";
                return View();
            }

            

            string gonderilecekMesaj = "Tebrikler hesabınız olusturulmustur. Hesabınızı aktif etmek icin http://localhost:60181/Home/Register/" + item1.ActivationCode + " linkine tıklayabilirsiniz.";

            #region MailGondermeIslemi
            
            #endregion

            item1.Password = Crypto.HashPassword(item1.Password); 
            apUser_repository.Add(item1);
            item2.ID = item1.ID;

            Session["register"] = apUser_repository.GetByID(item1.ID); 
            apDetail_repository.Add(item2);
            return RedirectToAction("RegisterOk");
        }


        public ActionResult RegisterOk()
        {
            return View();
        }










    }
}
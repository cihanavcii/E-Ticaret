using Project.BLL.RepositoryPattern.ConcreteRepository;
using Project.MODEL.Entities;
using Project.MVCUI.Models.MyEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class MemberController : Controller
    {
        ProductRepository product_repo = new ProductRepository();
        CategoryRepository cat_repo = new CategoryRepository();
        
        public ActionResult ShoppingList()
        {
            return View(Tuple.Create(product_repo.SelectActives(), cat_repo.SelectActives()));
        }


        public ActionResult SelecyByCategory(int id)
        {
            List<Product> byCategory = product_repo.Where(x => x.CategoryID == id);

            return View(Tuple.Create(byCategory, cat_repo.SelectActives()));
        }


        public ActionResult ProductDetail(int id)
        {
            List<Category> byCategory = cat_repo.SelectActives();
            Product secilenUrun = product_repo.GetByID(id);
            return View(Tuple.Create(byCategory, secilenUrun));
        }



        public ActionResult AddToCart(int id)
        {
            if (Session["member"] == null)
            {
                TempData["uyeYok"] = "Lütfen sepete ürün eklemeden önce üye olun";
                return RedirectToAction("Login", "Home");
            }

            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;

            Product eklenecekUrun = product_repo.GetByID(id);

            CartItem ci = new CartItem();
            ci.ID = eklenecekUrun.ID;
            ci.Name = eklenecekUrun.ProductName;
            ci.Price = eklenecekUrun.UnitPrice;
            c.SepeteEkle(ci);


            Session["scart"] = c;


            return RedirectToAction("ShoppingList");
        }


        public ActionResult CartPage()
        {
            if (Session["scart"] != null)
            {
                Cart c = Session["scart"] as Cart;
                return View(c);
            }
            TempData["message"] = "Sepetinizde ürün bulunmamaktadır";
            return RedirectToAction("ShoppingList");
        }


        public ActionResult RemoveFromCart(int id)
        {
            Cart c = Session["scart"] as Cart;

            c.SepettenSil(id);
            if (c.Sepetim.Count < 1)
            {
                Session.Remove("scart");

            }
            return RedirectToAction("CartPage");
        }

        


        public ActionResult SiparisVer()
        {
            if (Session["member"] != null)
            {
                AppUser mevcutKullanici = Session["member"] as AppUser;

                TempData["Kullanici"] = mevcutKullanici;
                
                return View();
            }

            TempData["uyeYok"] = "Üye olmadan sepete ürün ekleyemezsiniz.";
            return RedirectToAction("Login", "Home");

         }

        OrderRepository ord_repo = new OrderRepository();
        OrderDetailRepository detail_repo = new OrderDetailRepository();
        [HttpPost]

        public ActionResult SiparisVer(Order item)
        {
            if (TempData["Kullanici"]==null)
            {
                return RedirectToAction("Login", "Home");
            }

            AppUser kullanici = TempData["Kullanici"] as AppUser;


            



            item.AppUserID = kullanici.ID;

            ord_repo.Add(item);

           
            

            int sonSiparisID = ord_repo.GetLastAdded();

            

            Cart sepet = Session["scart"] as Cart;

            foreach (CartItem urun in sepet.Sepetim)
            {
                OrderDetail od = new OrderDetail();
                od.OrderID = sonSiparisID;
                od.ProductID = urun.ID;
                od.TotalPrice = urun.SubTotal;
                od.Quantity = urun.Amount;
                detail_repo.Add(od);
            }

            return RedirectToAction("ShoppingList");
        }






    }
}
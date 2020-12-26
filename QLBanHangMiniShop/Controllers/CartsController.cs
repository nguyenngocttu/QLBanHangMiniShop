using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using QLBanHangMiniShop.Models;
using System.Web.Mvc;
using System.Web;
using Newtonsoft.Json;
using QLBanHangMiniShop.Class;

namespace QLBanHangMiniShop.Controllers
{
    
    public class CartsController : Controller
    {
        public class jsonMess
        {
            public string mess { get; set; }
            public int value { get; set; }
        }
        public class ItemCart
        {
            public int id { get; set; }
            public string hinhDaiDien { get; set; }
            public string tenSanPham { get; set; }
            public decimal gia { get; set; }
            public int soLuong { get; set; }
            public float giamGia { get; set; }
            public  decimal tongGia { get; set; }
        }
        [HttpGet]
        [ChildActionOnly]
        public ContentResult ReponseItemCart()
        {
            List<CartCookie> listIdProduct = new List<CartCookie>();
            listIdProduct = RequestCookie(listIdProduct, "CartCookie");
            return Content(listIdProduct.Count().ToString());
        }
        /// <summary>
        /// nhận id từ client cập nhật cookie mới danh sách id của sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]//trả về số lượng sản phẩm trong giỏ và thông báo
        public JsonResult UpdateItemCart(int id)
        {
            CartCookie newCookie = new CartCookie();
            jsonMess result = new jsonMess();
            
            // lấy cookie             
            List<CartCookie> listIdProduct = new List<CartCookie>();
            HttpCookie cartsCookie = HttpContext.Request.Cookies["CartCookie"];//lấy cookie từ client

            if (cartsCookie != null)//đã có cookie
            {
                string valueCookie = Server.UrlDecode(cartsCookie.Value);//dịch ngược cookie đã lấy
                listIdProduct = JsonConvert.DeserializeObject<List<CartCookie>>(valueCookie);//chuyển từ kiểu json sang list<int>
                //kiểm tra đã có id sản phẩm chưa
                if(listIdProduct.Where(c=>c.idProduct==id).Count()!=0)//đã có sản phẩm này thì tăng số lượng
                {
                    newCookie = listIdProduct.Where(c => c.idProduct == id).SingleOrDefault();
                    newCookie.total += 1;//tăng số lượng
                    listIdProduct.Remove(newCookie);
                    listIdProduct.Add(newCookie);
                    result.mess = "Đã cập nhật số lượng sản phẩm";
                    result.value = listIdProduct.Count();                   
                }
                else
                {
                    newCookie.total = 1;
                    newCookie.idProduct = id;
                    listIdProduct.Add(newCookie);
                    result.mess = "Đã thêm sản phẩm vào giỏ";
                    result.value = listIdProduct.Count();
                }    
                //tạo và gửi cookie
                string jsonItemCart = JsonConvert.SerializeObject(listIdProduct,Formatting.Indented);//chuyển list<int> thành chuỗi json
                HttpCookie cartCookie = new HttpCookie("CartCookie");
                cartCookie.Value = Server.UrlEncode(jsonItemCart);//mã hóa cookie
                HttpContext.Response.Cookies.Add(cartCookie);//ghi đè cookie mới lên cookie cũ
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            else//nếu cookie chưa có 
            {
                newCookie.total = 1;
                newCookie.idProduct = id;
                listIdProduct.Add(newCookie);//thêm id mới vào cookie
                string jsonItemCart = JsonConvert.SerializeObject(listIdProduct, Formatting.Indented);//chuyển list<int> thành chuỗi json
                HttpCookie cartCookie = new HttpCookie("CartCookie");
                cartCookie.Value = Server.UrlEncode(jsonItemCart);//mã hóa cookie
                HttpContext.Response.Cookies.Add(cartCookie);//ghi đè cookie mới lên cookie cũ
                result.mess = "Đã thêm sản phẩm vào giỏ";
                result.value = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }           
        }
        [HttpGet]
        public ActionResult Cart()
        {

            ViewBag.lstLoaiSP = Class.ListShop.GetListChungLoai();
            QLBanHangDbContext db = new QLBanHangDbContext();
            List<CartCookie> listIdProduct = new List<CartCookie>();
            listIdProduct = RequestCookie(listIdProduct, "CartCookie");
            List<ItemCart> listItemCart = new List<ItemCart>();
            decimal Subtotal = 0;
            float Discount = 0;
            foreach (var item in listIdProduct)
            {
                var product = db.HangHoas.Find(item.idProduct);
                ItemCart itemCart = new ItemCart {
                    id=product.ID,
                    hinhDaiDien = product.hinhDaiDien[0],
                    tenSanPham = product.Ten,
                    gia = (decimal)product.GiaBan,
                    soLuong = item.total,
                    tongGia = item.total * product.GiaBan
                                    
                };
                Subtotal+= (itemCart.gia * itemCart.soLuong);
                listItemCart.Add(itemCart);
            }
            ViewBag.Subtotal = Subtotal;
            ViewBag.Discount = Discount;
            ViewBag.Total = Subtotal - ((Subtotal / 100) * (decimal)Discount);

            //công cụ đếm số người đang online 
            SessionsAccess.SetSessionAccess(AccessMode.Visitor);
            return View(listItemCart);
        }
        private List<T>RequestCookie<T>(List<T> list,string cookieName )
        {           
            HttpCookie Cookie = HttpContext.Request.Cookies[cookieName];
            if (Cookie!=null)
            {
                string valueCookie = Server.UrlDecode(Cookie.Value);
                list = JsonConvert.DeserializeObject<List<T>>(valueCookie);
                return list;
            }       
            else
            {
                return list;
            }                    
        }
        public ActionResult CreateBill()///partialView
        {
            QLBanHangDbContext db = new QLBanHangDbContext();
            //tạo danh sách chưa cookie giỏ hàng
            List<CartCookie> listIdProduct = new List<CartCookie>();
            //lấy cookie
            listIdProduct = RequestCookie(listIdProduct, "CartCookie");
            decimal Total = 0;//tổng giá của giỏ hàng
            foreach(var item in listIdProduct)
            {
                Total += db.HangHoas.Find(item.idProduct).GiaBan * item.total;
            }
            ViewBag.Total = Total;
            return View();
        }
        [HttpPost]
        
        public ActionResult CreateBill(HoaDon hd)//chưa bắt lỗi và ngoại lệ
        {
            QLBanHangDbContext db = new QLBanHangDbContext();
            List<CartCookie> listIdProduct = new List<CartCookie>();
            listIdProduct = RequestCookie(listIdProduct, "CartCookie");
            foreach(var item in listIdProduct)
            {
                HoaDonChiTiet hdct = new HoaDonChiTiet();
                hdct.HoaDonID = hd.ID;
                hdct.HangHoaID = item.idProduct;
                hdct.SoLuong = item.total;
                hdct.DonGia= db.HangHoas.Find(item.idProduct).GiaBan;
                hdct.ThanhTien = item.total * db.HangHoas.Find(item.idProduct).GiaBan;
                hdct.HangHoa = db.HangHoas.Find(item.idProduct);
                hd.TongTien += hdct.ThanhTien;
                hd.HoaDonChiTiets.Add(hdct);                
            }
            hd.NgayDatHang = DateTime.Now;
            hd.TrangThai = "Chưa xác nhận";
            db.HoaDons.Add(hd);
            db.SaveChanges();
            var h = db.HoaDons.Where(x => x.ID == hd.ID).SingleOrDefault();
            if (Request.Cookies["CartCookie"] != null)
            {
                var infock = new HttpCookie("CartCookie");
                infock.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(infock);
            }
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult RemoveItemCart(int id)
        {
            CartCookie item = new CartCookie();
            jsonMess result = new jsonMess();

            // lấy cookie             
            List<CartCookie> listIdProduct = new List<CartCookie>();
            HttpCookie cartsCookie = HttpContext.Request.Cookies["CartCookie"];//lấy cookie từ client

            if (cartsCookie != null)//đã có cookie
            {
                string valueCookie = Server.UrlDecode(cartsCookie.Value);//dịch ngược cookie đã lấy
                listIdProduct = JsonConvert.DeserializeObject<List<CartCookie>>(valueCookie);//chuyển từ kiểu json sang list<int>
                //kiểm tra đã có id có tồn tại hay không
                if (listIdProduct.Where(c => c.idProduct == id).SingleOrDefault()!=null)//đã có sản phẩm này thì tăng số lượng
                {
                    item = listIdProduct.Where(c => c.idProduct == id).SingleOrDefault();
                  
                    listIdProduct.Remove(item);
                    result.mess = "Đã bỏ sản phẩm khỏi giỏ";
                    result.value = listIdProduct.Count();
                }
                else
                {
                    
                    result.mess = "Lỗi khi xóa sản phẩm";
                    result.value = listIdProduct.Count();
                }
                //tạo và gửi cookie
                string jsonItemCart = JsonConvert.SerializeObject(listIdProduct, Formatting.Indented);//chuyển list<int> thành chuỗi json
                HttpCookie cartCookie = new HttpCookie("CartCookie");
                cartCookie.Value = Server.UrlEncode(jsonItemCart);//mã hóa cookie
                HttpContext.Response.Cookies.Add(cartCookie);//ghi đè cookie mới lên cookie cũ
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else//nếu cookie chưa có 
            {
               
                result.mess = "Chưa có sản phẩm trong giỏ";
                result.value = 1;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
       
    }
}

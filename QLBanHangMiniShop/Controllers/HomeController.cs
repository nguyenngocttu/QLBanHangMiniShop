using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanHangMiniShop.Models;
using QLBanHangMiniShop.Class;



namespace QLBanHangMiniShop.Controllers
{
    public class HomeController : Controller
    {
        //trang chủ 
        public ActionResult Index()
        {
            ViewBag.lstLoaiSP = ListShop.GetListChungLoai();
            ViewBag.lstSPMoi = ListShop.GetListHangHoaMoi(1,10);
            SessionsAccess.SetSessionAccess(AccessMode.Visitor);
            return View();
        }
        [ChildActionOnly]
        public PartialViewResult DanhSachSanPham (int loaiSp,int sluong)       
        {
            ViewBag.loaiSP = ListShop.db.ChungLoais.Find(loaiSp).Ten;
            ViewBag.lstSPMoi = ListShop.GetListHangHoaMoi(loaiSp, sluong);
            return PartialView("_DanhSachSanPhamPartial");
        }
        public ActionResult SingleProductPage(int id)
        {
            SessionsAccess.SetSessionAccess(AccessMode.Visitor);
            return View();
        }
       
        
       
    }
}
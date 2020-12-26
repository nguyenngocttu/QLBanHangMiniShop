using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanHangMiniShop.Models;
using QLBanHangMiniShop.Class;
namespace QLBanHangMiniShop.Controllers
{
    public class ShopController : Controller
    {
        public static int CLoaiSP = 1;
        public ActionResult Index(int? CloaiSP)
        {
            if(CloaiSP!=null)
            {
                CLoaiSP = int.Parse(CloaiSP.ToString());
               
               
            }
            ViewBag.ChungLoai = ListShop.db.ChungLoais.Find(CLoaiSP).Ten;
            ViewBag.SoLuong = ListShop.db.HangHoas.Where(h => h.Loai.ChungLoaiID == CLoaiSP).Count();
            ViewBag.lstLoaiSP = Class.ListShop.GetListChungLoai();
            SessionsAccess.SetSessionAccess(AccessMode.Visitor);
            return View();
        }
        [ChildActionOnly]
        [HttpGet]
        public PartialViewResult DanhSachSanPham()
       {

            List<HangHoa> lst = Class.SubPage.LstHangHoa_CLoai(CLoaiSP,1, 9).ToList();
            return PartialView("_DanhSachSanPhamPartial", lst);       
        }       
        [HttpPost]
        public PartialViewResult DanhSachSanPham( int indexPage)
        {
           
            QLBanHangDbContext db = new QLBanHangDbContext();
            int totalPage = (db.HangHoas.Where(h=>h.Loai.ChungLoaiID==CLoaiSP).Count() / 9) + 1;
            indexPage = indexPage > totalPage ? 1 : indexPage;
            if (indexPage < 0)
            {
                ViewBag.NameError = "Trang yêu cầu không tồn tại";
                return PartialView("Error");
            }
            List<HangHoa> lst = SubPage.LstHangHoa_CLoai(CLoaiSP,indexPage, 9).ToList();
            
            return PartialView("_DanhSachSanPhamPartial", lst);
        }
        [ChildActionOnly]
        public PartialViewResult DanhMucSP()
        {
            List<ChungLoai> ls= Class.ListShop.GetListChungLoai();
            ViewBag.Catalog = ls;
            ViewBag.CountCatalog = ls.Count();
            return PartialView("_DanhMucSPPartial");
        }
        [ChildActionOnly]    
        [HttpGet]
        public PartialViewResult PageIndex()
        {
          QLBanHangDbContext db = new QLBanHangDbContext();
        
        int c = db.HangHoas.Include("ChungLoai").Where(h=>h.Loai.ChungLoaiID==CLoaiSP).Count();
            ViewBag.TotalPage = (c % 9) == 0 ?
                  (c / 9) :
                  (c / 9) + 1;
            ViewBag.ActionName = "../Shop/DanhSachSanPham";
            return PartialView("ListPage");            
        }

        public ActionResult SingleProductPage(int id)
        {          
            ViewBag.lstLoaiSP = Class.ListShop.GetListChungLoai();
            HangHoa hh = Class.ListShop.GetHangHoas(id);
            SessionsAccess.SetSessionAccess(AccessMode.Visitor);                            
            return View(hh);
        }
      

    }
}
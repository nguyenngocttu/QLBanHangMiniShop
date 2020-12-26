using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBanHangMiniShop.Models;

namespace QLBanHangMiniShop.Controllers.Admin
{
    public class HoaDonsController : Controller
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        // GET: HoaDons
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: HoaDons/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = await db.HoaDons.Where(x=>x.ID==id).SingleOrDefaultAsync();
            List<HoaDonChiTiet> hdct =await db.HoaDonChiTiets.Include("HangHoa").Where(x => x.HoaDonID == hoaDon.ID).ToListAsync();
            //ViewBag.HDCT = hdct;
            hoaDon.HoaDonChiTiets = hdct ;
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        // GET: HoaDons/Create


        // GET: HoaDons/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = await db.HoaDons.FindAsync(id);
            List<HoaDonChiTiet> hdcts = await db.HoaDonChiTiets.Include("HangHoa").Where(x => x.HoaDonID == hoaDon.ID).ToListAsync();
            hoaDon.HoaDonChiTiets = hdcts;
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Chỉnh sửa thông tin đơn hàng";
            ViewBag.Header = "Đơn hàng " + id;
            return View(hoaDon);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "ID,NgayDatHang,HoTenKhach,DiaChi,DienThoai,Email,TongTien,TrangThai")]
            HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                HoaDon hd = db.HoaDons.Find(hoaDon.ID);
                hd.HoTenKhach = hoaDon.HoTenKhach;
                hd.DiaChi = hoaDon.DiaChi;
                hd.DienThoai = hoaDon.DienThoai;
                hd.Email = hoaDon.Email;
                await db.SaveChangesAsync();
                ViewBag.CRUD = "edit";
                return PartialView("RowHoaDonPartial_", hd);
            }
            return View(hoaDon);
        }
        public async Task<ActionResult> Delete(int? id,string trangThai)
        {
            ViewBag.Header = "Xóa Đơn hàng";
            ViewBag.Title = "Xóa";
            ViewBag.TrangThai = trangThai;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HoaDon hoaDon = await db.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
           
            try
            {
                HoaDon hoaDon = await db.HoaDons.FindAsync(id);
                List<HoaDonChiTiet> hdct = await db.HoaDonChiTiets.Include("HangHoa").Where(x => x.HoaDonID == hoaDon.ID).ToListAsync();
                //ViewBag.HDCT = hdct;

                hoaDon.HoaDonChiTiets = hdct;
                db.HoaDons.Remove(hoaDon);
                await db.SaveChangesAsync();
               
                if (db.HoaDons.Count()>5)
                {
                    HoaDon hoaDonNext = db.HoaDons.OrderBy(x => x.ID).Take(1).SingleOrDefault();
                    ViewBag.CRUD = "delete";
                  
                    return PartialView("RowHoaDonPartial_", hoaDonNext);
                }
                else
                {
                    return Content("");
                }

            }
            catch (Exception e)
            {
                return Content("xảy ra lôi trong quá trình xóa : " + e.ToString());
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        [ChildActionOnly]
        
        public async Task<ActionResult> DanhSachDonHang(int trangThai)
        {
            string tThai = "";
            List<HoaDon> lst = new List<HoaDon>();           
            if (trangThai==0)
            {
                lst = Class.SubPage.LstHoaDon(1, 5).ToList();
            }
            else
            {
                if (trangThai == 1)
                {
                    tThai = "Chưa xác nhận";
                }
                else

                if (trangThai == 2)
                {
                    tThai = "Đã xác nhận";
                }
                else if (trangThai == 3)
                {
                    tThai = "Hoàn thành";
                }
                else if (trangThai == 4)
                {
                    tThai = "Thất Bại";
                }
                else
                {
                    ViewBag.NameError = "Trang yêu cầu không tồn tại";
                    return PartialView("Error");
                }
                lst = Class.SubPage.LstHoaDon_Status(1, 5, tThai).ToList();
            }
           
            return PartialView("_DanhSachHoaDon", lst);
        }
        [HttpPost]
        public async Task<PartialViewResult> DanhSachDonHang(int indexPage,int trangThai)
        {
            string tThai = "";
            if(trangThai>0)
            {
                if (trangThai == 1)
                {
                    tThai = "Chưa xác nhận";
                }
                else

                if (trangThai == 2)
                {
                    tThai = "Đã xác nhận";
                }
                else if (trangThai == 3)
                {
                    tThai = "Hoàn thành";
                }
                else if(trangThai==4)
                {
                    tThai = "Thất Bại";
                }    
                else
                {
                    ViewBag.NameError = "Trang yêu cầu không tồn tại";
                    return PartialView("Error");
                }
                int totalPage = (db.HoaDons.Where(h => h.TrangThai == tThai).Count() / 5) + 1;
                indexPage = indexPage > totalPage ? 1 : indexPage;
                if (indexPage < 0)
                {
                    ViewBag.NameError = "Trang yêu cầu không tồn tại";
                    return PartialView("Error");
                }
                List<HoaDon> lst = Class.SubPage.LstHoaDon_Status(indexPage, 5,tThai).ToList();
                return PartialView("_DanhSachHoaDon", lst);
            }
            else
            {
                int totalPage = (db.HoaDons.Count() / 5) + 1;
                indexPage = indexPage > totalPage ? 1 : indexPage;
                if (indexPage < 0)
                {
                    ViewBag.NameError = "Trang yêu cầu không tồn tại";
                    return PartialView("Error");
                }
                List<HoaDon> lst = Class.SubPage.LstHoaDon(indexPage, 5).ToList();
                return PartialView("_DanhSachHoaDon", lst);
            }
            
        }
        [HttpGet]
        public PartialViewResult ListPage(int trangThai)
        {
            if( trangThai <1)
            {
                int c = db.HoaDons.Count();
                ViewBag.TotalPage = (c % 5) == 0 ?
                      (c / 5) :
                      (c / 5) + 1;
                ViewBag.ActionName = "../HoaDons/DanhSachDonHang/?trangThai="+trangThai+"&";
                return PartialView("ListPage");
            }
            else
            {
                string tThai = "";
                if (trangThai == 1)
                {
                    tThai = "Chưa xác nhận";
                }
                else

                if (trangThai == 2)
                {
                    tThai = "Đã xác nhận";
                }
                else if (trangThai == 3)
                {
                    tThai = "Hoàn thành";
                }
                else if (trangThai == 4)
                {
                    tThai = "Thất Bại";
                }
                else
                {
                    ViewBag.NameError = "Trang yêu cầu không tồn tại";
                    return PartialView("Error");
                }
                int c = db.HoaDons.Where(h => h.TrangThai == tThai).Count();
                ViewBag.TotalPage = (c % 5) == 0 ?
                      (c / 5) :
                      (c / 5) + 1;
                ViewBag.ActionName = "../HoaDons/DanhSachDonHang/?trangThai="+trangThai+"&";
                return PartialView("ListPage");
            }
          
        }
        [HttpPost]
        public ContentResult ListPagePost(string trangThai)
        {
            if (  trangThai !="cxn"&&trangThai!="dxn"&&trangThai!="ht"||trangThai=="")
            {
                int totalPage = db.HoaDons.Count();
                totalPage = (totalPage % 5) == 0 ?
                      (totalPage / 5) :
                      (totalPage / 5) + 1;
                return Content(totalPage.ToString());
            }    
            else
            {
                string tThai = "";
                if (trangThai == "cxn")
                {
                    tThai = "Chưa xác nhận";
                }
                else

                if (trangThai == "dxn")
                {
                    tThai = "Đã xác nhận";
                }
                else if (trangThai == "ht")
                {
                    tThai = "Hoàn thành";
                }
                else if (trangThai == "tb")
                {
                    tThai = "Thất Bại";
                }
                else
                {
                    ViewBag.NameError = "Trang yêu cầu không tồn tại";
                    return Content("Error");
                }
                int totalPage = db.HoaDons.Where(h=>h.TrangThai==tThai).Count();
                totalPage = (totalPage % 5) == 0 ?
                      (totalPage / 5) :
                      (totalPage / 5) + 1;
                return Content(totalPage.ToString());
            }    
                
        }
    }
}

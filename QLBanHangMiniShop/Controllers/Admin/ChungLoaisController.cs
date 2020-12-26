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
using System.Threading;

namespace QLBanHangMiniShop.Controllers.Admin
{
    [Authorize]
    public class ChungLoaisController : Controller
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        // GET: ChungLoais
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        // GET: ChungLoais/Details/5        
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungLoai chungLoai = await db.ChungLoais.FindAsync(id);
            if (chungLoai == null)
            {
                return HttpNotFound();
            }
            ViewBag.Header = "Chủng loại";
            ViewBag.Title = "Thông tin chi tiết";
            return View(chungLoai);
        }

        // GET: ChungLoais/Create
        [Authorize(Roles ="NhanVien")]
        public ActionResult Create()
        {
            
            ViewBag.Title = "Thêm Chủng loại";
            return View();
        }

        // POST: ChungLoais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "NhanVien")]
        public async Task<ActionResult> Create([Bind(Include = "ID,Ten")] ChungLoai chungLoai)
        {

            if (ModelState.IsValid)
            {
                db.ChungLoais.Add(chungLoai);
                await db.SaveChangesAsync();
                //ViewBag.CRUD = "create";
                //return PartialView("RowChungLoaiPartial_");
                return Content("Đã thêm thành công");
            }

            return Content("Vui lòng kiểm tra lại dữ liệu");
        }

        // GET: ChungLoais/Edit/5
        [Authorize(Roles = "QuanLy")]
        public async Task<ActionResult> Edit(int? id)
        {
            ViewBag.Header = "Chủng loại";
            ViewBag.Title = "Chỉnh sửa thông tin";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungLoai chungLoai = await db.ChungLoais.FindAsync(id);
            if (chungLoai == null)
            {
                return HttpNotFound();
            }
            return View(chungLoai);
        }

        // POST: ChungLoais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "QuanLy")]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Ten")] ChungLoai chungLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chungLoai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //sẽ cho trả về 1 đối tượng json cho nhẹ
                ViewBag.CRUD = "edit";
                return PartialView("RowChungLoaiPartial_", chungLoai);
            }          
            return View(chungLoai);
        }

        // GET: ChungLoais/Delete/5         
        [Authorize(Users ="admin@gmail.com")]
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.Header = "Chủng loại";
            ViewBag.Title = "Xóa";
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungLoai chungLoai = await db.ChungLoais.FindAsync(id);
            if (chungLoai == null)
            {
                return HttpNotFound();
            }
            return View(chungLoai);
        }

        // POST: ChungLoais/Delete/5
        [HttpPost, ActionName("Delete")]
        
       // [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChungLoai chungLoai = await db.ChungLoais.FindAsync(id);
            db.ChungLoais.Remove(chungLoai);
            try
            {
                await db.SaveChangesAsync();
                
                ChungLoai chungLoai1 =await db.ChungLoais.OrderBy(cl=>cl.ID).Where(cl=>cl.ID>id).Take(1).SingleOrDefaultAsync();
                if (chungLoai1 != null)
                {
                    //nên trả về đối tượng json cho nhẹ

                    ViewBag.CRUD = "delete";
                    return PartialView("RowChungLoaiPartial_", chungLoai1);
                }
                else
                    return Content("");
              
            }
            catch(Exception e)
            {
                return Content("xảy ra lôi trong quá trình xóa : "+e.ToString());
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
        
       
        [HttpPost]       
        public async Task<PartialViewResult> DanhSachChungLoai(int indexPage)
        {
            int totalPage = (db.ChungLoais.Count() / 5) + 1;
            indexPage = indexPage > totalPage ? 1 : indexPage;
            if (indexPage<0)
            {
                ViewBag.NameError = "Trang yêu cầu không tồn tại";
                return PartialView("Error");
            }    
            //bằng 0 là lấy trang cuối cùng
            List<ChungLoai> lst =  Class.SubPage.LstChungLoai(indexPage,5).ToList();
            return PartialView(lst);
        }
        [HttpGet]
        [ChildActionOnly]
        public async Task<PartialViewResult> DanhSachChungLoai()
        {
           
           List<ChungLoai> lst =  Class.SubPage.LstChungLoai(1, 5).ToList();
           return PartialView(lst);
        }
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult ListPage()
        {
            ViewBag.TotalPage = (db.ChungLoais.Count() % 5) == 0 ?
                  (db.ChungLoais.Count() / 5) :
                  (db.ChungLoais.Count() / 5) + 1;
            ViewBag.ActionName = "../ChungLoais/DanhSachChungLoai";
            return PartialView("ListPage");
        }
        [HttpPost]
        public ContentResult ListPagePost()
        {
            int totalPage= (db.ChungLoais.Count() % 5) == 0 ?
                  (db.ChungLoais.Count() / 5) :
                  (db.ChungLoais.Count() / 5) + 1;
            
            return Content(totalPage.ToString());
        }
    }
}

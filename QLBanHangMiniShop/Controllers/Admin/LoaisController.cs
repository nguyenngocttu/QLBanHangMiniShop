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
    public class LoaisController : Controller
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        // GET: Loais
        public async Task<ActionResult> Index()
        {
            ViewBag.Search = "Loais";
            return View(model:"Loais");
        }
        //search theo ten
        public async Task<ActionResult> Search(string value,int? indexPage)
        {
            int indexP = 0;
            if(indexPage!=null)
            {
                indexP = indexPage.Value;
            }
            try
            {
                int totalPage= (db.ChungLoais.Count() % 5) == 0 ?
                (db.ChungLoais.Count() / 5) :
                (db.ChungLoais.Count() / 5) + 1;
                var result = await db.Loais.Where(x => x.Ten.Contains(value)).OrderBy(x=>x.ID).Skip((totalPage-1)*5).Take(5).ToListAsync();
                ViewBag.Search = "Loai";
                ViewBag.Header = $"Kết quả tìm kiếm {value}";
                return PartialView("_DanhSachLoaiPartial",result);
            }
            catch
            {
                return PartialView("_DanhSachLoaiPartial", db.Loais.ToList());
            }
        }
        // GET: Loais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.Search = "Loai";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loai loai = await db.Loais.Include("ChungLoai").FirstOrDefaultAsync(l=>l.ID==id);
            if (loai == null)
            {
                return HttpNotFound();
            }
            return View(loai);
        }

        // GET: Loais/Create
        public ActionResult Create()
        {
            
            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ID", "Ten");
            ViewBag.Title = "Thêm Loại Sản Phẩm";
            return View();
        }

        // POST: Loais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       // [ValidateAntiForgeryToken]
        public async Task<ContentResult> Create([Bind(Include = "ID,MaSo,Ten,ChungLoaiID")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                db.Loais.Add(loai);
                await db.SaveChangesAsync();
                return Content("đã thêm thành công");
            }

            //ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ID", "Ten", loai.ChungLoaiID);
            
            return Content("Vui lòng kiểm tra lại dữ liệu");
        }

        // GET: Loais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loai loai = await db.Loais.FindAsync(id);
            if (loai == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Chỉnh sửa thông tin";
            ViewBag.Header = "Loại Sản Phẩm";
           
            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ID", "Ten", loai.ChungLoaiID);
            return View(loai);
        }

        // POST: Loais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MaSo,Ten,ChungLoaiID")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loai).State = EntityState.Modified;

                await db.SaveChangesAsync();
                ViewBag.CRUD = "edit";
                ViewBag.TenChungLoai = db.ChungLoais.Find(loai.ChungLoaiID).Ten;
                return PartialView("RowLoaiPartial_", loai);             
            }            
            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ID", "Ten", loai.ChungLoaiID);
            return View(loai);
        }

        // GET: Loais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            ViewBag.Header = "Loại Sản Phẩm";
            ViewBag.Title = "Xóa";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loai loai = await db.Loais.FindAsync(id);
            if (loai == null)
            {
                return HttpNotFound();
            }
            ViewBag.TenChungLoai = db.ChungLoais.Find(loai.ChungLoaiID).Ten;
            return View(loai);
        }

        // POST: Loais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Loai loai = await db.Loais.FindAsync(id);
            db.Loais.Remove(loai);
            try
            {
                await db.SaveChangesAsync();
                Loai loai1 = await db.Loais.OrderBy(cl => cl.ID).Where(cl => cl.ID > id).Take(1).SingleOrDefaultAsync();
                if (loai1 != null)
                {
                    ViewBag.CRUD = "delete";
                    return PartialView("RowLoaiPartial_", loai1);
                }
                else
                    return Content("");               
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
        public async Task<ActionResult> DanhSachLoai()
        {
            //lấy danh sách loại tại trang 1
            List<Loai> lst = Class.SubPage.LstLoai(1, 5).ToList();
            return PartialView("_DanhSachLoaiPartial",lst);
        }
        [HttpPost]
        public async Task<PartialViewResult>DanhSachLoai(int indexPage)
        {
            int totalPage = (db.Loais.Count() / 5)+1;
            indexPage = indexPage >totalPage ? 1 : indexPage;
            if(indexPage<0)
            {
                ViewBag.NameError = "Trang yêu cầu không tồn tại";
                return PartialView("Error");
            }
            List<Loai> lst = Class.SubPage.LstLoai(indexPage, 5).ToList();
            return PartialView("_DanhSachLoaiPartial", lst);
        }      
        [HttpGet]
        public PartialViewResult ListPage(string key)
        {
            if(key==null)
            {
                int c = db.Loais.Count();
                ViewBag.TotalPage = (c % 5) == 0 ?
                      (db.Loais.Count() / 5) :
                      (db.Loais.Count() / 5) + 1;
                ViewBag.ActionName = "../Loais/DanhSachLoai";
                return PartialView("ListPage");
            }
            else
            {
                int c = db.Loais.Count();
                ViewBag.TotalPage = (c % 5) == 0 ?
                      (db.Loais.Count() / 5) :
                      (db.Loais.Count() / 5) + 1;
                ViewBag.ActionName = "../Loais/Search?value="+key+"&";
                return PartialView("ListPage");
            }
         
        }
        [HttpPost]
        public ContentResult ListPagePost()
        {
            int totalPage = db.Loais.Count();
            totalPage = (totalPage % 5) == 0 ?
                  (db.Loais.Count() / 5) :
                  (db.Loais.Count() / 5) + 1;
            return Content(totalPage.ToString());
        }
    }
}

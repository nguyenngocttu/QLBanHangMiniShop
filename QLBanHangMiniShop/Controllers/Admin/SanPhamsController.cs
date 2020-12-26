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
using System.Drawing;
using System.IO;

namespace QLBanHangMiniShop.Controllers.Admin
{
    public class SanPhamsController : Controller
    {
        private QLBanHangDbContext db = new QLBanHangDbContext();

        // GET: SanPhams
        public async Task<ActionResult> Index()
        {
            
            return View();
        }

        // GET: SanPhams/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHoa hangHoa = await db.HangHoas.Include("Loai").FirstOrDefaultAsync(sp=>sp.ID==id);
            if (hangHoa == null)
            {
                return HttpNotFound();
            }
            return View(hangHoa);
        }

        // GET: SanPhams/Create
        public ActionResult Create()
        {
            ViewBag.LoaiID = new SelectList(db.Loais, "ID", "MaSo");
            ViewBag.Title = "Thêm Sản Phẩm";
            return View();
        }
        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HangHoa hangHoa, HttpPostedFileBase[] file)
        {
                        
            try
            {
                if (ModelState.IsValid)
                {
                    hangHoa.NgayTao = DateTime.Today;
                    hangHoa.NgayCapNhat = DateTime.Today;
                    string Hinh = "";
                    int dem = 1;

                    foreach (var item in file)
                    {
                        string[] f = item.FileName.Split('.');
                        Hinh += hangHoa.MaSo + "-" + dem + "." + f[1] + ",";
                        string url = Server.MapPath("~/Photos/" + hangHoa.MaSo + "-" + dem + "." + f[1]);
                        //Class.DBFileControl.ResizeImg(Image.FromStream(item.InputStream), 256, url);
                        item.SaveAs(url);
                        dem++;
                    }
                    hangHoa.TenHinh = Hinh.Remove(Hinh.LastIndexOf(','));
                    db.HangHoas.Add(hangHoa);
                    await db.SaveChangesAsync();
                    return Content("đã thêm thành công");
                }
                return Content("false");
            }
            catch(Exception ex )
            {
                return Content($"Gặp lỗi trong quá trình kiểm thêm dữ liệu: {ex.Message} ");
            }
         
        }

        // GET: SanPhams/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            
                if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHoa hangHoa = await db.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Title = "Chỉnh sửa thông tin";
            ViewBag.Header = "Loại Sản Phẩm";
            ViewBag.LoaiID = new SelectList(db.Loais, "ID", "MaSo", hangHoa.LoaiID);
            return View(hangHoa);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,MaSo,Ten,DonViTinh,MoTa,ThongSoKyThuat,GiaBan,LoaiID")] HangHoa hangHoa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //db.Entry(hangHoa).State = EntityState.Modified;
                    HangHoa entity = db.HangHoas.Find(hangHoa.ID);
                    entity.Ten = hangHoa.Ten;
                    entity.DonViTinh = hangHoa.DonViTinh;
                    entity.MoTa = hangHoa.MoTa;
                    entity.MaSo = hangHoa.MaSo;
                    entity.ThongSoKyThuat = hangHoa.ThongSoKyThuat;
                    entity.LoaiID = hangHoa.LoaiID;
                    entity.GiaBan = hangHoa.GiaBan;

                    //lấy danh sách hinh
                    List<string> arrTenhinh = entity.TenHinh.Contains(',') ? entity.TenHinh.Split(',').ToList() : entity.TenHinh.Split().ToList();

                  //lấy các input[type=file]
                 for(int i =1;i<10;i++)
                    {
                        HttpPostedFileBase file = Request.Files["file" + i];
                        if (file != null&&i>arrTenhinh.Count())
                        {
                            string[] f = file.FileName.Split('.');
                         
                            string url = Server.MapPath("~/Photos/" + hangHoa.MaSo + "-" + i + "." + f[1]);
                            
                            //Class.DBFileControl.ResizeImg(Image.FromStream(item.InputStream), 256, url);
                            file.SaveAs(url);
                            arrTenhinh.Add(hangHoa.MaSo + "-" + i + "." + f[1]);
                        }
                        if(file!=null&&i<=arrTenhinh.Count())
                        {
                            string[] f = file.FileName.Split('.');

                            string url = Server.MapPath("~/Photos/" + hangHoa.MaSo + "-" + i + "." + f[1]);
                            System.IO.File.Delete(arrTenhinh[i]);
                            arrTenhinh.Remove(arrTenhinh[i]);
                            //Class.DBFileControl.ResizeImg(Image.FromStream(item.InputStream), 256, url);
                            file.SaveAs(url);
                            arrTenhinh.Add(hangHoa.MaSo + "-" + i + "." + f[1]);
                        }
                    }    
                    
                     db.SaveChanges();//lưu bị lỗi
                    ViewBag.CRUD = "edit";
                    ViewBag.TenLoai = db.Loais.Find(entity.LoaiID).Ten;
                    return PartialView("RowSanPhamPartial_", entity);
                }
                ViewBag.LoaiID = new SelectList(db.Loais, "ID", "MaSo", hangHoa.LoaiID);
                return View(hangHoa);
            }
            catch (Exception ex)
            {
                return Content($"Gặp lỗi trong quá trình kiểm thêm dữ liệu: {ex.Message} ");
            }

        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> AddImgHangHoa(HttpPostedFileBase file,int id)
        //{
        //    var hh = db.HangHoas.Find(id);
        //    if(hh.TenHinh!=null)
        //        if(hh.TenHinh.Contains(','))
        //        {
        //            string[] arrHinh = hh.TenHinh.Split(',');
        //            string[] f = file.FileName.Split('.');
        //            hh.TenHinh += "," + hh.MaSo + "-" + arrHinh.Length + "." + f[1] + ",";
        //            string url = Server.MapPath("~/Photos/" + hh.MaSo + "-" + arrHinh.Length + "." + f[1]);
        //            file.SaveAs(url);
        //        }
        //    else
        //        {
        //            string[] f = file.FileName.Split('.');
        //            hh.TenHinh += "," + hh.MaSo + "-" +2+ "." + f[1] + ",";
        //            string url = Server.MapPath("~/Photos/" + hh.MaSo + "-" + 2 + "." + f[1]);
        //            file.SaveAs(url);
        //        }
        //    else
        //    {
        //        string[] f = file.FileName.Split('.');
        //        hh.TenHinh= hh.MaSo + "-" + 1 + "." + f[1] + ",";
        //        string url = Server.MapPath("~/Photos/" + hh.MaSo + "-" + 1 + "." + f[1]);
        //        file.SaveAs(url);
        //    }
        //    await db.SaveChangesAsync();
        //    return Content("ok");
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ChangeImghangHoa(HttpPostedFileBase file,int indexImg,int id)
        //{
        //    var hh = db.HangHoas.Find(id);
        //    if(!hh.TenHinh.Contains(','))
        //    {
        //        string[] f = file.FileName.Split('.');
              
        //        string url = Server.MapPath("~/Photos/"+hh.TenHinh);
        //        System.IO.File.Delete(url);
        //        url= Server.MapPath("~/Photos/" + hh.MaSo + "-" + 1 + "." + f[1]);
        //        file.SaveAs(url);
        //        hh.TenHinh = hh.MaSo + "-" + 1 + "." + f[1] + ",";
        //    }
        //    else
        //    {
        //        string[] arrTenHinh = hh.TenHinh.Split(',');
        //        string url = Server.MapPath("~/Photos/" + arrTenHinh[indexImg]);
        //        System.IO.File.Delete(url);
        //        string[] f = file.FileName.Split('.');
        //        url = Server.MapPath("~/Photos/" + hh.MaSo + "-" + 1 + "." + f[1]);
        //        file.SaveAs(url);
        //        hh.TenHinh = hh.MaSo + "-" + 1 + "." + f[1] + ",";
        //    }
        //    return Content("ok");
        //}
        //// GET: SanPhams/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHoa hangHoa = await db.HangHoas.FindAsync(id);

            if (hangHoa == null)
            {
                return HttpNotFound();
            }
            return View(hangHoa);
        }
        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HangHoa hangHoa = await db.HangHoas.FindAsync(id);
            
            try
            {
                db.HangHoas.Remove(hangHoa);
                await db.SaveChangesAsync();
                HangHoa hh = await db.HangHoas.Include("Loai").OrderBy(cl => cl.ID)
                                                .Where(cl => cl.ID > id)
                                                .Take(1)
                                                .SingleOrDefaultAsync();
                if (hh != null)
                {
                    ViewBag.CRUD = "delete";
                    return PartialView("RowSanPhamPartial_", hh);
                }
                else
                    return Content("");
            }
            catch(Exception e)
            {
                return Content("xảy ra lôi trong quá trình xóa : " + e.Message);
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
        public async Task<ActionResult> DanhSachSanPham()
        {
            List<HangHoa> lst = Class.SubPage.LstHangHoa(1, 5).ToList();            
            return PartialView("_DanhSachSanPhamPartial", lst);
        }
        [HttpPost]
        public async Task<PartialViewResult> DanhSachSanPham(int indexPage)
        {
            int totalPage = (db.HangHoas.Count() / 5) + 1;
            indexPage = indexPage > totalPage ? 1 : indexPage;
            if (indexPage < 0)
            {
                ViewBag.NameError = "Trang yêu cầu không tồn tại";
                return PartialView("Error");
            }
            List<HangHoa> lst = Class.SubPage.LstHangHoa(indexPage, 5).ToList();
            return PartialView("_DanhSachSanPhamPartial", lst);
        }
        [HttpGet]
        public PartialViewResult ListPage()
        {
            int c = db.HangHoas.Count();
            ViewBag.TotalPage = (c % 5) == 0 ?
                  (db.HangHoas.Count() / 5) :
                  (db.HangHoas.Count() / 5) + 1;
            ViewBag.ActionName = "../SanPhams/DanhSachSanPham";
            return PartialView("ListPage");
        }
        [HttpPost]
        public ContentResult ListPagePost()
        {
            int c = db.HangHoas.Count();
            int  TotalPage = (c % 5) == 0 ?
                  (c / 5) :
                  (c / 5) + 1;         
            return Content(TotalPage.ToString()) ;
        }
    }
}

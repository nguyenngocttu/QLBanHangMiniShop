using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanHangMiniShop.Models;
namespace QLBanHangMiniShop.Controllers
{
    public class ProductManageController : Controller
    {
        // GET: ProductManagement
        public ActionResult Index()
        {
            ViewBag.lstLoaiSP = Class.ListShop.GetListChungLoai();
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(HangHoa hh,HttpPostedFileBase[] file)
        {
            
            QLBanHangDbContext db = new QLBanHangDbContext();
            hh.NgayTao = DateTime.Today;
            hh.NgayCapNhat = DateTime.Today;
            string Hinh = "";
            int dem = 1;            
            foreach (var item in file)
            {
                string[] f = item.FileName.Split('.');
                Hinh += hh.MaSo + "-" + dem + "." + f[1]+",";
                string url = Server.MapPath("~/Photos/" + hh.MaSo + "-" + dem+"."+f[1]);
                Class.DBFileControl.ResizeImg(Image.FromStream(item.InputStream), 256, url);
                dem++;
            }
            hh.TenHinh = Hinh;
            db.HangHoas.Add(hh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
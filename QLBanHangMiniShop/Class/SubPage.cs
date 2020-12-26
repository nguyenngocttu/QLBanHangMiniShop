using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLBanHangMiniShop.Models;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;

namespace QLBanHangMiniShop.Class
{
    public class SubPage
    {
        private static QLBanHangDbContext db = new QLBanHangDbContext();
        public static int totalPage = 0;
        //lấy danh sách chủng loại theo số lượng và chỉ số trang
        public static   IQueryable<ChungLoai> LstChungLoai(int indexPage,int sluong)
        {

            totalPage = (db.ChungLoais.Count()%sluong)==0?
                (db.ChungLoais.Count()/sluong):
                (db.ChungLoais.Count()/sluong)+1;
            if(indexPage==0)
            {
                var list = db.ChungLoais
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }    
            var lst = db.ChungLoais
                .OrderBy(cl => cl.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);          
                return lst;

              
           
        }
        //lấy danh sách loại theo số lượng và chỉ số trang
        public static IQueryable<Loai> LstLoai(int indexPage, int sluong)
        {

            totalPage = (db.Loais.Count() % sluong) == 0 ?
                (db.Loais.Count() / sluong) :
                (db.Loais.Count() / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.Loais.Include(l => l.ChungLoai)
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.Loais
                .OrderBy(cl => cl.ID).Include(l => l.ChungLoai)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;



        }
        //lấy toàn bộ hàng hóa theo theo số lượng và chỉ số trang
        public static IQueryable<HangHoa> LstHangHoa(int indexPage, int sluong)
        {

            totalPage = (db.HangHoas.Count() % sluong) == 0 ?
                (db.HangHoas.Count() / sluong) :
                (db.HangHoas.Count() / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HangHoas.Include("Loai")
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HangHoas.Include("Loai")
                .OrderBy(cl => cl.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;



        }
        //lấy hàng hóa theo chủng loại, số lượng và chỉ số trang
        public static IQueryable<HangHoa> LstHangHoa_CLoai(int cloaiSP,int indexPage, int sluong)
        {
            int c = db.HangHoas.Include("Loai").Where(h => h.Loai.ChungLoaiID == cloaiSP).Count();
            totalPage = ( c% sluong) == 0 ?
                (c / sluong) :
                (c / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HangHoas
               .OrderBy(cl => cl.NgayTao)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HangHoas.Include("Loai").Where(h => h.Loai.ChungLoaiID == cloaiSP)
                .OrderBy(cl => cl.NgayTao)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;



        }
        //lấy danh sách tất cả hóa đơn theo số lượng và chỉ số trang
        public static IQueryable<HoaDon> LstHoaDon(int indexPage,int sluong)
        {
            int count = db.HoaDons.Count();
            totalPage = (count % sluong) == 0 ?
                (count / sluong) :
                (count / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HoaDons
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HoaDons
                .OrderBy(cl => cl.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;
        }
        //lấy hóa đơn theo trang thái của hóa đơn
        public static IQueryable<HoaDon>LstHoaDon_Status(int indexPage,int sluong,string tThai)
        {
            int count = db.HoaDons.Where(hd => hd.TrangThai == tThai).Count();
            totalPage = (count % sluong) == 0 ?
                (count / sluong) :
                (count / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HoaDons
               .Where(hd => hd.TrangThai == tThai)
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HoaDons
                .Where(hd => hd.TrangThai == tThai)
                .OrderBy(cl => cl.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;
        }
        /// <summary>
        /// lấy danh sách id sản phẩm với số lượng sản phẩm  theo trang và id loại
        /// </summary>
        /// <param name="indexPage"></param>
        /// <param name="sluong"></param>
        /// <param name="idLoai"></param>
        /// <returns></returns>
        public static IQueryable<HangHoa>LstHanHoaNew_Loai(int indexPage,int sluong,int idLoai)
        {

            //lấy danh sách id sản phẩm mới trong bảng hanhhoamoi
            
            int count = db.HangHoas.Where(x => x.Moi == true&&x.LoaiID==idLoai).Count();
            totalPage = (count % sluong) == 0 ?
                (count / sluong) :
                (count / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HangHoas
               .Where(x => x.LoaiID == idLoai)
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HangHoas
                .Where(x => x.LoaiID == idLoai)
                .OrderBy(cl => cl.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;
        }
        /// <summary>
        /// lấy danh sách số lượng sản phẩm tại theo trang và id chủng  loại
        /// </summary>
        /// <param name="indexPage"></param>
        /// <param name="sluong"></param>
        /// <param name="idLoai"></param>
        /// <returns></returns>
        public static IQueryable<HangHoa> LstHangHoaNew_CLoai(int indexPage, int sluong, int idCLoai)
        {

            //lấy danh sách id sản phẩm mới trong bảng hanhhoamoi

            int count = db.HangHoas.Include("ChungLoai").Where(x => x.Moi==true&&x.Loai.ChungLoaiID==idCLoai).Count();

            totalPage = (count % sluong) == 0 ?
                (count / sluong) :
                (count / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HangHoas.Include("ChungLoai")
               .Where(x => x.Loai.ChungLoaiID == idCLoai&&x.Moi==true)
               .OrderBy(cl => cl.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HangHoas
                .Where(x => x.Loai.ChungLoaiID == idCLoai&&x.Moi==true)
                .OrderBy(cl => cl.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;
        }
        /// <summary>
        /// lấy danh sách số lượng sản phẩm tại theo trang 
        /// </summary>
        /// <param name="indexPage"></param>
        /// <param name="sluong"></param>
        /// <param name="idLoai"></param>
        /// <returns></returns>
        public static IQueryable<HangHoa> LstHangHoaNew_All(int indexPage, int sluong)
        {
            int count = db.HangHoas.Where(x => x.Moi == true).Count();
            totalPage = (count % sluong) == 0 ?
                (count / sluong) :
                (count / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HangHoas.Where(x => x.Moi == true)
               .OrderBy(x => x.ID)
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HangHoas.Where(x => x.Moi == true)
                .OrderBy(x => x.ID)
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;
        }
        public static IQueryable<HangHoa> LstHangHoa_AllTopSale(int indexPage,int sluong)//cần lấy theo loại hay chủng loại ra ngoài thêm điều kiện
        {

            int count = db.HangHoas.Include("HoaDonChiTiet").OrderBy(x => x.HoaDonChiTiets.Count()).Count();

            totalPage = (count % sluong) == 0 ?
                (count / sluong) :
                (count / sluong) + 1;
            if (indexPage == 0)
            {
                var list = db.HangHoas.Include("HoaDonChiTiet").OrderBy(x => x.HoaDonChiTiets.Count())                     
               .Skip((totalPage - 1) * sluong)
               .Take(sluong);
                return list;
            }
            var lst = db.HangHoas.Include("HoaDonChiTiet").OrderBy(x => x.HoaDonChiTiets.Count())
                .Skip((indexPage - 1) * sluong)
                .Take(sluong);
            return lst;

        }
        

    }
}
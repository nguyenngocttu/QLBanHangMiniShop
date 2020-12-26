
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QLBanHangMiniShop.Models;
namespace QLBanHangMiniShop.Class
{
    public class ListShop
    {
        public static QLBanHangDbContext db = new QLBanHangDbContext();
        public static List<ChungLoai> lstCL = db.ChungLoais.Include("Loais")
                .Where(cl => cl.Loais.Count > 0)
                .ToList();
      
        public static int PageCount;
        public static List<ChungLoai> GetAllListChungLoai()
        {
            return db.ChungLoais.ToList();
        }
        /// <summary>
        /// lấy danh sách chủng loại
        /// </summary>
        /// <returns></returns>
        public static List<ChungLoai> GetListChungLoai()//Chủng loại hàng hóa có loại hàng hóa hàng hóa có thể k có
        {
            
            return lstCL;

                
        }
        public static List<ChungLoai> GetListChungLoai2()//chủng loại hàng hóa có hàng hóa
        {
              List<ChungLoai> lst = new List<ChungLoai>();
            
            foreach (ChungLoai itemCL in lstCL)
                foreach (Loai item in itemCL.Loais)
                {
                    if (db.HangHoas.Where(hh => hh.LoaiID == item.ID).Count() > 0)
                    {
                        lst.Add(itemCL);
                    }
                }
            return lst;
        }
        /// <summary>
        /// lấy ra hàng hóa mới thêm vào theo chung loại và số lương
        /// </summary>
        /// <param name="MachungLoai"></param>
        /// <param name="soluong"></param>
        /// <returns></returns>
        public static List<HangHoa> GetListHangHoaMoi(int MachungLoai,int soluong)
        {
            return db.HangHoas.Where(hh => hh.LoaiID == MachungLoai)
                   .OrderByDescending(hh => hh.NgayTao).Take(soluong).ToList();
        }
        /// <summary>
        /// loaiSP=null:sluong=null=>trả về danh sách tât cả hàng hóa
        /// loaiSP!=null: sluong=null=>trả về danh sách tất cả hàng hóa của loaiSP
        /// </summary>
        /// <param name="loaiSP"></param>
        /// <param name="sluong"></param>
        /// <returns></returns>
        public static List<HangHoa>GetListHangHoa(int? loaiSP,int? sluong,int? indexPage)
        {
            int sl,loai,iPage;
            int.TryParse(loaiSP.ToString(), out loai);
            int.TryParse(sluong.ToString(), out sl);
            int.TryParse(indexPage.ToString(), out iPage);
            List<HangHoa> lst = new List<HangHoa>();
            if (loai==0)
            {
                return lst;
            }
            else
            {
                if (sl == 0)
                    sl = 3;
                PageCount = (int)db.HangHoas.Where(hh => hh.Loai.ChungLoaiID == loai).Count()/sl;                                
                    if(iPage==0)
                    {
                        iPage = 1;
                    }
                    return db.HangHoas.Where(hh => hh.Loai.ChungLoaiID == loai).ToList()
                            .Skip((iPage - 1) * sl).Take(sl).ToList();
                                                                   
            }
            
        }
        public static HangHoa GetHangHoas(int id)
        {
            HangHoa hh = db.HangHoas.Find(id);
            return hh;
        }
    }
}
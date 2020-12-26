using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBanHangMiniShop.Models
{
    public partial class HangHoa
    {
        public List< string> hinhDaiDien
        {
            get
            {
                List<string> hinh = new List<string>();
                if (TenHinh == null)
                {
                    hinh.Add( "upload.png");
                    return hinh;
                }
                else
                {
                    if (TenHinh.Contains(","))
                    {
                        hinh.AddRange(TenHinh.Split(','));
                    }
                    else
                    {
                        hinh.Add(TenHinh);
                    }
                    return hinh;
                }
            }
        }
    }
}
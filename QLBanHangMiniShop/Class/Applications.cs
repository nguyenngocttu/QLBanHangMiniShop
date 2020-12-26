using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBanHangMiniShop.Class
{
    public class Applications
    {
        public static void UpdateVisitorOnline(bool plus)
        {
            int countVistorOn;//chứa giá trị của của VistorOnline
            countVistorOn = int.Parse(HttpContext.Current.Application["VisitorOnline"].ToString());
            if (plus)
            {
                countVistorOn++;
            }
            else
            {
                countVistorOn--;
            }//code quen tay luôn:)))note lại cho nhớ
            HttpContext.Current.Application["VisitorOnline"] = countVistorOn.ToString();
        }
    }
}
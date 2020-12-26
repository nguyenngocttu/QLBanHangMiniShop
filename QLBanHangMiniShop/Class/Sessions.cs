using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBanHangMiniShop.Class
{
    public class SessionsAccess
    {
      
            
        public AccessMode modeAccess { get; set; }       
        public string value { get; set; }
        public static void SetSessionAccess(AccessMode accessMode)
        {
            if (HttpContext.Current.Session["Access"] == null)
            {
                SessionsAccess access = new SessionsAccess()
                {
                    modeAccess = accessMode,
                    value =HttpContext.Current.Session.SessionID + accessMode.ToString()
                };
                HttpContext.Current.Session["Access"] = access;
                HttpContext.Current.Session.Timeout = 1;
                Applications.UpdateVisitorOnline(true);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace QLBanHangMiniShop.Class
{
    public class DBFileControl
    {
        public static bool ResizeImg(Image images,int width,string path)
        {

            try
            {
                //lấy chiều dài và chiều rộng của ảnh
                int W = images.Width;
                int H = images.Height;
                //thiết lập chiều dài và chiều rộng mới
                int newW = width;
                int newH = (H * width) / W;
                Bitmap b = new Bitmap(newH, newW);
                Graphics g = Graphics.FromImage((Image)b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(images, 0, 0, newW, newH);
                g.Dispose();
                b.Save(path);
                return true;
            }
            catch
            {
                return true;
            }
           
        }
    }
}
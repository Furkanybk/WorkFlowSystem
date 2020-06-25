using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WFS.business.SessionSettings;
using WFS.db.Tables;
using WFS.web.Models; 

namespace WFS.web.Utilities
{
    public class ImageProcess : IDisposable
    {
        public string Resolution(HttpPostedFile Image, int[] ImageSize, string fileName, string dir)
        { 
            string gui = Guid.NewGuid().ToString();
            string time = DateTime.Now.ToString("HHmm");
            string fname = $"{time}_{gui}.{Image.ContentType.Split('/')[1]}";
            var mainFolder = HttpContext.Current.Server.MapPath($"~/Images/" + dir + "/" + fileName);

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            else
            {
                Directory.Delete(mainFolder, true);
                Directory.CreateDirectory(mainFolder); 
            }

            foreach (var size in ImageSize)
            {
                var folder = HttpContext.Current.Server.MapPath($"~/Images/" + dir + "/" + fileName + "/" + size.ToString());

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                else
                {
                    Directory.Delete(folder, true);
                    Directory.CreateDirectory(folder);
                }

                System.Drawing.Image uploadedImagae = System.Drawing.Image.FromStream(Image.InputStream);
                Bitmap processedImage = null;
                Graphics graphicThing = null;

                int width = size, height = size, new_width, new_height;

                new_height = (int)Math.Round(((float)uploadedImagae.Height * (float)size) / (float)uploadedImagae.Width);
                new_width = width;
                height = new_height;
                new_width = new_width > width ? width : new_width;
                new_height = new_height > height ? height : new_height;

                processedImage = new Bitmap(size, size);
                graphicThing = Graphics.FromImage(processedImage);
                graphicThing.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, size, size));

                int x = (width - new_width) / 2;
                int y = (height - new_height) / 2;

                graphicThing.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphicThing.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphicThing.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                graphicThing.DrawImage(uploadedImagae, x, y, size, size);

                ImageCodecInfo codec = ImageCodecInfo.GetImageEncoders()[1];
                EncoderParameters eParams = new EncoderParameters(1);
                eParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L); 
                 
                processedImage.Save(HttpContext.Current.Server.MapPath($"~/Images/{dir}/{fileName}/{size.ToString()}/{fname}"), codec, eParams);
            }

            return fname;
        } 

        public void watermark(Graphics graphicThing, int height, int width)
        {
            Font writeType = new Font("Century Schoolbook", 12, FontStyle.Italic);
            Brush brushStyle = new SolidBrush(Color.WhiteSmoke);
            Point point = new Point(width / 3, height - 30);
            graphicThing.DrawString("kodlamamerkezi.com", writeType, brushStyle, point);
        } 
         
        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected bool Disposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        } 
        #endregion
    }
}
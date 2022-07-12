using RegistrationDirectory.Service.Absract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RegistrationDirectory.Service.Concrete
{
    public class Watermark
    {
        private readonly ICustomerService _customerService;

        public Watermark(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public Image waterMark()
        {


            

            byte[] imageBytes = _customerService.GetById(2).Photograph.ToArray();
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);

                Bitmap bmp = new Bitmap(image.Width, image.Height);
                Graphics gra = Graphics.FromImage(bmp);
                gra.DrawImageUnscaled(image, new Point(0, 0));
                gra.Dispose();

                string belgelerim = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                bmp.Save(belgelerim + "\\resimdosyasi.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
              
                bmp.Dispose();


                var siteName = "wwww.mysite.com";

                using var img = Image.FromFile(belgelerim + "\\resimdosyasi.jpg");

                using var graphic = Graphics.FromImage(img);

                var font = new Font(FontFamily.GenericMonospace, 40, FontStyle.Bold, GraphicsUnit.Pixel);

                var textSize = graphic.MeasureString(siteName, font);

                var color = Color.FromArgb(255, 255, 0, 0);
                var brush = new SolidBrush(color);

                var position = new Point(img.Width - ((int)textSize.Width + 30), img.Height - ((int)textSize.Height + 30));


                graphic.DrawString(siteName, font, brush, position);

                img.Save(belgelerim + "\\resimdosyasiwithwatermark.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);


                img.Dispose();
                graphic.Dispose();













                return image;
            }

            
        }
    }
}

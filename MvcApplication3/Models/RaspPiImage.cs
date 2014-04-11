using System.IO;
using System.Web;
using System.Drawing;

namespace MvcApplication3.Models
{
    public class RaspPiImage
    {
        public RaspPiImage(HttpPostedFileBase file)
        {
            FileName = file.FileName;
            HttpStream = file.InputStream;
            ContentLength = file.ContentLength;
            ContentType = file.ContentType;
            FileName = file.FileName;
            ImageFile = getImage(file.InputStream);
            BmpFile = GetBmpImage(file.InputStream);
        }


        public string ContentType { get; set; }

        public int ContentLength { get; set; }

        public System.Drawing.Image ImageFile { get; set; }

        public Bitmap BmpFile { get; set; }

        public Stream HttpStream { get; set; }

        public string FileName { get; set; }

        public byte[] ContentByte()
        {
            var converter = new ImageConverter();
            var array = (byte[])converter.ConvertTo(ImageFile, typeof(byte[]));

            return array;
        }


        private System.Drawing.Image getImage(System.IO.Stream httpStream)
        {
            var image = System.Drawing.Image.FromStream(HttpStream);

            return image;
        }

        private Bitmap GetBmpImage(System.IO.Stream httpStream)
        {
            var image = System.Drawing.Image.FromStream(HttpStream);

            Graphics g = Graphics.FromImage(new Bitmap(image));

            g.DrawString("Test", new Font("Arial", 12), Brushes.Red, 10, 10);
            g.Save();
            return new Bitmap(image.Width, image.Height, g);
        }
    }



}
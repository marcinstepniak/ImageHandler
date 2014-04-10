﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Web.UI.WebControls;

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

        public System.IO.Stream HttpStream { get; set; }

        public string FileName { get; set; }

        public byte[] ContentByte 
        {
            get
            {
                var image = 
                    ImageFile; //(System.Drawing.Image)BmpFile;

                byte[] array;
                using (MemoryStream m = new MemoryStream())
                {
                   image.Save(m,ImageFormat.Png);
                    m.Close();
                    array = m.ToArray();
                }

                return array;
            }
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

            g.DrawString("Test", new Font("Arial",12), Brushes.Red, 10,10) ;
            g.Save();
            return new Bitmap(image.Width,image.Height,g);
        }
    }



}
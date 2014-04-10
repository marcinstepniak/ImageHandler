using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using System.Web.HttpPostedFileBase;
using MvcApplication3.Models;

namespace MvcApplication3.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public static Queue<RaspPiImage> ImgCollection
        {
            get
            {
                if (imgCollection == null)
                    imgCollection = new Queue<RaspPiImage>();
                return
                    imgCollection;
            }
            set { imgCollection = value; }
        }

        private static Queue<RaspPiImage> imgCollection;
            
        [HttpPost]
        public ActionResult Index()
        {
            RaspPiImage Model = null;

            var file = Request.Files[0];

            if (file != null)
            {
                Model = new RaspPiImage(file);
              
                ImgCollection.Enqueue(Model);
            }
            return View(Model);
        }

        public ActionResult GetLastImage()
        {
            return View(new object());
        }

        public void GetLastImageContent()
        {
            if (imgCollection==null || imgCollection.Count == 0)
                return;

            var model = imgCollection.Count == 1 ? imgCollection.ElementAt(0) : imgCollection.Dequeue();
            Response.Clear();
            Response.ContentType = model.ContentType;
            Response.ContentEncoding = Encoding.UTF8;
            Response.BufferOutput = false;
            Response.AppendHeader("content-disposition", string.Concat("attachment; filename=", model.FileName));

            Response.BinaryWrite(model.ContentByte);
            Response.OutputStream.Flush();

        }

    }
}

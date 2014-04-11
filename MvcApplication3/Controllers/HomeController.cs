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
        public static Queue<RaspPiImage> ImgCollection = new Queue<RaspPiImage>();

        //
        // GET: /Home/
        [HttpPost]
        public ActionResult Index()
        {

            if (Request.Files.Count < 1)
                return null;

            var file = Request.Files[0];

            RaspPiImage model = null;
            if (file != null)
            {
                model = new RaspPiImage(file);

                lock (ImgCollection)
                {
                    if (ImgCollection.Count > 3)
                    {
                        ImgCollection.Dequeue();
                    }

                    ImgCollection.Enqueue(model);
                }
            }

            return View(model);
        }

        public ActionResult GetLastImage()
        {
            lock (ImgCollection)
            {
                ImgCollection.Clear();
            }

            return View(new object());
        }

        public void GetLastImageContent()
        {
            lock (ImgCollection)
            {
                if (ImgCollection == null || ImgCollection.Count == 0)
                    return;

                var model = ImgCollection.Count == 1 ? ImgCollection.ElementAt(0) : ImgCollection.Dequeue();
                Response.Clear();
                Response.ContentType = model.ContentType;
                Response.ContentEncoding = Encoding.UTF8;
                Response.BufferOutput = false;
                Response.AppendHeader("content-disposition", string.Concat("attachment; filename=", model.FileName));

                if (Response.IsClientConnected)
                {
                    try
                    {
                        Response.BinaryWrite(model.ContentByte());
                        Response.OutputStream.Flush();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

    }
}

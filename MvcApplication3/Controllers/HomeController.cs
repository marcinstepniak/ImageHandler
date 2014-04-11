using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MvcApplication3.Models;

namespace MvcApplication3.Controllers
{
    public class HomeController : Controller
    {
        public static RaspPiImage Image;

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

                if (Image == null)
                {
                    Image = model;
                }
                else
                {
                    lock (Image)
                    {
                        Image = model;
                    }
                }
            }

            return View(model);
        }

        public ActionResult GetLastImage()
        {
            return View(new object());
        }

        public void GetLastImageContent()
        {
            if (Image == null)
                return;

            lock (Image)
            {

                Response.Clear();
                Response.ContentType = Image.ContentType;
                Response.ContentEncoding = Encoding.UTF8;
                Response.BufferOutput = false;
                Response.AppendHeader("content-disposition", string.Concat("attachment; filename=", Image.FileName));

                if (Response.IsClientConnected)
                {
                    try
                    {
                        Response.BinaryWrite(Image.ContentByte());
                        Response.OutputStream.Flush();
                    }
                    catch (Exception)
                    { }
                }
            }
        }

    }
}

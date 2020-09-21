using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebGUI.Models;

namespace WebGUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Data()  //Get the data
        {
            WebRequest request = WebRequest.Create("https://localhost:44395/api/Speakers/");
            WebResponse response = request.GetResponse();
            using (Stream streamer = response.GetResponseStream())
            {
                StreamReader read = new StreamReader(streamer);
                string serverRead = read.ReadToEnd();
                var deserialize = JsonConvert.DeserializeObject<List<Speaker>>(serverRead);
                return View(deserialize);
        }    }


        [HttpGet]  //Post page
        public ActionResult Create()
        {
            Speaker posting = new Speaker();
            return View(posting);
        }


        [HttpPost] //Post code
        public ActionResult Create(Speaker postParameters)
        {
            string postData = JsonConvert.SerializeObject(postParameters);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://localhost:44395/api/Speakers/");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.ContentType = "application/json";
            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Count());
            }
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();


           return RedirectToAction("Data");
        }

        public ActionResult About()
        {
            ViewBag.Message = "An MVC example on how to create and use API";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
    }
}
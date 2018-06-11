using Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FirstController : Controller
    {
        static ImageWebModel imageWebModel  = new ImageWebModel();
        static PhotosModel photosModel;
        static ConfigModel configModel = new ConfigModel();
        static LogsModel logsModel = new LogsModel();
        static DeletePhotoModel deletePhotoModel = new DeletePhotoModel();

        /// <summary>
        /// action for confirm hendler delete page
        /// </summary>
        /// <param name="name">the name of the handler</param>
        /// <returns></returns>
        public ActionResult DeleteHendlerView(string name)
        {
            return View((object)name);
        }

        /// <summary>
        /// action happens when a handler is deleted. redirect to config after delete.
        /// </summary>
        /// <param name="name">handler name</param>
        /// <returns></returns>
        public ActionResult DeleteHendler(string name)
        {
            configModel.deleteHendler(name);
            return RedirectToAction("ConfigView");
        }

        /// <summary>
        /// main page view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ImageWebView()   
        {
            configModel.getConfig();
            imageWebModel.CountNumOfPictures(configModel.OutputDirectory);
            return View(imageWebModel);
        }

        /// <summary>
        /// log page view.
        /// </summary>
        /// <param name="searchString">string in search box</param>
        /// <returns></returns>
        public ActionResult LogsView(string searchString)
        {
            //get all logs
            logsModel.getLogs();
            var list = from l in logsModel.MessageList select l;
            //check if there is string in search box
            if (!String.IsNullOrEmpty(searchString))
            {
                //choose only logs where the type is the type that was searched.
                list = list.Where(s => s.Status.ToString().Contains(searchString));
            }
            return View(list);
        }

        /// <summary>
        /// view photo page.
        /// </summary>
        /// <param name="name">name of photo</param>
        /// <param name="year">year that the photo was taken</param>
        /// <param name="month">minth that the photo was taken</param>
        /// <returns></returns>
        public ActionResult ViewPhotoView(string name, string year, string month)
        {
            string yearNum = year.Split(':')[1];
            string monthNum = month.Split(':')[1];
            //create path of original photo
            string path = "../PhotosOutput" + "/" + yearNum + "/" + monthNum + "/" + name;
            ViewPhotoModel viewPhotoModel = new ViewPhotoModel(new Photo(path,name,year,month));
            return View(viewPhotoModel);
        }

        /// <summary>
        /// confirm delete photo page
        /// </summary>
        /// <param name="name">name of photo</param>
        /// <param name="year">year that the photo was taken</param>
        /// <param name="month">month that the photo was taken</param>
        /// <returns></returns>
        public ActionResult DeletePhotoView(string name, string year, string month)
        {
            string yearNum = year.Split(':')[1];
            string monthNum = month.Split(':')[1];
            //create thumbnail path
            string path = "../PhotosOutput/Thumbnails" + "/" + yearNum + "/" + monthNum + "/" + name;
            deletePhotoModel.photo = (new Photo(path, name, year, month));
            return View(deletePhotoModel);
        }

        /// <summary>
        /// action happens when a photo is deleted. redirect to photos after delete.
        /// </summary>
        /// <returns></returns>
        public ActionResult DeletePhoto()
        {
            deletePhotoModel.Delete();
            return RedirectToAction("PhotosView");
        }

        /// <summary>
        /// photos page view.
        /// </summary>
        /// <returns></returns>
        public ActionResult PhotosView()
        {
            configModel.getConfig();
            photosModel = new PhotosModel(configModel.OutputDirectory);
            return View(photosModel);
        }

        /// <summary>
        /// config page view.
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigView()
        {
            configModel.getConfig();
            return View(configModel);
        }
    }
}

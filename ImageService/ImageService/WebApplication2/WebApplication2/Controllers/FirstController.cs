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


        static List<Employee> employees = new List<Employee>()
        {
          new Employee  { FirstName = "Moshe", LastName = "Aron", Email = "Stam@stam", Salary = 10000, Phone = "08-8888888" },
          new Employee  { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 2000, Phone = "08-8888888" },
          new Employee   { FirstName = "Mor", LastName = "Sinai", Email = "Stam@stam", Salary = 500, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 20, Phone = "08-8888888" },
          new Employee   { FirstName = "Dor", LastName = "Nisim", Email = "Stam@stam", Salary = 700, Phone = "08-8888888" }
        };
        // GET: First
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AjaxView()
        {
            return View();
        }

        [HttpGet]
        public JObject GetEmployee()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }

        [HttpPost]
        public JObject GetEmployee(string name, int salary)
        {
            foreach (var empl in employees)
            {
                if (empl.Salary > salary || name.Equals(name))
                {
                    JObject data = new JObject();
                    data["FirstName"] = empl.FirstName;
                    data["LastName"] = empl.LastName;
                    data["Salary"] = empl.Salary;
                    return data;
                }
            }
            return null;
        }

        // GET: First/Details
        public ActionResult Details()
        {
            return View(employees);
        }

        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            try
            {
                employees.Add(emp);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Employee emp in employees) {
                if (emp.ID.Equals(id)) { 
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Employee empT)
        {
            try
            {
                foreach (Employee emp in employees)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Employee emp in employees)
            {
                if (emp.ID.Equals(id))
                {
                    employees.RemoveAt(i);
                    return RedirectToAction("Details");
                }
                i++;
            }
            return RedirectToAction("Error");
        }

        public ActionResult DeleteHendlerView(string name)
        {
            return View((object)name);
        }
        public ActionResult DeleteHendler(string name)
        {
            configModel.deleteHendler(name);
            return RedirectToAction("ConfigView");
        }
        public ActionResult ImageWebView()   
        {
            configModel.getConfig();
            imageWebModel.CountNumOfPictures(configModel.OutputDirectory);
            return View(imageWebModel);
        }
        public ActionResult LogsView(string searchString)
        {
            logsModel.getLogs();
            var list = from l in logsModel.MessageList select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                list = list.Where(s => s.Status.ToString().Contains(searchString));
            }
            return View(list);
        }

        public ActionResult ViewPhotoView(string name, string year, string month)
        {
            string yearNum = year.Split(':')[1];
            string monthNum = month.Split(':')[1];
            string path = "../PhotosOutput" + "/" + yearNum + "/" + monthNum + "/" + name;
            ViewPhotoModel viewPhotoModel = new ViewPhotoModel(new Photo(path,name,year,month));
            return View(viewPhotoModel);
        }

        public ActionResult DeletePhotoView(string name, string year, string month)
        {
            string yearNum = year.Split(':')[1];
            string monthNum = month.Split(':')[1];
            string path = "../PhotosOutput/Thumbnails" + "/" + yearNum + "/" + monthNum + "/" + name;
            deletePhotoModel.photo = (new Photo(path, name, year, month));
            return View(deletePhotoModel);
        }

        public ActionResult DeletePhoto()
        {
            deletePhotoModel.Delete();
            return RedirectToAction("PhotosView");
        }

        public ActionResult PhotosView()
        {
            photosModel = new PhotosModel();
            return View(photosModel);
        }
        public ActionResult ConfigView()
        {
            configModel.getConfig();
            return View(configModel);
        }
    }
}

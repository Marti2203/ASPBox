﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Mvc;
using BoxApp.Models;

namespace BoxApp.Controllers
{
    public class ATPController : Controller
    {
        private IList<BoxModel> _models;//The "database" for the boxes

        public ATPController()
        {
            _models = SingletonBoxList.Models ; //Create the instance of the Singleton
        }

        //Open index page
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //Create a new Box Model if the previous session had created one and show page for Box Creation
        public ActionResult Create()
        {
            BoxModel viewModel = (BoxModel)TempData["viewModel"] ?? new BoxModel();

            return View(viewModel);
        }

        [HttpGet]
        //List all Boxes
        public ActionResult List()
        {
            return View(_models);
        }
        //Delete a box with the specified id
        public ActionResult Delete(int id)
        {
            _models.Remove(_models.Where(box => box.ID == id).FirstOrDefault());//Find box if there exists one
            return RedirectToAction("List");
        }

        //Edit information of a specified by id box
        public ActionResult Edit(int id)
        {
            BoxModel model = _models.Where(element => element.ID == id).FirstOrDefault();//Find box if there exists one
            return View(model);
        }

        [HttpPost]
        //Edit box with a specified BoxModel
        public ActionResult Edit(BoxModel viewModel)
        {
            if (ModelState.IsValid)
            {
                BoxModel old = _models.Where(model => model.ID == viewModel.ID).FirstOrDefault();
                foreach (PropertyInfo property in old.GetType().GetProperties())
                {
                    property.SetValue(old, property.GetValue(viewModel));
                }

                return RedirectToAction("List");
            }
            else
                return RedirectToAction("Edit",new { id= viewModel.ID });
        }

        [HttpPost]
        //Output information of created box
        public ActionResult RequestBox(BoxModel viewModel)
        {
            //Show a view consisting of the fields of the box
            if (ModelState.IsValid)
            {
                string message = string.Format("The request for box with with size {0}:{1}:{2} and weight {3}, color {4} and material {5} was successfully accepted."
                , viewModel.Width, viewModel.Height, viewModel.Length, viewModel.Weight, viewModel.Colour, viewModel.Material);

                ViewBag.SuccessMessage = message;
                viewModel.ID = _models.Last().ID+1;
                _models.Add(viewModel);
                Session["models"] = _models;
                return View();

            }
            //Go back to creating a box
            else
            {
                TempData["viewModel"] = viewModel;
                return RedirectToAction("Create");
            }
        }
    }
}
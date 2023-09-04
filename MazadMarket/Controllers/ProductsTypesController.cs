using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MazadMarket.Models;

namespace MazadMarket.Controllers
{
    public class ProductsTypesController : Controller
    {
        private Model1 db = new Model1();

        // GET: ProductsTypes
        public ActionResult Index()
        {
            return View(db.ProductsTypes.ToList());
        }

        public ActionResult Indexs()
        {
            return View(db.ProductsTypes.ToList());
        }

        // GET: ProductsTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            ProductsType productsType = db.ProductsTypes.Find(id);
            if (productsType == null)
            {
                return HttpNotFound();
            }
            return View(productsType);
        }

        // GET: ProductsTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductsType productsType, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                var allowedExtensions = new[] { ".Jpg", ".png", ".PNG", ".jpg", "jpeg" };

                var fileimage = file.ToString(); //getting complete url   MHD.php.png

                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + ext; //appending the name with id  
                                                      // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/ProductTypeImage"), myfile);

                    file.SaveAs(path);

                    productsType.Picture = myfile;
                }
                else
                {
                    ViewBag.message = "Please choose only Image file";
                }



                db.ProductsTypes.Add(productsType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productsType);
        }

        // GET: ProductsTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsType productsType = db.ProductsTypes.Find(id);
            if (productsType == null)
            {
                return HttpNotFound();
            }
            return View(productsType);
        }

        // POST: ProductsTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductsType productsType, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    var allowedExtensions = new[] { ".Jpg", ".jpg", ".png", ".PNG", ".jpg", ".jpeg" };

                    var fileimage = file.ToString(); //getting complete url  

                    var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                    string ext = Path.GetExtension(file.FileName).ToLower();//getting the extension(ex-.jpg)  



                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                        string myfile = name + "_" + ext; //appending the name with id  
                                                          // store the file inside ~/project folder(Img)  
                        var path = Path.Combine(Server.MapPath("~/ProductTypeImage"), myfile);

                        file.SaveAs(path);

                        productsType.Picture = myfile;
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                    }


                }


                db.Entry(productsType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productsType);
        }

        // GET: ProductsTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsType productsType = db.ProductsTypes.Find(id);
            if (productsType == null)
            {
                return HttpNotFound();
            }
            return View(productsType);
        }

        // POST: ProductsTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductsType productsType = db.ProductsTypes.Find(id);
            db.ProductsTypes.Remove(productsType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MazadMarket.Models;

namespace MazadMarket.Controllers
{
    public class ProductsAuctionsController : Controller
    {
        private Model1 db = new Model1();

        // GET: ProductsAuctions
        public ActionResult Index()
        {
            var productsAuctions = db.ProductsAuctions.Include(p => p.Products);
            return View(productsAuctions.ToList());
        }

        // GET: ProductsAuctions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsAuction productsAuction = db.ProductsAuctions.Find(id);
            if (productsAuction == null)
            {
                return HttpNotFound();
            }
            return View(productsAuction);
        }

        // GET: ProductsAuctions/Create
        public ActionResult Create()
        {
            ViewBag.ProductsId = new SelectList(db.Products, "Id", "productName");
            return View();
        }

        // POST: ProductsAuctions/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductsAuction productsAuction)
        {
            
            if (ModelState.IsValid)
            {
                db.ProductsAuctions.Add(productsAuction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductsId = new SelectList(db.Products, "Id", "productName", productsAuction.ProductsId);
            return View(productsAuction);
        }

        // GET: ProductsAuctions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); //error400
            }
            ProductsAuction productsAuction = db.ProductsAuctions.Find(id);
            if (productsAuction == null)
            {
                return HttpNotFound();//error 404
            }
            ViewBag.ProductsId = new SelectList(db.Products, "Id", "productName", productsAuction.ProductsId);
            return View(productsAuction);
        }

        // POST: ProductsAuctions/Edit/5
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductsId,userName,riseAmount,dateOfRise,Remarks")] ProductsAuction productsAuction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsAuction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index"); //save code and display changes
            }
            ViewBag.ProductsId = new SelectList(db.Products, "Id", "productName", productsAuction.ProductsId);
            return View(productsAuction);
        }

        // GET: ProductsAuctions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsAuction productsAuction = db.ProductsAuctions.Find(id);
            if (productsAuction == null)
            {
                return HttpNotFound();
            }
            return View(productsAuction);
        }

        // POST: ProductsAuctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductsAuction productsAuction = db.ProductsAuctions.Find(id);
            db.ProductsAuctions.Remove(productsAuction);
            db.SaveChanges();
            return RedirectToAction("Index"); //save code and display 
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

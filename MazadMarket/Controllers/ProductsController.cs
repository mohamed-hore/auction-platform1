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
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;

namespace MazadMarket.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {



        private Model1 db = new Model1();
        private string userId;



        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Where(a => a.userId == User.Identity.Name).Include(p => p.ProductsType);
            return View(products.ToList());
        }


        public ActionResult categoryProduct(int id, string search)
        {

            if (search == null)
            {

                var products = db.Products.Where(a => (a.ProductsTypeId == id && a.approved.Equals(true) && a.sold.Equals(false) && a.dateOfAuctionStart <= DateTime.Now && a.dateOfAuctionEnd >= DateTime.Now)).Include(p => p.ProductsType);
                return View(products.ToList());
            }
            else
            {

                var products = db.Products.Where(a => (a.ProductsTypeId == id && a.approved.Equals(true)
                && a.sold.Equals(false) && a.dateOfAuctionStart <= DateTime.Now && a.dateOfAuctionEnd >= DateTime.Now && a.productName.Contains(search)))
                .Include(p => p.ProductsType);
                return View(products.ToList());
            }



        }



        public ActionResult Form()
        {

            return View();
        }



        [HttpPost]
        public ActionResult Form(paymentConfirmation paymentConfirmation, string depositValue, string cCV, string visaCard, DateTime cardExpirationDate)


        {
            var person = User.Identity.GetUserId();


            var payment = new paymentConfirmation();
            payment.userId = User.Identity.Name;
            //payment.ProductsId = ProductsId;

            payment.depositValue = depositValue;
            payment.cCV = cCV;
            payment.visaCard = visaCard;
            payment.cardExpirationDate = cardExpirationDate;
            ViewBag.Result = " Payment Completed Successfully ";
            payment.canBid = true;
            db.paymentConfirmation.Add(payment);

            db.SaveChanges();
            return View();

            //return View();

        }



        public ActionResult categoryProductDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }

            var list = db.ProductsAuctions.Where(t => t.ProductsId == id).ToList();

            if (list.Count == 0)
            {
                ViewBag.MaxPrice = "There is No Raises Yet";
            }
            else
            {
                ViewBag.MaxPrice = "The Max  Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == id).Max(t => t.riseAmount);
            }

            products.ProductsAuction = list;

            return View(products);
        }


        public ActionResult categoryProductDetailsAuto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }

            var list = db.ProductsAuctions.Where(t => t.ProductsId == id).ToList();

            if (list.Count == 0)
            {
                ViewBag.MaxPrice = "There is No Raises Yest";
            }
            else
            {
                ViewBag.MaxPrice = "The Max  Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == id).Max(t => t.riseAmount);
            }

            products.ProductsAuction = list;
            return View(products);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> categoryProductDetails(int productId, string remarks, string raise)
        {
            bool first = false;
            var person = User.Identity.GetUserId();
            Products product = (Products)db.Products.Where(a => a.Id == productId).FirstOrDefault();
            double offer = Convert.ToDouble(raise);
            Products productnew = (Products)product;
            double maxOffer;
            var list = db.ProductsAuctions.Where(t => t.ProductsId == productId).ToList(); 
            double initialPrice = db.Products.Where(t => t.Id == productId).FirstOrDefault().Price;
            Console.WriteLine(product.userId);
            Console.WriteLine(User.Identity.Name);
            if (product.userId == User.Identity.Name) //owner
            {
                ModelState.AddModelError(string.Empty, "Sorry You Own This Product.");
                ////////////////
                if (list.Count == 0)
                {
                    ViewBag.MaxPrice = "There is No Raises Yet";
                }
                else
                {
                    ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);
                }
                product.ProductsAuction = list;
                return View(product);
            }





            if (list.Count == 0) //لايوجد اي زيادة
            {
                maxOffer = initialPrice;
                if ((maxOffer + product.minRaise) > offer)
                {
                    ModelState.AddModelError(string.Empty, "The Offered Price Is Less Than The Min Price");
                    if (list.Count == 0)
                    {
                        ViewBag.MaxPrice = "There is No Raises Yet";
                    }
                    else
                    {
                        ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);
                    }



                    product.ProductsAuction = list;
                    return View(product);
                }
                else
                {
                    first = true;

                }
            }
            else
            {
                if ((list.Max(t => t.riseAmount) + product.minRaise) < offer && (list.Max(t => t.riseAmount) + product.minRaise) > initialPrice)
                {
                    maxOffer = offer;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The Offered Price Is Less Than The Max Price");
                    if (list.Count == 0)
                    {
                        ViewBag.MaxPrice = "There is No Raises Yet";
                    }
                    else
                    {
                        ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);
                    }
                    product.ProductsAuction = list;
                    return View(product);
                }
            }

            //////////////////// success start 
            ProductsAuction productsAuction = new ProductsAuction();
            productsAuction.ProductsId = productId;
            productsAuction.riseAmount = Convert.ToInt32(raise);
            productsAuction.userId = User.Identity.Name;
            productsAuction.Remarks = remarks;
            productsAuction.dateOfRise = DateTime.Now;
            db.ProductsAuctions.Add(productsAuction);
            db.SaveChanges();


            if (first)
            {
                ViewBag.MaxPrice = "The Max Price :   " + raise;

            }
            else
            {
                if (list.Count == 0)
                {
                    ViewBag.MaxPrice = "There is No Raises Yet";
                }
                else
                {
                    ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);

                }
            }
            ////////
            list.Add(productsAuction);
            product.ProductsAuction = list;

           await oneUsercall(productId);
            //manyUser(productId);

            return View(product);

            ///////////////////// success End 





            return View(product);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> categoryProductDetailsAuto(int productId, string valueUP, string limitPrice)
        {
            Products product = db.Products.Where(a => a.Id == productId).FirstOrDefault();
            var person = User.Identity.GetUserId();
            var list = db.ProductsAuctions.Where(t => t.ProductsId == productId).ToList();
            double vauleBidding = Convert.ToDouble(valueUP);
            double stop_condition = Convert.ToDouble(limitPrice);
            double initialPrice = db.Products.Where(t => t.Id == productId).FirstOrDefault().Price;
            double maxOffer;


            if (vauleBidding > stop_condition)
            {


                ModelState.AddModelError(string.Empty, "You should Enter the vauleBidding less Than SecretPrice.");

                return View(product);
            }




            if (list.Count == 0) //لايوجد اي زيادة من قبل 
            {

                if ((initialPrice + product.minRaise) > vauleBidding)
                {
                    ModelState.AddModelError(string.Empty, "The Offered Price Is Less Than The Min Price");
                }

                else
                { ///////// صار مزايدة
                    maxOffer = vauleBidding;
                    ViewBag.MaxPrice = "The Max Price :   " + maxOffer;
                }

                ///////////////////  succsess start

                await AddNewAuction(person, productId, vauleBidding, stop_condition);



                //////////////////// success end 
                product.ProductsAuction = list;
                return View(product);

            }





            var riseAmount = await GetMaxRaisForProduct(productId);


            if ((db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount) + product.minRaise) > vauleBidding + stop_condition)
            {
                ModelState.AddModelError(string.Empty, "The Offered Price Is Less Than The Min Price");
                if (list.Count == 0)
                {
                    ViewBag.MaxPrice = "There is No Raises Yet";
                }
                else
                {

                    ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);
                }
                product.ProductsAuction = list;
                return View(product);
            }
            maxOffer = db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount) + vauleBidding;

            ViewBag.MaxPrice = "The Max Price :   " + maxOffer;
            string lastUserBidding = User.Identity.GetUserId();

            stop_condition = stop_condition - vauleBidding;

            /////////////////// success start 

            await AddNewAuction(person, productId, vauleBidding, stop_condition, "manual");


            //////////////////// success end 

             product.ProductsAuction = list;
            return View(product);

        }


        public async Task oneUsercall(int productId)
        {
            var autoUser = await getMaxinimAutoUser(productId);

            var riseAmount = await GetMaxRaisForProduct(productId);


            var newraisAmoutn = riseAmount + autoUser.riseAmount;


            await AddNewAuction(autoUser.userId, productId, newraisAmoutn, autoUser.limitPrice - newraisAmoutn, "autoEncriminting");

            #region commint

            //// /////// get the max of value from BD to passing 
            //////var limitPrice2 = db.ProductsAuctions.Where(a => a.limitPrice == productId).ToList();



            ////var limitPrice = db.ProductsAuctions.Where(t => t.ProductsId == productId).Min(t => t.limitPrice);
            ////var valueUP = db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.valueUP);

            ////var users = db.ProductsAuctions.Where(x => x.limitPrice != null).Take(1).FirstOrDefault().userId;

            ////var productsauctionlist = db.ProductsAuctions.Where( x => x.limitPrice != null && x.ProductsId == productId).OrderByDescending(x=>x.limitPrice).Take(1).FirstOrDefault();


            //////var users = db.ProductsAuctions.Where(x => x.limitPrice != null && x.userId != null).ToList();

            //////foreach (var productsauction in productsauctionlist)
            //////{
            //////    Console.WriteLine(user.userId);

            //////    name = i.userId.ToString();

            //////}

            //riseAmount =  valueUP + riseAmount;

            //if (limitPrice != null)
            //{
            //    if (limitPrice != 0 && riseAmount < limitPrice)


            //    {
            //        limitPrice = limitPrice - valueUP;


            //        Products product = (Products)db.Products.Where(a => a.Id == productId).FirstOrDefault();
            //        Products productnew = (Products)product;
            //        var list = db.ProductsAuctions.Where(t => t.ProductsId == productId).ToList();
            //        double vauleBidding = Convert.ToDouble(valueUP);
            //        double stop_condition = Convert.ToDouble(limitPrice);
            //        double maxOffer;
            //        double initialPrice = db.Products.Where(t => t.Id == productId).FirstOrDefault().Price;

            //        vauleBidding = valueUP;




            //        if (vauleBidding > stop_condition && riseAmount > limitPrice)
            //        {


            //            ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);


            //        }


            //        else
            //        {


            //            if (list.Count == 0) //لايوجد اي زيادة من قبل 
            //            {

            //                if ((initialPrice + product.minRaise) > vauleBidding)
            //                {
            //                    ModelState.AddModelError(string.Empty, "The Offered Price Is Less Than The Min Price");
            //                }

            //                else
            //                { ///////// صار مزايدة
            //                    maxOffer = vauleBidding + initialPrice;
            //                    ViewBag.MaxPrice = "The Max Price :   " + maxOffer;
            //                }


            //                product.ProductsAuction = list;
            //            }


            //            else // list.cont !=0 تزايد عليه من قبل 
            //            {

            ////                if ((db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount) + product.minRaise) > riseAmount)
            ////                {
            //                    ModelState.AddModelError(string.Empty, "The Offered Price Is Less Than The Min Price");
            //                    if (list.Count == 0)
            //                    {
            //                        ViewBag.MaxPrice = "There is No Raises Yet";
            //                    }
            //                    else
            //                    {

            //            //            ViewBag.MaxPrice = "The Max Price :   " + db.ProductsAuctions.Where(t => t.ProductsId == productId).Max(t => t.riseAmount);
            //            //        }
            //        product.ProductsAuction = list;
            //    }
            //    else
            //    {
            //        maxOffer = riseAmount;

            //        ViewBag.MaxPrice = "The Max Price :   " + maxOffer;


            //        /////////////////// success start 
            //        ProductsAuction productsAuction = new ProductsAuction();
            //        productsAuction.ProductsId = productId;
            //        productsAuction.riseAmount = Convert.ToInt32(maxOffer);
            //        productsAuction.valueUP = Convert.ToInt32(vauleBidding);
            //        productsAuction.userId = name;

            //        //productsAuction.userId = userAuto;
            //        productsAuction.dateOfRise = DateTime.Now;
            //        db.ProductsAuctions.Add(productsAuction);
            //        productsAuction.limitPrice = limitPrice;
            //        db.SaveChanges();
            //        list.Add(productsAuction);

            //        //////////////////// success end 


            //    }
            //}



            //            //product.ProductsAuction = list;
            //        }


            //    }


            //}


            #endregion


        }
        /// <summary>
        /// اضافة قيمة مزاد جديد
        /// </summary>
        /// <param name="userId"> المستخدم الذي زايد </param>
        /// <param name="proudctId"> المنتج الذي زايد عليه  </param>
        /// <param name="raise">  قيمة المزايدة </param>
        /// <param name="remarks"> ملاحظات </param>
        /// <returns></returns>

        public async Task AddNewAuction(string userId, int proudctId, double raise, double? limitPrice = null, string remarks = "")
        {
            ProductsAuction productsAuction = new ProductsAuction();
            productsAuction.ProductsId = proudctId;
            productsAuction.limitPrice = limitPrice;
            productsAuction.riseAmount = Convert.ToInt32(raise);
            productsAuction.userId = userId;
            productsAuction.Remarks = remarks;
            productsAuction.dateOfRise = DateTime.Now;
            db.ProductsAuctions.Add(productsAuction);
            await db.SaveChangesAsync();
        }
        /// <summary>
        /// to get all users in productsAuction auto incrising amuont
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<ProductsAuction> getMaxinimAutoUser(int productId)
        {

            return await db.ProductsAuctions.Where(x => x.limitPrice != null && x.ProductsId == productId)
                .OrderByDescending(x => x.limitPrice).FirstOrDefaultAsync();

        }




        /// <summary>
        /// get the max rais amout for a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>

        public async Task<double> GetMaxRaisForProduct(int productId)
        {
            return await db.ProductsAuctions.Where(t => t.ProductsId == productId).MaxAsync(t => t.riseAmount);
        }













        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            var oldproducts = db.Products.Find(id);
            if (User.Identity.Name != oldproducts.userId)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {


                var allowedExtensions = new[] { ".Jpg", ".png", ".PNG", ".jpg", "jpeg" };

                var fileimage = file.ToString(); //getting complete url  

                var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
                var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
                if (allowedExtensions.Contains(ext)) //check what type of extension  
                {
                    string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                    string myfile = name + "_" + ext; //appending the name with id  
                                                      // store the file inside ~/project folder(Img)  
                    var path = Path.Combine(Server.MapPath("~/ProductImage"), myfile);

                    file.SaveAs(path);

                    products.Picture = myfile;
                }
                else
                {
                    ViewBag.message = "Please choose only Image file";
                }
                products.dateOfAdd = DateTime.Now;
                products.dateOfApprove = DateTime.Now;
                products.dateOfSell = DateTime.Now;
                products.userId = User.Identity.Name;
                products.dateOfSell = products.dateOfAdd;
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", products.ProductsTypeId);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {

            var oldproducts = db.Products.Find(id);
            if (User.Identity.Name != oldproducts.userId)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", products.ProductsTypeId);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products, HttpPostedFileBase file)
        {

            var oldproducts = db.Products.Find(products.Id);
            if (User.Identity.Name != oldproducts.userId)
            {
                return RedirectToAction("Index");
            }
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
                        var path = Path.Combine(Server.MapPath("~/ProductImage"), myfile);

                        file.SaveAs(path);

                        products.Picture = myfile;
                    }
                    else
                    {
                        ViewBag.message = "Please choose only Image file";
                    }


                }

                //////tootrow to replace the attributes of old product from new one :)





                oldproducts.productName = products.productName;
                oldproducts.ProductsTypeId = products.ProductsTypeId;
                oldproducts.Picture = products.Picture;
                oldproducts.Description = products.Description;
                oldproducts.Price = products.Price;
                oldproducts.minRaise = products.minRaise;
                oldproducts.dateOfAuctionEnd = products.dateOfAuctionEnd;
                oldproducts.dateOfAuctionEnd = products.dateOfAuctionEnd;

                db.Entry(oldproducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", products.ProductsTypeId);
            return View(products);
        }


        // GET: Products/Sell/5
        public ActionResult Sell(int? id)
        {
            var oldproducts = db.Products.Find(id);
            if (User.Identity.Name != oldproducts.userId)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", products.ProductsTypeId);
            return View(products);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Sell(Products product)
        {
            var oldproducts = db.Products.Find(product.Id);
            if (User.Identity.Name != oldproducts.userId)
            {
                return RedirectToAction("Index");
            }

            Products products = db.Products.Find(product.Id);
            if (products == null)
            {
                return HttpNotFound();
            }
            products.sold = true;
            products.dateOfSell = DateTime.Now;
            db.Entry(products).State = EntityState.Modified;
            db.SaveChanges();





            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", products.ProductsTypeId);
            return View(products);
        }



        // GET: Products/EditAprove/5
        public ActionResult EditAprove(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", products.ProductsTypeId);
            return View(products);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAprove(Products products, bool Aprove)
        {


            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            Products oldproducts = db.Products.Find(products.Id);
            if (ModelState.IsValid)
            {


                oldproducts.approved = Aprove;
                oldproducts.dateOfApprove = DateTime.Now;
                db.Entry(oldproducts).State = EntityState.Modified;
                db.SaveChanges();

                if (Aprove)
                {
                    return RedirectToAction("Aprove");
                }
                else
                {
                    return RedirectToAction("Reject");
                }
            }
            ViewBag.ProductsTypeId = new SelectList(db.ProductsTypes, "Id", "typeName", oldproducts.ProductsTypeId);

            return View(oldproducts);
        }




        // GET: Products/Aprove
        public ActionResult Aprove()
        {

            var products = db.Products.Where(a => a.approved.Equals(false)).Include(p => p.ProductsType);
            return View(products.ToList());


        }



        public ActionResult Reject()
        {

            var products = db.Products.Where(a => a.approved.Equals(true)).Include(p => p.ProductsType);
            return View(products.ToList());


        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            var oldproducts = db.Products.Find(id);
            if (User.Identity.Name != oldproducts.userId)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }




        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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

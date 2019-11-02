using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.Entities;
using MyEvernote.BusinessLayer;
using MyEverNote.WebApp.Models;
using MyEverNote.WebApp.filter;

namespace MyEverNote.WebApp.Controllers
{
    
    [Auth]
    [AuthAdmin]
    [Exc]
    public class CategoryController : Controller
    {
        //kategori ile alakalı işlemlerim için kullanıyorum.
        CategoryManager categoryManager = new CategoryManager();

        //DATAACCESSLAYERIN Repository'de kullandığı metodlar CORE'dan geliyor. 
        //CORE'daki IDAtaaccess interface'inden geliyor. Oradanda MAnagerBase'e implemente ettim. 
        //Bu da UI'da kullamamızı sağladı.
        // GET: Category
        public ActionResult Index()
        {
            return View(categoryManager.List());

            //return View(CacheHelper.GetCategoriesFromCache());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ///nullable int olduğu için id.value dedim.
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiededOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        //Edit edilecek olanın elde edilmesi
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiededOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                //Update olacak kategoriyi çekiyorum.
                Category cat = categoryManager.Find(x => x.Id == category.Id);
                //Modelden gelen verileri basıyorum
                cat.Title = category.Title;
                cat.Description = category.Description;
                
                
                categoryManager.Update(cat);

                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");


            }
            return View(category);
        }

        
        //Silinecek Kategoriyi çekiyorum.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.Find(x => x.Id == id.Value);
            
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.Find(x => x.Id == id);
            categoryManager.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();
     
            return RedirectToAction("Index");
        }

       
    }
}

using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEverNote.WebApp.Models
{
    public class CacheHelper
    {
        //Webhelperi nugetten yükledim.
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");

            if (result == null)
            {
                //cache boşsa catManagere git ve categorileri çek
                CategoryManager categoryManager = new CategoryManager();
                result = categoryManager.List();
                WebCache.Set("category-cache", result, 20,true);

            }
            return result;
          

        }

        public static void RemoveCategoriesFromCache()
        {
            Remove("category-cache");
        }

        //insertten sonra güncllemek için yaptım.
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}
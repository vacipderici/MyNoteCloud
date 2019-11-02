using MyEvernote.Common;
using MyEvernote.Core.DataAccess;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{   
    //Başka yerde de kullanmak için repositorybase(singleton kullanarak databasecontext oluşturduk),Irepository kullandık.
    public class Repository<T> :RepositoryBase,IDataAccess<T> where T : class //Generic bir class yaptım. Newlediğimiz zaman ne tip verirsem çalışacak. where ile kısıtladık newlenen bir tip olmalı. Örneğin int verilememeli.
    {
        /*    private DatabaseContext db = new DatabaseContext();*/ //Singletona uydurmak zorundayım hata aldım. Tüm repository newlenmesinde bu bir kere oluşsun.
        //private DatabaseContext db;
        private DbSet<T> _objectSet;

        public Repository()
        {
          
            _objectSet = context.Set<T>(); // Her defasında tipi bulmakla uğraşmamak için.  --Protected olduğu için db fieldi görmekteyiz.
        }

        public List<T> List()      //İlgili tablonun tüm kayıtları dönecek
        {
            //return db.Set<T>().ToList();
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()      //İlgili tablonun tüm kayıtları dönecek
        {
            //return db.Set<T>().ToList();
            return _objectSet.AsQueryable<T>();  //Bir IQueryable dönder. Orderladığımızda ne zaman tolist çağırırsak o zaman sqle gider.
        }


        public List<T> List(Expression<Func<T, bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }
        public int Insert(T obj)
        {
            _objectSet.Add(obj); //İlgili seti elde edip verilen tiple alakalı eklemeyi yapıp save metodunu çağır.
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUserName();//TODO : İŞlem yapan kullanıcı adı yazılmalı.

            }
            return Save();


        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

           
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUserName();//TODO : İŞlem yapan kullanıcı adı yazılmalı.

            }
            return Save();
        }
        public int Delete(T obj)
        {
            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase o = obj as MyEntityBase;
            //    DateTime now = DateTime.Now;

            //    o.CreatedOn = now;
            //    o.ModifiedOn = now;
            //    o.ModifiedUsername = "system";//TODO : İŞlem yapan kullanıcı adı yazılmalı.

            //}

            _objectSet.Remove(obj);

            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }


        //Tek bir tip döndüren metod
        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }
    }
}

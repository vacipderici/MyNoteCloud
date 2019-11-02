using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.DataAccess
{
    public interface IDataAccess<T>
    {
         List<T> List();     //İlgili tablonun tüm kayıtları dönecek


        List<T> List(Expression<Func<T, bool>> where);

        int Insert(T obj);


        int Update(T obj);

        int Delete(T obj);


        int Save();



        //Tek bir tip döndüren metod
        T Find(Expression<Func<T, bool>> where);

    }
}

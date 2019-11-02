using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        protected static DatabaseContext context; //Miras alınarak ulaşılabilir yaptım.
        private static object _lockSync = new object();

        protected RepositoryBase() //Miras alınabilir ancak newlenemez yaptım. Singleton için.
        {
           CreateContext();
        }

        //databasecontexti kontrol edip null ise bir kere newleyeceğiz.
        private static void CreateContext()
        {

            if (context == null)
            {
                lock (_lockSync)
                {
                    //Aynı anda sadece tek bir parcacik calistirmak icin
                    if (context == null) //Garanti olsun diye tekrardan kontrol ettim.
                    {
                        context = new DatabaseContext();
                    }

                }

            }
          
        }
    }
}

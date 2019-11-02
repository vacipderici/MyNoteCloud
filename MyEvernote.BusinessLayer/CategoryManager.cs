using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
   public class CategoryManager : ManagerBase<Category>
    {
        #region İlişkili  kayıtların silinmesi ile alakalı kod çözümü
        //-- Onemli not aşağıdaki satırları yoruma aldım çünkü fluent api ile çözdüm bir diğer seçenek ise DB de diagramda cascade aktif yapmaktı.

        //Delete metodunu burası iş katmanı olduğu için ezeceğim.Bir kategori silindiğinde ilgili yazılarla alakalı sıkıntılar için
        //  Her türlü platformda yararlanmak için mobil , web, windows v.b


        //Note: Virtual Metodları Override edebilirim. Çok Çok Biçimlilik (Polymorphism)


        //public override int Delete(Category category)
        //{
        //    //NoteManager noteManager = new NoteManager();
        //    //LikedManager likeManager = new LikedManager();
        //    //CommentManager commentManager = new CommentManager();

        //    ////Kategori ile ilişkili notların silinmesi lazım likeları ve commentleri dahil. Sildiğimiz için tolist kullandık.

        //    //foreach (Note note in category.Notes.ToList())
        //    //{
        //    //    //Note ile ilişkili ilk başta likeları silelim
        //    //    foreach (Liked like in note.Likes.ToList())
        //    //    {
        //    //        likeManager.Delete(like);
        //    //    }
        //    //    //Note ile ilişkili commentleri silme

        //    //    foreach (Comment comment in note.Comments.ToList())
        //    //    {
        //    //        commentManager.Delete(comment);
        //    //    }
        //    //    noteManager.Delete(note);
        //    //}

        //    //return base.Delete(category);
        //}
        #endregion 
    }
}

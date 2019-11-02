using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
  public  class NoteManager: ManagerBase<Note>
    {
        //private Repository<Note> repo_note = new Repository<Note>();

        //NOTE aşağıdaki metodları MAnagerBase sağladığı için sildim.

        //public List<Note> GetNotes()
        //{
        //    return List();
        //}
        //public IQueryable<Note> GetNoteQueryable() //Repositoryden sonra burayı yazdım.
        //{
        //    return ListQueryable();
        //}
    }
}

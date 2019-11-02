using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext :DbContext
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes  { get; set; }  //Artık Ef yapacağı tabloları biliyor.
        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //İlişkili kayıtları silmek için eziyorum bu metodu kendiliğinden gelen bir method.. DB ile diagramda cascade edebilirdik ancak bu yolu daha pratik buluyorum.

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Fluent Api Annotationsların bir alternatifi
            //Notelarda likeslar ve commentlerle alakalı cascade ilişikisi kuracağız.
            modelBuilder.Entity<Note>().
                HasMany(n => n.Comments).
                WithRequired(c => c.Note) //Comment Note'u olmadan olamaz
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Note>().
                HasMany(n => n.Likes).
                WithRequired(l => l.Note).
                WillCascadeOnDelete(true);

        }
    }
}

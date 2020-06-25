using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFS.db.Tables;
using WFS.db.WFScontext;

namespace WFS.business.Management
{
    public class NoteManagement : IDisposable
    {

        public class NoteFunctions : IDisposable
        {
            public List<Note> ListNotes()
            {
                using (cfgContext db = new cfgContext())
                {
                    return db.Note.ToList();
                }
            }

            public bool addNote(Note param, long id, string role)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {

                        if (role == "Personal")
                        { 
                            var personal = db.Personal.FirstOrDefault(r => r.PersonalId == id);

                            if (personal.Notes == null)
                            {
                                personal.Notes = new List<Note>();
                            }

                            personal.Notes.Add(param);

                            db.SaveChanges();
                            return true;
                        }

                        else
                        {
                            var manager = db.ClientManager.FirstOrDefault(r => r.ClientManagerId == id);

                            if (manager.Notes == null)
                            {
                                manager.Notes = new List<Note>();
                            }

                            manager.Notes.Add(param);

                            db.SaveChanges();
                            return true;
                        }
                    }
                }
                catch (Exception)
                { 
                    return false;
                }
            }

            public bool updateNote(Note param, long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var note = db.Note.Find(id);
                        note.Title = param.Title;
                        note.NoteTxt = param.NoteTxt; 
                        note.Update_Date = param.Update_Date;

                        db.SaveChanges();

                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool deleteNote(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var note = db.Note.Find(id);

                        if (note != null)
                        {
                            db.Note.Remove(note);
                            db.SaveChanges();
                        }
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public Note findNote(long id)
            {
                try
                {
                    using (cfgContext db = new cfgContext())
                    {
                        var note = db.Note.Find(id);
                        if (note != null)
                        {
                            return note;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }


            #region Disposee
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            protected bool Disposed { get; private set; }
            protected virtual void Dispose(bool disposing)
            {
                Disposed = true;
            }
            #endregion
        }
        #region Disposee
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected bool Disposed { get; private set; }
        protected virtual void Dispose(bool disposing)
        {
            Disposed = true;
        }
        #endregion
    }
}

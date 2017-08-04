using DatabaseLib.Models;
using System.Data.Entity;

namespace DatabaseLib.Dao
{
    public class XRefSpecDBContext : DbContext
    {
        public XRefSpecDBContext(string connString) : base(connString)
        {

        }

        public DbSet<XRefSpecObject> XRefSpecObjects { get; set; }
    }
}
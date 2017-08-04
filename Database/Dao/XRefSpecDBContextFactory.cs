using DatabaseLib.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLib.Dao
{
    public class XRefSpecDBContextFactory : IDbContextFactory<XRefSpecDBContext>
    {
        public XRefSpecDBContext Create()
        {
            return new XRefSpecDBContext("");
        }
    }
}

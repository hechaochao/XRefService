using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

using System.IO;
using DatabaseLib.Dao;
using Microsoft.AspNetCore.Mvc;
using DatabaseLib.Utility;
using System.Data.Entity;

namespace RestfulApiService.Controllers
{
    [Route("uids")]
    public class QueryController : Controller
    {
        private readonly XRefSpecDBContext _db;

        public QueryController(XRefSpecDBContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("{uid}")]
        public async Task<IActionResult> GetByUid(string uid)
        {
            string hashedUid = MD5Encryption.CalculateMD5Hash(uid);
            List<string> ut = null;
            try
            {
                ut = await _db.XRefSpecObjects.Where(b => b.HashedUid == hashedUid)
                               .Select(c => c.XRefSpecJson)
                               .ToListAsync();
            }catch(System.Exception e)
            {
                var t = e.Message;
            }

           

            //var response = new HttpResponseMessage();
            //var ms = new MemoryStream();
            //var sw = new StreamWriter(ms);
            //sw.Write("[");
            //sw.Write(string.Join(",", ut));
            //sw.Write("]");
            //sw.Flush();
            //ms.Position = 0;
            //response.Content = new StreamContent(ms);
            //response.StatusCode = HttpStatusCode.OK;
            //return new ObjectResult(response);
            return StatusCode(200, "[" + string.Join(",", ut) + "]");
        }

        [HttpGet]
        [Route("extension/{uid}")]
        public async Task<IActionResult> GetByUidForExtension(string uid)
        {
            string hashedUid = MD5Encryption.CalculateMD5Hash(uid);
            var ut = await _db.XRefSpecObjects.Where(b => b.HashedUid == hashedUid)
                                .Select(c => c.Uid)
                                .Take(20)
                                .ToListAsync();

            //var response = new HttpResponseMessage();
            //var ms = new MemoryStream();
            //var sw = new StreamWriter(ms);
            //sw.Write("[");
            //sw.Write(string.Join(",", ut));
            //sw.Write("]");
            //sw.Flush();
            //ms.Position = 0;
            //response.Content = new StreamContent(ms);
            //response.StatusCode = HttpStatusCode.OK;
            //return new ObjectResult(response);
            return StatusCode(200, "[" + string.Join(",", ut) + "]");
        }
        //[HttpPost]
        //[Route("")]
        //public async Task<IHttpActionResult> PostByUids([FromBody]string[] uids)
        //{ 
        //    List<Microsoft.DocAsCode.Plugins.XRefSpec> xfs = new List<Microsoft.DocAsCode.Plugins.XRefSpec>();
        //    var getUidTasks = new List<Task<XRefSpecObject>>();
        //    foreach (string uid in uids)
        //    {
        //        XrefSpecDBContext tempDB = new XrefSpecDBContext();
        //        string hashedUid = MD5Encryption.CalculateMD5Hash(uid);
        //        getUidTasks.Add(tempDB.XRefSpecObjects.Where(b => b.HashedUid == hashedUid)
        //                       .Select(c => c)
        //                       .FirstOrDefaultAsync());
        //    }

        //    var uidList = await Task.WhenAll(getUidTasks);
        //    foreach (var temp in uidList)
        //    {
        //        XRefSpec xf = null;
        //        if (temp != null)
        //        {
        //            xf = JsonConvert.DeserializeObject<XRefSpec>(temp.XRefSpecJson);
        //        }
        //        xfs.Add(xf);
        //    }
        //    return Ok(xfs);
        //}

    }
}


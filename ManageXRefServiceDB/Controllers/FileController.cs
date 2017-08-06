using DatabaseLib.Dao;
using DatabaseLib.Models;
using DatabaseLib.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DocAsCode.Build.Engine;
using Microsoft.DocAsCode.Common;
using Microsoft.DocAsCode.Plugins;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ManageXRefServiceDB.Controllers
{
    //[BasicAuthenticationAttribute("user", "user",
    //BasicRealm = "localhost")]
    [Route("uids")]
    public class FileController : Controller
    {
        private XRefSpecDBContext _db;
        private IHostingEnvironment _environment;

        public FileController(XRefSpecDBContext db, IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpPost("add")]
        public IActionResult Create([FromBody]XRefSpec spec)
        {
            if(spec == null)
            {
                return BadRequest();
            }

            _db.XRefSpecObjects.Add(new XRefSpecObject {HashedUid=MD5Encryption.CalculateMD5Hash(spec.Uid),
                Uid=spec.Uid, XRefSpecJson =JsonUtility.Serialize(spec) });
            _db.SaveChanges();
            return Ok();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(ICollection<IFormFile> files)
        {
            //var uploads = Path.Combine(_environment.ContentRootPath, "uploads");
            var tasks = new List<Task>();
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        tasks.Add(SaveDataAsync(reader));
                    }
                }
            }
            await Task.WhenAll(tasks);
            return StatusCode(200, "upload successfully");
        }

        private async Task SaveDataAsync(TextReader reader)
        {
            await Task.FromResult<int>(SaveData(reader));
        }

        private int SaveData(TextReader reader)
        {

            XRefMap xrefMap = YamlUtility.Deserialize<XRefMap>(reader);
            foreach (var spec in xrefMap.References)
            {
                XRefSpecObject xrefSpec = new XRefSpecObject();
                xrefSpec.Uid = spec["uid"];
                xrefSpec.HashedUid = MD5Encryption.CalculateMD5Hash(xrefSpec.Uid);
                xrefSpec.XRefSpecJson = JsonUtility.Serialize(spec);
                _db.XRefSpecObjects.Add(xrefSpec);
            }
            _db.SaveChanges();
            return 0;
        }

    }
}
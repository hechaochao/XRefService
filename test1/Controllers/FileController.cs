using DatabaseLib.Dao;
using DatabaseLib.Models;
using DatabaseLib.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DocAsCode.Build.Engine;
using Microsoft.DocAsCode.Common;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ManageXRefServiceDB.Controllers
{
    //[BasicAuthenticationAttribute("user", "user",
    //BasicRealm = "localhost")]
    public class FileController : Controller
    {
        private XRefSpecDBContext _db;
        private IHostingEnvironment _environment;

        public FileController(XRefSpecDBContext db, IHostingEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        // GET: File
        [HttpGet]
        public ActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
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
                        tasks.Add(SaveData(reader));
                    }
                }
            }
            await Task.WhenAll(tasks);
            ViewData["Message"] = "Upload successfully";
            return View();
        }

        private async Task SaveData(TextReader reader)
        {

            XRefMap xrefMap = YamlUtility.Deserialize<XRefMap>(reader);
            foreach (var spec in xrefMap.References)
            {
                XRefSpecObject xrefSpec = new XRefSpecObject();
                xrefSpec.Uid = spec["uid"];
                xrefSpec.HashedUid = MD5Encryption.CalculateMD5Hash(xrefSpec.Uid);
                xrefSpec.XRefSpecJson = Newtonsoft.Json.JsonConvert.SerializeObject(spec);
                _db.XRefSpecObjects.Add(xrefSpec);
            }
            _db.SaveChanges();
        }

    }
}
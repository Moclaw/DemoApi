using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _3PsProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3PsProj.Controllers
{
    [Route("api/reader/[action]")]
    public class FileReaderController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var stream = new FileStream("config.json", FileMode.OpenOrCreate);
            var reader = new StreamReader(stream);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetFileReader()
        {
            int id = 1;
            DirectoryInfo directory = new DirectoryInfo(@"ActionFolder");
            DirectoryInfo[] folders = directory.GetDirectories("*", SearchOption.AllDirectories);
            List<FileReader> fileReaders = new List<FileReader>();
            foreach (DirectoryInfo folder in folders)
            {
                id++;
                FileReader fileReader = new FileReader();
                fileReader.Id = id;
                fileReader.FileName = folder.Name;
                fileReader.Type = false;
                fileReader.Size = 0;
                fileReader.Childrens = new List<FileReader>();
                foreach (FileInfo file in folder.GetFiles("*.png", SearchOption.AllDirectories))
                {
                    FileReader fileReaderChild = new FileReader();
                    fileReaderChild.Id = id;
                    fileReaderChild.FileName = file.Name;
                    fileReaderChild.Type = true;
                    fileReaderChild.Size = (int)file.Length;
                    fileReader.Childrens.Add(fileReaderChild);
                }

                fileReaders.Add(fileReader);
            }
            return Ok(fileReaders);
        }
    }
}

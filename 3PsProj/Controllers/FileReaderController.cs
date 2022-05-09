using System.Collections.Generic;
using _3PsProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Drawing;

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
            DirectoryInfo directory = new DirectoryInfo(@"ActionFolder");
            var folders = directory.GetDirectories();
            var files = directory.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            var filesAndFolders = new List<FileReader>();
            foreach (var folder in folders)
            {
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = folder.Name,
                        Type = false,
                        Height = 0,
                        Width = 0,
                        Childrens = GetChildrens(folder)
                    }
                );
            }

            return Ok(filesAndFolders);
        }

        private List<FileReader> GetChildrens(DirectoryInfo directory)
        {
            var folders = directory.GetDirectories();
            var files = directory.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            var filesAndFolders = new List<FileReader>();
            foreach (var folder in folders)
            {
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = folder.Name,
                        Type = false,
                        Height = 0,
                        Width = 0,
                        Childrens = GetChildrens(folder)
                    }
                );
            }
            foreach (var file in files)
            {
                double height;
                double width;
                using (var img = Image.FromFile(file.Name))
                {
                     height = img.Height;
                     width = img.Width;
                }
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = height,
                        Width = width,
                        Childrens = null
                    }
                );
            }
            return filesAndFolders;
        }
    }
}

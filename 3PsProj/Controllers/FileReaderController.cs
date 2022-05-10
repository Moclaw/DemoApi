using System.Collections.Generic;
using _3PsProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3PsProj.Controllers
{
    [Route("api/reader/[action]")]
    public class FileReaderController : Controller
    {
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
            foreach (var file in files)
            {
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = 0,
                        Width = 0,
                        Childrens = null
                    }
                );
            }
            WriteFileToJson(filesAndFolders);
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
                // var img = Image.FromFile(file.FullName);
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = 0, //img.Height,
                        Width = 0, // img.Width,
                        Childrens = null
                    }
                );
            }
            return filesAndFolders;
        }

        private void WriteFileToJson(List<FileReader> filesAndFolders)
        {
            using (StreamWriter file = new StreamWriter(@"config.json"))
            {
                file.WriteLine(JsonConvert.SerializeObject(filesAndFolders));
            }
        }
    }
}

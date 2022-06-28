using System.Collections.Generic;
using _3PsProj.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using System;
using System.Xml;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3PsProj.Controllers
{
    [Route("api/reader/[action]")]
    public class FileReaderController : Controller
    {
        string[] filters = new string[]
        {
            "*.jpg",
            "*.jpeg",
            "*.jpe",
            "*.bmp",
            "*.png",
            "*.tiff",
            "*.gif",
        };

        [HttpGet]
        public IActionResult GetFileReader()
        {
            DirectoryInfo directory = new DirectoryInfo(@"ActionFolder");
            var folders = directory.GetDirectories();
            var fileSVG = directory.GetFiles("*.svg");
            var filesAndFolders = new List<FileReader>();
            List<FileInfo> files = new List<FileInfo>();
            foreach (var filter in filters)
            {
                files.AddRange(directory.GetFiles(filter, SearchOption.TopDirectoryOnly));
            }

            Console.WriteLine(directory.GetFiles("*.svg", SearchOption.TopDirectoryOnly));
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
                var img = Image.FromFile(file.FullName);
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = img.Height,
                        Width = img.Width,
                        Childrens = null
                    }
                );
            }
            foreach (var file in fileSVG)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(file.FullName);
                var root = xmlDoc.DocumentElement;
                var height = root.Attributes["height"].Value;
                var width = root.Attributes["width"].Value;
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = Convert.ToInt32(height),
                        Width = Convert.ToInt32(width),
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
            var filesAndFolders = new List<FileReader>();
            var fileSVG = directory.GetFiles("*.svg");
            List<FileInfo> files = new List<FileInfo>();
            foreach (var filter in filters)
            {
                files.AddRange(directory.GetFiles(filter, SearchOption.TopDirectoryOnly));
            }
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
                var img = Image.FromFile(file.FullName);
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = img.Height,
                        Width = img.Width,
                        Childrens = null
                    }
                );
            }
            foreach (var file in fileSVG)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(file.FullName);
                var root = xmlDoc.DocumentElement;
                var height = root.Attributes["height"].Value;
                var width = root.Attributes["width"].Value;
                filesAndFolders.Add(
                    new FileReader
                    {
                        FileName = file.Name,
                        Type = true,
                        Height = Convert.ToInt32(height),
                        Width = Convert.ToInt32(width),
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

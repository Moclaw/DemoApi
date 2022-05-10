using System;
using System.Collections.Generic;

namespace _3PsProj.Models
{
    public class FileReader
    {
        public string FileName { get; set; }
        public bool Type { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public List<FileReader> Childrens { get; set; }
    }
}

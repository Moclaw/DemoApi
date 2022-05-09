using System;
using System.Collections.Generic;

namespace _3PsProj.Models
{
    public class FileReader
    {
        public string FileName { get; set; }
        public bool Type { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public List<FileReader> Childrens { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace _3PsProj.Models
{
    public class FileReader
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public bool Type { get; set; }
        public int Size { get; set; }
        public List<FileReader> Childrens { get; set; }
    }
}

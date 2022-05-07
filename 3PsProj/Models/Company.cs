using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace _3PsProj.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Serial { get; set; }
        public bool Radio { get; set; }
        public string Note { get; set; }
        public string Decription { get; set; }
        public int? CompanyId { get; set; }
        public virtual List<Company> Childrens { get; set; }
    }
}

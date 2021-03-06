using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper.Models
{

    [Table(Name = "HERO")]
    [Index("uk_Name", "Name", true)]
    public class HerosModel
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string ImagePath { get; set; }
        public string Positions { get; set; }

    }
}

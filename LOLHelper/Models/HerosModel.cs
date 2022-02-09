﻿using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper.Models
{
    [Table(Name = "HERO")]
    [Index("uk_Name_Position", "Name,Position", true)]
    public class HerosModel
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        public int Order { get; set; }

        public virtual ICollection<SkillModel> Skills { get; set; }
        public bool Available { get; set; }
    }

    


}
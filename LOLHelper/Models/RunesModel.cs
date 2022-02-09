using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper.Models
{
    public enum RunesFactionEnum
    {
        精密,
        主宰,
        巫术,
        坚决,
        启迪
    }

    [Table(Name = "RUNES")]
    public class RunesModel
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public  int Hero_ID { get; set; }

        public RunesFactionEnum MasterRunes { get; set; }
        public int MasterOne { get; set; }
        public int MasterTow { get; set; }
        public int MasterThree { get; set; }
        public int MasterFour { get; set; }

        public RunesFactionEnum SlaveRunes { get; set; }
        public int SlaveOne { get; set; }
        public int SlaveTow { get; set; }
        public int SlaveThree { get; set; }

    }
}

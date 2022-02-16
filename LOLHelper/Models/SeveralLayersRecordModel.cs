using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper.Models
{
    [Table(Name = "SeveralLayersRecord")]
    public class SeveralLayersRecordModel
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public Position Position { get; set; }
        public int SeveralLayers { get; set; }
    }
}

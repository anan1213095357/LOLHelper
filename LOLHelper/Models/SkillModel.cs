using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper.Models
{
    [Table(Name = "SKILL")]
    public class SkillModel
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        public long Hero_ID { get; set; }
        public SkillEnum Skill_D { get; set; }
        public SkillEnum Skill_F { get; set; }


    }

    public enum SkillEnum
    {
        闪现,
        引燃,
        疾跑,
        传送,
        治疗,
        屏障,
        惩戒
    }
}

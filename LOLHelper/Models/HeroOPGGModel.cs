using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLHelper.Models
{

    public class HeroOPGGModel
    {
        public Datum[] data { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string image_url { get; set; }
        public string[] enemy_tips { get; set; }
        public string[] ally_tips { get; set; }
        public Skin[] skins { get; set; }
        public Passive passive { get; set; }
        public Spell[] spells { get; set; }
    }

    public class Passive
    {
        public string name { get; set; }
        public string description { get; set; }
        public string image_url { get; set; }
        public string video_url { get; set; }
    }

    public class Skin
    {
        public string name { get; set; }
        public bool has_chromas { get; set; }
        public string splash_image { get; set; }
        public string loading_image { get; set; }
        public string tiles_image { get; set; }
    }

    public class Spell
    {
        public string key { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int max_rank { get; set; }
        public int[] range_burn { get; set; }
        public int[] cooldown_burn { get; set; }
        public int[] cost_burn { get; set; }
        public string tooltip { get; set; }
        public string image_url { get; set; }
        public string video_url { get; set; }
    }

}

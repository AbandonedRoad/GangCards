using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Characters
{
    public class CharacterBase
    {
        public string Name { get; set;}
        public string ActiveStreeName { get; set; }
        public List<string> StreetName { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Level { get; set; }
    }
}

using Enum;
using System;
using System.Linq;

namespace CharacterBase
{
    public class SkillDicingOutput
    {
        public string[] Attributes { get; private set; }
        public string[] Needed { get; private set; }
        public string[] Diced { get; private set; }
        public bool Successful { get; private set; }

        private Skills _skillToCheck;
        private int _modifikator;

        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="skillToCheck"></param>
        /// <param name="modifikator"></param>
        /// <param name="attributes"></param>
        /// <param name="needed"></param>
        /// <param name="diced"></param>
        /// <param name="succesfull"></param>
        public SkillDicingOutput(Skills skillToCheck, int modifikator, string[] attributes, int[] needed, int[] diced, bool succesfull)
        {
            Attributes = attributes;

            Needed = new string[needed.Length];
            Diced = new string[diced.Length];
            for (int i = 0; i < needed.Length; i++)
            {
                Needed[i] = needed[i].ToString();
                Diced[i] = diced[i].ToString();
            }

            _modifikator = modifikator;
            _skillToCheck = skillToCheck;

            Successful = succesfull;
        }

        /// <summary>
        /// Gets the output
        /// </summary>
        /// <returns></returns>
        public string GetOutput()
        {
            string[] abbreviations = new string[Attributes.Length];
            for (int i = 0; i < Attributes.Length; i++)
            {
                abbreviations[i] = Attributes[i].Substring(0, 3);
            }

            // var result = String.Concat(String.Join("/", abbreviations), " Needed: ", String.Join("/", Needed), " Diced: ", String.Join("/", Diced));
            var result = String.Concat(_skillToCheck.ToString(), " + ", _modifikator.ToString(), " (", String.Join("/", abbreviations), "): ", String.Join("/", Diced));

            return result;
        }
    }
}
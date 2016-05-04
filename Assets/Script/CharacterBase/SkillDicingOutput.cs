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

        public SkillDicingOutput(string[] attributes, int[] needed, int[] diced, bool succesfull)
        {
            Attributes = attributes;

            Needed = new string[needed.Length];
            Diced = new string[diced.Length];
            for (int i = 0; i < needed.Length; i++)
            {
                Needed[i] = needed[i].ToString();
                Diced[i] = diced[i].ToString();
            }

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

            var result = String.Concat(String.Join("/", abbreviations), " Needed: ", String.Join("/", Needed), " Diced: ", String.Join("/", Diced));

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Characters
{
    public class NameCreator
    {
        private readonly List<string> _wVornamen = new List<string>();
        private readonly List<string> _mVornamen = new List<string>();
        private readonly List<string> _nachnamen = new List<string>();

        /// <summary>
        /// The Name creator.
        /// </summary>
        public NameCreator()
        {
            _wVornamen = SplitTextUp(ReadText("VNamenW"));
            _mVornamen = SplitTextUp(ReadText("VNamenM"));
            _nachnamen = SplitTextUp(ReadText("NNamen"));
        }

        /// <summary>
        /// Reads the text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        private string ReadText(string name)
        {
            var texts = Resources.Load(name) as TextAsset;
            return texts.text;
        }

        /// <summary>
        /// Splits the text up.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        private List<string> SplitTextUp(string text)
        {
            var splitUp = text.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < splitUp.Length; i++)
            {
                splitUp[i] = splitUp[i].Trim();
            }

            return splitUp.ToList();
        }

        /// <summary>
        /// Generates a name
        /// </summary>
        /// <param name="isMale"></param>
        /// <returns></returns>
        public string[] GenerateName(bool isMale)
        {
            var result = new string[2];

            result[0] = isMale ? _mVornamen.ElementAt(UnityEngine.Random.Range(0, 35)) : _wVornamen.ElementAt(UnityEngine.Random.Range(0, 35));
            result[1] = _nachnamen.ElementAt(UnityEngine.Random.Range(0, 35));

            return result;
        }
    }
}

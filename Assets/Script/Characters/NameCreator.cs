using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Creator
{
    public class NameCreator
    {
        private readonly List<string> _wVornamen = new List<string>();
        private readonly List<string> _mVornamen = new List<string>();
        private readonly List<string> _nachnamen = new List<string>();

        public NameCreator()
        {
            _wVornamen = SplitTextUp(ReadText("VNamenW.txt"));
            _mVornamen = SplitTextUp(ReadText("VNamenM.txt"));
            _nachnamen = SplitTextUp(ReadText("NNamen.txt"));
        }

        /// <summary>
        /// Reads the text.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>System.String.</returns>
        private string ReadText(string file)
        {
            try
            {
                var fullFile = Path.Combine(Directory.GetCurrentDirectory(), file);
                TextReader readFile = new StreamReader(fullFile);
                var content = readFile.ReadToEnd();
                readFile.Close();

                return content;
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return String.Empty;
        }

        /// <summary>
        /// Splits the text up.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>Dictionary&lt;System.String, System.String&gt;.</returns>
        private List<string> SplitTextUp(string text)
        {
            var splitUp = text.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);

            return splitUp.ToList();
        }

        public string[] GenerateName(bool isMale)
        {
            var result = new string[2];

            var rnd = new Random();

            result[0] = isMale ? _mVornamen.ElementAt(rnd.Next(35)) : _wVornamen.ElementAt(rnd.Next(35));
            result[1] = _nachnamen.ElementAt(rnd.Next(35));

            return result;
        }
    }
}

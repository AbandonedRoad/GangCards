using Enum;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Singleton
{
    public static class GUIHelper
    {
        /// <summary>
        /// Replaces all Texts on the text controls
        /// </summary>
        /// <param name="textToBeReplaced"></param>
        public static void ReplaceText(List<Text> textToBeReplaced)
        {
            var filtered = textToBeReplaced.Where(tx => tx.text.Trim().StartsWith("@") && tx.text.EndsWith("@"));

            foreach (var text in filtered)
            {
                var key = text.text.Substring(1, text.text.Length - 2);
                var replacement = string.Empty;
                var entryFound = ResourceSingleton.Instance.GetText(key, out replacement);
                if (!entryFound)
                {
                    // Not found - maybe a Special text?
                    SpecialText specialParsed = SpecialText.NotSet;
                    if (System.Enum.IsDefined(typeof(SpecialText), key))
                    {
                        specialParsed = (SpecialText)System.Enum.Parse(typeof(SpecialText), key, true);
                        replacement = ResourceSingleton.Instance.GetSpecialText(specialParsed);
                    }                    
                }

                if (entryFound)
                {
                    text.text = replacement;
                }
            }
        }
    }
}
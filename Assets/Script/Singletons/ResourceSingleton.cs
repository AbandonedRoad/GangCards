using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.Script;
using System.Globalization;
using Enum;

namespace Singleton
{
    public class ResourceSingleton
    {
        private static ResourceSingleton _instance;

        private Dictionary<string, string> _texts = new Dictionary<string, string>();

        /// <summary>
        /// Gets instance
        /// </summary>
        public static ResourceSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ResourceSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            var texts = Resources.Load("TextResources") as TextAsset;
            var splitUp = texts.text.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var splitted in splitUp)
            {
                var keyValuePair = splitted.Split('\t');
                if (keyValuePair.Count() != 2)
                {
                    continue;
                }

                var filteredKey = keyValuePair[0].Replace("\r\n", String.Empty).Replace(@"\", String.Empty).Replace("~", String.Empty).Replace("\"", String.Empty);
                var filteredValue = keyValuePair[1].Replace("\r\n", Environment.NewLine).Replace(@"\", String.Empty).Replace("~", String.Empty).Replace("\"", String.Empty);
                _texts.Add(filteredKey, filteredValue);
            }
        }

        /// <summary>
        /// Creates the action key, needed to map Text Ressources to action windo
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="actualAction"></param>
        /// <returns></returns>
        public string CreateActionText(string prefix, int actualAction)
        {
            var key = string.Concat(prefix, "Action", actualAction.ToString(), SettingsSingleton.Instance.Language == Language.English ? "_Eng" : "_Ger");

            return ResourceSingleton.Instance.GetText(key);
        }

        /// <summary>
        /// Creates the action key, needed to map Text Ressources to action windo
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="actualAction"></param>
        /// <returns></returns>
        public string CreateActionText(string prefix, string textType)
        {
            var key = string.Concat(prefix, textType, SettingsSingleton.Instance.Language == Language.English ? "_Eng" : "_Ger");

            return ResourceSingleton.Instance.GetText(key);
        }

        /// <summary>
        /// Gets a Text for a certain text key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(string key)
        {
            string result;
            if (!_texts.TryGetValue(key, out result))
            {
                result = String.Concat("?! Key: '", key, "' !?");
            }

            return result;
        }

        /// <summary>
        /// Gets a Special Text key via enum
        /// </summary>
        /// <param name="textType"></param>
        /// <returns></returns>
        public string GetSpecialText(SpecialText textType)
        {
            string key = String.Concat(textType.ToString(), SettingsSingleton.Instance.Language == Language.English ? "_Eng" : "_Ger");

            return GetText(key);
        }
    }
}
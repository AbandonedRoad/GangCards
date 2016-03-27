using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.Script;
using System.Globalization;
using Enum;
using Interfaces;
using Items;

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

                var filteredKey = keyValuePair[0].Trim();
                var filteredValue = keyValuePair[1].Trim();
                _texts.Add(filteredKey, filteredValue);
            }
        }

        /// <summary>
        /// Gets all Items fro the Text Ressource
        /// </summary>
        /// <returns></returns>
        internal List<IItem> GetItems()
        {
            List<IItem> result = new List<IItem>();

            var items = _texts.Where(pair => pair.Key.StartsWith("Item_"));
            var keys = items.Where(pair => pair.Key.EndsWith("Name_Ger"));

            foreach (var item in keys)
            {
                var key = item.Key.Substring(5, item.Key.Length - ("Item_Name_Ger").Length);
                string language = SettingsSingleton.Instance.Language == Language.English ? "Eng" : "Ger";

                var name = _texts[String.Concat("Item_", key, "Name_", language)];
                var itemType = _texts[String.Concat("Item_", key, "Type_", language)];
                var slot = _texts[String.Concat("Item_", key, "Slot_", language)];
                var p1Type = _texts[String.Concat("Item_", key, "Prop1Typ_", language)];
                var p1Val = _texts[String.Concat("Item_", key, "Prop1Val_", language)];
                var p2Type = _texts[String.Concat("Item_", key, "Prop2Typ_", language)];
                var p2Val = _texts[String.Concat("Item_", key, "Prop2Val_", language)];
                var p3Type = _texts[String.Concat("Item_", key, "Prop3Typ_", language)];
                var p3Val = _texts[String.Concat("Item_", key, "Prop3Val_", language)];                

                if (System.Enum.GetNames(typeof(WeaponType)).Contains(itemType))
                {
                    // Its a weapon
                    var splitP1 = p1Val.Split('-');
                    var splitP2 = p2Val.Split('-');
                    var splitP3 = p3Val.Split('-');

                    var wType = String.IsNullOrEmpty(itemType) ? null : System.Enum.Parse(typeof(WeaponType), itemType, true) as WeaponType?;
                    var d1Type = String.IsNullOrEmpty(p1Type) ? null : System.Enum.Parse(typeof(DamageType), p1Type, true) as DamageType?;
                    var d2Type = String.IsNullOrEmpty(p2Type) ? null : System.Enum.Parse(typeof(DamageType), p2Type, true) as DamageType?;
                    var d3Type = String.IsNullOrEmpty(p3Type) ? null : System.Enum.Parse(typeof(DamageType), p3Type, true) as DamageType?;
                    var iSlot = String.IsNullOrEmpty(slot) ? null : System.Enum.Parse(typeof(ItemSlot), slot, true) as ItemSlot?;

                    IItem weapon = null;
                    if (splitP2.Length == 1)
                    {
                        // Only one Property
                        weapon = new Weapon();
                    }
                    else if (splitP3.Length == 1)
                    {
                        // Two Properties
                        weapon = new Weapon(d2Type.Value, new int[] { int.Parse(splitP2[0]), int.Parse(splitP2[1]) });
                    }
                    else
                    {
                        weapon = new Weapon(d2Type.Value, new int[] { int.Parse(splitP2[0]), int.Parse(splitP2[1]) },
                            d3Type.Value, new int[] { int.Parse(splitP3[0]), int.Parse(splitP3[1]) });
                    }
                    ((Weapon)weapon).Init(name, wType.Value, iSlot.Value, d1Type.Value, new int[] { int.Parse(splitP1[0]), int.Parse(splitP1[1]) });

                    result.Add(weapon);
                }
                else if (System.Enum.GetNames(typeof(ArmorType)).Contains(itemType))
                {
                    // Its an aromr
                }
                else
                {
                    Debug.LogError("Item Type " + itemType + " unknown!");
                }
            }

            return result;
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
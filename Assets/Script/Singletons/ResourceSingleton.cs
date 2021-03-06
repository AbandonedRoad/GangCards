﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Enum;
using Interfaces;
using Items;

namespace Singleton
{
    public class ResourceSingleton
    {
        private static ResourceSingleton _instance;

        private Dictionary<string, string> _texts = new Dictionary<string, string>();
        private Dictionary<string, string> _items = new Dictionary<string, string>();

        public Dictionary<string, Sprite> ImageFaces { get; private set; }
        public Sprite BackgroundSelected { get; private set; }
        public Sprite BackgroundDeSelected { get; private set; }
        public Sprite FightingSkullSprite { get; private set; }
        public Sprite FightingRegularSprite { get; private set; }
        public Dictionary<Gangs, Sprite> Logos { get; private set; }
        public List<AudioClip> AllClips { get; private set; }

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
                    _instance.LoadImages();
                    _instance.LoadAudio();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            // Load TextRessource.
            ImageFaces = new Dictionary<string, Sprite>();
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

            // Load Item ressources.
            texts = Resources.Load("ItemResources") as TextAsset;
            splitUp = texts.text.Split(new string[] { "~" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var splitted in splitUp)
            {
                var keyValuePair = splitted.Split('\t');
                if (keyValuePair.Count() != 2)
                {
                    continue;
                }

                var filteredKey = keyValuePair[0].Trim();
                var filteredValue = keyValuePair[1].Trim();
                _items.Add(filteredKey, filteredValue);
            }    
        }

        /// <summary>
        /// Load all sounds
        /// </summary>
        private void LoadAudio()
        {
            AllClips = Resources.LoadAll<AudioClip>(String.Empty).ToList();
        }

        /// <summary>
        /// Loads relevant images
        /// </summary>
        private void LoadImages()
        {
            var sprites = Resources.LoadAll<Sprite>(String.Empty);

            // Load Image Ressources
            Sprite image = null;
            int groupCounter = 1;
            int faceCounter = 0;
            List<string> faceGroups = new List<string> { "Shaman", "Thug" };
            var faceToLoad = faceGroups.ElementAt(faceCounter);
            string nameToBeSearched;
            while (true)
            {
                nameToBeSearched = String.Concat(faceToLoad, groupCounter.ToString());
                image = sprites.FirstOrDefault(spr => spr.name == nameToBeSearched);
                if (image == null)
                {
                    faceCounter++;
                    faceToLoad = faceGroups.Count > faceCounter ? faceGroups.ElementAt(faceCounter) : String.Empty;
                    groupCounter = 1;
                }
                else
                {
                    ImageFaces.Add(faceToLoad + groupCounter.ToString(), image);
                    groupCounter++;
                }

                if (String.IsNullOrEmpty(faceToLoad))
                {
                    break;
                }
            }

            // Special Sprites
            FightingSkullSprite = sprites.First(spr => spr.name == "Skull");
            FightingRegularSprite = sprites.First(spr => spr.name == "EnemyBorder");

            Logos = new Dictionary<Gangs, Sprite>();
            Logos.Add(Gangs.WheelersOfDecay, sprites.First(spr => spr.name == "WheelersLogo"));
            Logos.Add(Gangs.CGN9, sprites.First(spr => spr.name == "CNG9Logo"));
            Logos.Add(Gangs.Shamans, sprites.First(spr => spr.name == "ShamansLogo"));
            Logos.Add(Gangs.Tragosa, sprites.First(spr => spr.name == "TragosaLogo"));

            BackgroundDeSelected = sprites.First(spr => spr.name == "BackgroundGray");
            BackgroundSelected = sprites.First(spr => spr.name == "BackgroundSelected");
        }

        /// <summary>
        /// Gets all Items fro the Text Ressource
        /// </summary>
        /// <returns></returns>
        internal List<IItem> GetUniqueItems()
        {
            List<IItem> result = new List<IItem>();

            var items = _items.Where(pair => pair.Key.StartsWith("Item_"));
            var keys = items.Where(pair => pair.Key.EndsWith("Name_Ger"));

            foreach (var item in keys)
            {
                // Add all unique items.

                var key = item.Key.Substring(5, item.Key.Length - ("Item_Name_Ger").Length);
                string language = SettingsSingleton.Instance.Language == Language.English ? "Eng" : "Ger";

                var name = _items[String.Concat("Item_", key, "Name_", language)];
                var itemKey = int.Parse(_items[String.Concat("Item_", key, "Key")]);
                var skill = _items[String.Concat("Item_", key, "Skill")];
                var rarity = _items[String.Concat("Item_", key, "Rarity")];
                var level = _items[String.Concat("Item_", key, "Level")];
                var itemType = _items[String.Concat("Item_", key, "Type_", language)];
                var slot = _items[String.Concat("Item_", key, "Slot_", language)];
                var p1Type = _items[String.Concat("Item_", key, "Prop1Typ_", language)];
                var p1Val = _items[String.Concat("Item_", key, "Prop1Val_", language)];
                var p2Type = _items[String.Concat("Item_", key, "Prop2Typ_", language)];
                var p2Val = _items[String.Concat("Item_", key, "Prop2Val_", language)];
                var p3Type = _items[String.Concat("Item_", key, "Prop3Typ_", language)];
                var p3Val = _items[String.Concat("Item_", key, "Prop3Val_", language)];                

                if (System.Enum.GetNames(typeof(WeaponType)).Contains(itemType))
                {
                    // Its a weapon
                    var splitP1 = p1Val.Split('-');
                    var splitP2 = p2Val.Split('-');
                    var actionPointCosts = int.Parse(p3Val);

                    var wType = String.IsNullOrEmpty(itemType) ? null : System.Enum.Parse(typeof(WeaponType), itemType, true) as WeaponType?;
                    var pt1Type = String.IsNullOrEmpty(p1Type) ? null : System.Enum.Parse(typeof(DamageType), p1Type, true) as DamageType?;
                    var pt2Type = String.IsNullOrEmpty(p2Type) ? null : System.Enum.Parse(typeof(DamageType), p2Type, true) as DamageType?;
                    var iSlot = String.IsNullOrEmpty(slot) ? null : System.Enum.Parse(typeof(ItemSlot), slot, true) as ItemSlot?;
                    var neededSkill = String.IsNullOrEmpty(skill) ? Skills.None : System.Enum.Parse(typeof(Skills), skill, true) as Skills?;
                    var rarityEnum = System.Enum.Parse(typeof(Rarity), rarity, true) as Rarity?;

                    IItem weapon = null;
                    if (splitP2.Length == 1)
                    {
                        // Only one Property
                        weapon = new Weapon(pt1Type.Value, new int[] { int.Parse(splitP1[0]), int.Parse(splitP1[1]) });
                    }
                    else
                    {
                        // Two Properties
                        weapon = new Weapon(pt1Type.Value, new int[] { int.Parse(splitP1[0]), int.Parse(splitP1[1]) },
                            pt2Type.Value, new int[] { int.Parse(splitP2[0]), int.Parse(splitP2[1]) });
                    }
                    
                    ((Weapon)weapon).Init(itemKey, name, int.Parse(level), neededSkill.Value, wType.Value, iSlot.Value, actionPointCosts, rarityEnum.Value);
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
        /// Generate items.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IItem> GenerateItems()
        {
            List<IItem> result = new List<IItem>();
            for (int level = 1; level < 15; level++)
            {
                // Generate Weapons
                foreach (WeaponType weaponType in System.Enum.GetValues(typeof(WeaponType)).Cast<WeaponType>().Where(wt => wt != WeaponType.NotSet))
                {
                    var rarity = Rarity.Normal;
                    rarity = rarity.GetRandomRarity();

                    Weapon weapon;
                    DamageType dmgType = DamageType.NotSet;
                    dmgType = dmgType.GetDamageTypeForWeaponType(weaponType);
                    if (rarity != Rarity.VeryRare)
                    {
                        weapon = new Weapon(new DamageRange(dmgType, weaponType, level));
                    }
                    else
                    {
                        weapon = new Weapon(new DamageRange(dmgType, weaponType, level),
                                            new DamageRange(dmgType, weaponType, level)); // TODO: Macht eigentlich keinen sinn! 2x den selben type ist käse!
                    }

                    weapon.Init(0, weaponType.ToString(), level, weaponType.GetNeededSkillForWeaponType(), 
                        weaponType, weaponType.GetMatchingItemSlot(), weaponType.GetActionCost(), rarity);

                    result.Add(weapon);
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
            var key = string.Concat(prefix, "Action", actualAction.ToString());

            var text = String.Empty;
            ResourceSingleton.Instance.GetText(key, out text);
            return text;
        }

        /// <summary>
        /// Creates the action key, needed to map Text Ressources to action windo
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="actualAction"></param>
        /// <returns></returns>
        public string CreateActionText(string prefix, string textType)
        {
            var key = string.Concat(prefix, textType);

            var text = String.Empty;
            ResourceSingleton.Instance.GetText(key, out text);
            return text;
        }

        /// <summary>
        /// Gets a Text for a certain text key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool GetText(string key, out string value)
        {
            key += (SettingsSingleton.Instance.Language == Language.English ? "_Eng" : "_Ger");

            if (!_texts.TryGetValue(key, out value))
            {
                value = String.Concat("?! ", key, " !?");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets a Special Text key via enum
        /// </summary>
        /// <param name="textType"></param>
        /// <returns></returns>
        public string GetSpecialText(SpecialText textType)
        {
            var text = String.Empty;
            ResourceSingleton.Instance.GetText(textType.ToString(), out text);

            return text;
        }
    }
}
﻿using Enum;
using Interfaces;
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
        public string ImageName { get; set; }

        public Dictionary<ItemSlot, IItem> UsedItems { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public CharacterBase()
        {
            UsedItems = new Dictionary<ItemSlot, IItem>();
            ImageName = "Thug1";

            foreach (ItemSlot item in System.Enum.GetValues(typeof(ItemSlot)))
            {
                if (item == ItemSlot.NotSet)
                {
                    continue;
                }

                UsedItems.Add(item, null);
            }
        }
    }
}
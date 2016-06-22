using System;
using Enum;
using Interfaces;
using UnityEngine;

namespace Items
{
    [Serializable]
    public class Armor : IItem
    {
        public int Key { get; private set; }
        public string Name { get; set; }
        public int Level { get; private set; }
        public ArmorType ArmorType { get; private set; }
        public int DamageAbsorb { get; private set; }
        public ItemSlot UsedInSlot { get; private set; }
        public IItemStrategy ItemStragegy { get; private set; }
        public Skills NeededSkill { get; private set; }
        public AudioClip AudioClip { get; private set; }
        public ItemType ItemType { get { return ItemType.Armor; } }
        public int Price { get; private set; }
        public Rarity Rarity { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="damageAbsorb"></param>
        public Armor(string name, ArmorType type, int damageAbsorb)
        {
            Name = name;
            ArmorType = type;
            DamageAbsorb = damageAbsorb;
            NeededSkill = Skills.None;
        }

        public string GetValue(ItemIdentifiers identifier)
        {
            throw new NotImplementedException();
        }

        public void ApplyParamters(int level, IGangMember assignTo)
        {
            throw new NotImplementedException();
        }
    }
}

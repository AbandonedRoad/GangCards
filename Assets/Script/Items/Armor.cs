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
        public IGangMember AssignedTo { get; set; }
        public IItemStrategy ItemStragegy { get; private set; }
        public Skills NeededSkill { get; private set; }
        public AudioClip AudioClip { get; private set; }
        public ItemType ItemType { get { return ItemType.Armor; } }
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

        /// <summary>
        /// Prepare Armor
        /// </summary>
        /// <param name="rarity"></param>
        public void Init(Rarity rarity)
        {
            Rarity = rarity;
        }

        public string GetValue(ItemIdentifiers identifier)
        {
            throw new NotImplementedException();
        }

        public IItem Clone()
        {
            throw new NotImplementedException();
        }
    }
}

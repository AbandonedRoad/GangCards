using Enum;
using Interfaces;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using Singleton;

namespace Items
{
    [Serializable]
    public class Weapon : IItem
    {
        public int Key { get; private set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public WeaponType WeaponType { get; private set; }
        public ItemSlot UsedInSlot { get; private set; }
        public Dictionary<ItemIdentifiers, DamageRange> DamageRanges { get; private set; }
        public IItemStrategy ItemStragegy { get; private set; }
        public Skills NeededSkill { get; private set; }
        public IGangMember AssignedTo { get; set; }
        public int ActionCosts { get; private set; }
        public AudioClip AudioClip { get; private set; }
        public ItemType ItemType { get { return ItemType.Weapon; } }
        public Rarity Rarity { get; private set; }
        public int Price { get; set; }

        /// <summary>
        /// Creates a weapon with one damage range
        /// </summary>
        /// <param name="type"></param>
        /// <param name="damageTypeP1"></param>
        /// <param name="dmgRangeP1"></param>
        public Weapon(DamageType damageTypeP1, int[] dmgRangeP1)
            : this(new DamageRange(damageTypeP1, dmgRangeP1[0], dmgRangeP1[1]))
        { }

        /// <summary>
        /// Creates a new Weapon with two damage ranges.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="damageTypeP1"></param>
        /// <param name="dmgRangeP1"></param>
        public Weapon(DamageType damageTypeP1, int[] dmgRangeP1, DamageType damageTypeP2, int[] dmgRangeP2)
            : this (new DamageRange(damageTypeP1, dmgRangeP1[0], dmgRangeP1[1]), 
                    new DamageRange(damageTypeP2, dmgRangeP2[0], dmgRangeP2[1]))
        { }

        /// <summary>
        /// Creates a weapon with one damage range
        /// </summary>
        /// <param name="range1"></param>
        public Weapon(DamageRange range1)
        {
            DamageRanges = new Dictionary<ItemIdentifiers, DamageRange>();
            DamageRanges.Add(ItemIdentifiers.Property1Val, range1);
        }

        /// <summary>
        /// Creates a new Weapon with two damage ranges.
        /// </summary>
        /// <param name="range1"></param>
        /// <param name="range2"></param>
        public Weapon(DamageRange range1, DamageRange range2)
        {
            DamageRanges = new Dictionary<ItemIdentifiers, DamageRange>();
            DamageRanges.Add(ItemIdentifiers.Property1Val, range1);
            DamageRanges.Add(ItemIdentifiers.Property2Val, range2);
        }

        /// <summary>
        /// Creates a weapon, whereby the given weapon is the base for it.
        /// </summary>
        /// <param name="weapon"></param>
        public Weapon(Weapon weapon)
        {
            Key = weapon.Key;
            Name = weapon.Name;
            Level = weapon.Level;
            WeaponType = weapon.WeaponType;
            UsedInSlot = weapon.UsedInSlot;
            DamageRanges = new Dictionary<ItemIdentifiers, DamageRange>(weapon.DamageRanges);
            ItemStragegy = weapon.ItemStragegy;
            NeededSkill = weapon.NeededSkill;
            AssignedTo = null;
            ActionCosts = weapon.ActionCosts;
            AudioClip = weapon.AudioClip;
            Rarity = weapon.Rarity;
    }

    /// <summary>
    /// Initializes base set
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="damageTypeP1"></param>
    /// <param name="dmgRangeP1"></param>
    public void Init(int key, string name, int level, Skills neededSkill, WeaponType type, ItemSlot slot, int actionPointsCost, Rarity rarity)
        {
            Key = key != 0 ? key : Math.Abs((name + level.ToString()).GetHashCode());
            Name = name;
            Rarity = rarity;
            Level = level;
            NeededSkill = neededSkill;
            WeaponType = type;
            ActionCosts = actionPointsCost;
            UsedInSlot = slot;
            ItemStragegy = new WeaponStrategy(this);
            AudioClip = ResourceSingleton.Instance.AllClips.FirstOrDefault(clp => clp.name == String.Concat(name, "Sound"));
        }

        /// <summary>
        /// Gets a value
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public string GetValue(ItemIdentifiers identifier)
        {
            switch (identifier)
            {
                case ItemIdentifiers.Name:
                    return Name;
                case ItemIdentifiers.Type:
                    return WeaponType.ToString();
                case ItemIdentifiers.Property1Type:
                    return GetDisplayType(ItemIdentifiers.Property1Val);
                case ItemIdentifiers.Property2Type:
                    return GetDisplayType(ItemIdentifiers.Property2Val);
                case ItemIdentifiers.Property1Val:
                case ItemIdentifiers.Property2Val:
                    if (!DamageRanges.ContainsKey(identifier))
                    {
                        return string.Empty;
                    }
                    return DamageRanges[identifier].CreateDisplayValue();
                case ItemIdentifiers.Property3Val:
                    return ActionCosts.ToString();
            }

            Debug.LogError("ItemIdentifier is unknown!");

            return String.Empty;
        }

        /// <summary>
        /// Clones this item
        /// </summary>
        /// <returns></returns>
        public IItem Clone()
        {
            Weapon clone = new Weapon(this);

            return clone;
        }

        /// <summary>
        /// Gets a Display Type.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private string GetDisplayType(ItemIdentifiers identifier)
        {
            if (!DamageRanges.ContainsKey(identifier))
            {
                return string.Empty;
            }
            return DamageRanges[identifier].CreateDisplayType();
        }
    }
}

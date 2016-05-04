using Enum;
using Interfaces;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

namespace Items
{
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
        public IGangMember AssignedTo { get; private set; }

        /// <summary>
        /// Creates new instance
        /// </summary>
        public Weapon()
        { }

        /// <summary>
        /// Creates a new Weapon
        /// </summary>
        /// <param name="type"></param>
        /// <param name="damageTypeP1"></param>
        /// <param name="dmgRangeP1"></param>
        public Weapon(DamageType damageTypeP2, int[] dmgRangeP2)
        {
            DamageRanges.Add(ItemIdentifiers.Property2Val, new DamageRange(damageTypeP2, dmgRangeP2[0], dmgRangeP2[1]));
        }

        /// <summary>
        /// Creates a new Weapon
        /// </summary>
        /// <param name="type"></param>
        /// <param name="damageTypeP1"></param>
        /// <param name="dmgRangeP1"></param>
        public Weapon(DamageType damageTypeP2, int[] dmgRangeP2, DamageType damageTypeP3, int[] dmgRangeP3)
        {
            DamageRanges.Add(ItemIdentifiers.Property2Val, new DamageRange(damageTypeP2, dmgRangeP2[0], dmgRangeP2[1]));
            DamageRanges.Add(ItemIdentifiers.Property3Val, new DamageRange(damageTypeP3, dmgRangeP3[0], dmgRangeP3[1]));
        }

        /// <summary>
        /// Initializes base set
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="damageTypeP1"></param>
        /// <param name="dmgRangeP1"></param>
        public void Init(int key, string name, Skills neededSkill, WeaponType type, ItemSlot slot, DamageType damageTypeP1, int[] dmgRangeP1)
        {
            Key = key;
            Name = name;
            NeededSkill = neededSkill;
            WeaponType = type;
            DamageRanges = new Dictionary<ItemIdentifiers, DamageRange>();
            UsedInSlot = slot;
            ItemStragegy = new WeaponStrategy(this);

            DamageRanges.Add(ItemIdentifiers.Property1Val, new DamageRange(damageTypeP1, dmgRangeP1[0], dmgRangeP1[1]));
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
                case ItemIdentifiers.Property3Type:
                    return GetDisplayType(ItemIdentifiers.Property3Val);
                case ItemIdentifiers.Property1Val:
                case ItemIdentifiers.Property2Val:
                case ItemIdentifiers.Property3Val:
                    if (!DamageRanges.ContainsKey(identifier))
                    {
                        return string.Empty;
                    }
                    return DamageRanges[identifier].CreateDisplayValue();
            }

            Debug.LogError("ItemIdentifier is unknown!");

            return String.Empty;
        }

        /// <summary>
        /// Applies a new level.
        /// </summary>
        /// <param name="level">Level to be used.</param>
        public void ApplyParamters(int level, IGangMember assignTo)
        {
            AssignedTo = assignTo;
            Level = level;
            if (Level > 1)
            {
                // Apply level to damage
                float factor = Level * 0.75f;
                var keys = DamageRanges.Keys.ToList();
                foreach (var identifierKey in keys)
                {
                    var actual = DamageRanges[identifierKey];
                    actual = new DamageRange(actual.Type, actual.MinDamage * factor, actual.MaxDamage * factor);
                }
            }
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

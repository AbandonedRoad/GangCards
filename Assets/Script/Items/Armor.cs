using System;
using Enum;
using Interfaces;

namespace Items
{
    public class Armor : IItem
    {
        public int Key { get; private set; }
        public string Name { get; set; }
        public int Level { get; private set; }
        public ArmorType ArmorType { get; private set; }
        public int DamageAbsorb { get; private set; }

        public ItemSlot UsedInSlot { get; private set; }

        public IItemStrategy ItemStragegy { get; private set; }

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
        }

        public string GetValue(ItemIdentifiers identifier)
        {
            throw new NotImplementedException();
        }

        public void ApplyLevel(int level)
        {
            throw new NotImplementedException();
        }
    }
}

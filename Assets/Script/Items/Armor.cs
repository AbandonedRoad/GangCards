using System;
using Enum;
using Interfaces;

namespace Items
{
    public class Armor : IItem
    {
        public string Name { get; set; }
        public ArmorType ArmorType { get; private set; }
        public int DamageAbsorb { get; private set; }

        public ItemSlot UsedInSlot { get; private set; }

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
    }
}

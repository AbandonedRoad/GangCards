using Enum;
using UnityEngine;

namespace Interfaces
{
    public interface IItem
    {
        int Key { get; }

        string Name { get; set; }

        int Level { get; }

        ItemSlot UsedInSlot { get; }

        ItemType ItemType { get; }

        Rarity Rarity { get; }

        int Price { get; }

        IItemStrategy ItemStragegy { get; }

        Skills NeededSkill { get; }

        AudioClip AudioClip { get; }

        string GetValue(ItemIdentifiers identifier);

        void ApplyParamters(int level, IGangMember assignedTo);
    }
}

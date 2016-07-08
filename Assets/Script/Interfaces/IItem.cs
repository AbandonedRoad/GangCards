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

        IItemStrategy ItemStragegy { get; }

        Skills NeededSkill { get; }

        IGangMember AssignedTo { get; set; }

        AudioClip AudioClip { get; }

        string GetValue(ItemIdentifiers identifier);

        IItem Clone();
    }
}

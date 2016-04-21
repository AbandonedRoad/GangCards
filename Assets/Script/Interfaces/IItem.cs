using Enum;

namespace Interfaces
{
    public interface IItem
    {
        int Key { get; }

        string Name { get; set; }

        int Level { get; }

        ItemSlot UsedInSlot { get; }

        IItemStrategy ItemStragegy { get; }

        string GetValue(ItemIdentifiers identifier);

        void ApplyLevel(int level);
    }
}

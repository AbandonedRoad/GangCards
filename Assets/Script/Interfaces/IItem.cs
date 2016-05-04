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

        Skills NeededSkill { get; }

        string GetValue(ItemIdentifiers identifier);

        void ApplyParamters(int level, IGangMember assignedTo);
    }
}

using Enum;

namespace Interfaces
{
    public interface IItem
    {
        string Name { get; set; }

        ItemSlot UsedInSlot { get; }

        string GetValue(ItemIdentifiers identifier);
    }
}

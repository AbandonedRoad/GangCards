using Enum;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IGangMember
    {
        Dictionary<ItemSlot, IItem> UsedItems { get; set; }
        string Name { get; set; }
        string ActiveStreeName { get; set; }
        List<string> StreetName { get; set; }
        int Intelligence { get; set; }
        int Strength { get; set; }
        int Level { get; set; }
        int Intiative { get; set; }
        string ImageName { get; set; }
    }
}

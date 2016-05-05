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
        int Initiative { get; set; }
        int Accuracy { get; set; }
        int Courage { get; set; }
        int Level { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
        Gangs GangAssignment { get; set; }
        HealthStatus HealthStatus { get; }

        void PostProcessInit(Gangs desiredGang);
    }
}

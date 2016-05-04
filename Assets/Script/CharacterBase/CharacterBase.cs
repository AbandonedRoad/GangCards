using Enum;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Characters
{
    public class CharacterBase
    {
        public string Name { get; set;}
        public string ActiveStreeName { get; set; }
        public List<string> StreetName { get; set; }

        public int Level { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Initiative { get; set; }
        public int Accuracy { get; set; }
        public int Courage { get; set; }

        public Gangs GangAssignment { get; set; }

        public Dictionary<ItemSlot, IItem> UsedItems { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public CharacterBase()
        {
            UsedItems = new Dictionary<ItemSlot, IItem>();

            foreach (ItemSlot item in System.Enum.GetValues(typeof(ItemSlot)))
            {
                if (item == ItemSlot.NotSet)
                {
                    continue;
                }

                UsedItems.Add(item, null);
            }
        }

        /// <summary>
        /// Post process initialisation
        /// </summary>
        /// <param name="desiredGang"></param>
        public virtual void PostProcessInit(Gangs desiredGang)
        {
            GangAssignment = desiredGang;
        }
    }
}

using Enum;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Singleton
{
    public class ItemSingleton
    {
        private static ItemSingleton _instance;

        public List<IItem> OwnedItems { get; private set; }
        public List<IItem> Items { get; private set; }

        /// <summary>
        /// Gets instance
        /// </summary>
        public static ItemSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ItemSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Gets an item
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public IItem GetItem(int key, int level, IGangMember assignTo)
        {
            var result = Items.FirstOrDefault(typ => typ.Key == key);

            if (result == null)
            {
                Debug.LogError("Following item is not found!  Key: " + key);
            }

            result.ApplyParamters(level, assignTo);

            return result;
        }

        /// <summary>
        /// Gets an item for the desired slot.
        /// </summary>
        /// <returns></returns>
        public IItem GetItem(ItemSlot type, int level, IGangMember assignTo)
        {
            var result = Items.FirstOrDefault(typ => typ.UsedInSlot == type);

            if (result == null)
            {
                Debug.LogError("An item of type " + type + " is not found!");
            }

            result.ApplyParamters(level, assignTo);

            return result;
        }

        /// <summary>
        /// Returns an appropriate Weapon for a level
        /// </summary>
        /// <returns></returns>
        public void ReturnAppropiateWeapon(int level, IGangMember assignToMember)
        {
            Instance.GetItem(ItemSlot.MainWeapon, level, assignToMember);
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            Items = ResourceSingleton.Instance.GetItems();
            OwnedItems = ResourceSingleton.Instance.GetItems();
        }
    }
}

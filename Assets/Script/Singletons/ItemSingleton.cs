using Enum;
using Interfaces;
using Items;
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
        private List<IItem> _itemsInHQ;
        private List<IItem> _itemsInCar;

        public Dictionary<ItemLocation, Func<List<IItem>>> ItemCollections { get; private set; }
        public List<IItem> AvailableItems { get; private set; }

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
        public IItem GetItem(WeaponType weaponType, int level, IGangMember assignTo)
        {
            var item = AvailableItems.Where(it => it is Weapon).Cast<Weapon>().FirstOrDefault(typ => typ.WeaponType == weaponType);

            if (item == null)
            {
                Debug.LogError("No item was found for type!  Weapon Type: " + weaponType);
            }

            item.AssignedTo = assignTo;

            return item;
        }

        /// <summary>
        /// Gets an item for the desired slot.
        /// </summary>
        /// <returns></returns>
        public IItem GetItem(ItemSlot type, int level, IGangMember assignTo)
        {
            var item = AvailableItems.FirstOrDefault(typ => typ.UsedInSlot == type);

            if (item == null)
            {
                Debug.LogError("An item of type " + type + " is not found!");
            }

            item.AssignedTo = assignTo;

            return item;
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
        /// Returns the price for an item
        /// </summary>
        /// <param name="item"></param>
        public int ReturnPriceForItem(IItem item)
        {
            Weapon weapon = item as Weapon;
            Armor armor = item as Armor;

            int price = 0;

            if (weapon != null)
            {
                if (weapon.Price > 0)
                {
                    return weapon.Price;
                }

                float rarityFactor = 1;
                if (item.Rarity == Rarity.Rare)
                {
                    rarityFactor = 1.2f;
                }
                else if (item.Rarity == Rarity.VeryRare)
                {
                    rarityFactor = 1.4f;
                }
                else if (item.Rarity == Rarity.Unique)
                {
                    rarityFactor = 1.8f;
                }

                switch (weapon.WeaponType)
                {
                    case WeaponType.Rifle:
                        price = UnityEngine.Random.Range(200, 400);
                        price = price * item.Level;
                        break;
                    case WeaponType.Pistol:
                        price = UnityEngine.Random.Range(100, 200);
                        price = price * item.Level;
                        break;
                    case WeaponType.Blade:
                        price = UnityEngine.Random.Range(75, 150);
                        price = price * item.Level;
                        break;
                    default:
                        break;
                }

                item.Price = (int)Math.Round((price * rarityFactor), 0);
            }
            else if (armor != null)
            {
                Debug.LogError("Armor is not implemeted!");
            }
            else
            {
                Debug.LogError("IItem type unknown");
            }

            return price;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            UnityEngine.Random.seed = DateTime.Now.Millisecond * DateTime.Now.Second;

            AvailableItems = ResourceSingleton.Instance.GetUniqueItems();
            AvailableItems.AddRange(ResourceSingleton.Instance.GenerateItems());

            // Prepare Dictionaries.
            _itemsInHQ = new List<IItem>();
            _itemsInCar = new List<IItem>();

            ItemCollections = new Dictionary<ItemLocation, Func<List<IItem>>>();
            ItemCollections.Add(ItemLocation.ItemsInHeadQuarter, () => { return _itemsInHQ; });
            ItemCollections.Add(ItemLocation.ItemsInTheCar, () => { return _itemsInCar; });
            ItemCollections.Add(ItemLocation.ItemsOfTheMembers, () => GetGangMembersItems());
        }

        /// <summary>
        /// Generate List of Items which the gang owns
        /// </summary>
        /// <returns></returns>
        private List<IItem> GetGangMembersItems()
        {
            var result = new List<IItem>();
            List<ItemSlot> slotsToCheck = new List<ItemSlot> { ItemSlot.Knife, ItemSlot.MainWeapon, ItemSlot.Pistol };
            foreach (var gangMember in CharacterSingleton.Instance.PlayersGang)
            {
                foreach (var slot in slotsToCheck)
                {
                    if (gangMember.UsedItems.ContainsKey(slot) && gangMember.UsedItems[slot] != null)
                    {
                        result.Add(gangMember.UsedItems[slot]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Generate List of Items which the gang owns
        /// </summary>
        /// <returns></returns>
        private List<IItem> GetHQItems()
        {
            var result = new List<IItem>();
            List<ItemSlot> slotsToCheck = new List<ItemSlot> { ItemSlot.Knife, ItemSlot.MainWeapon, ItemSlot.Pistol };
            foreach (var gangMember in CharacterSingleton.Instance.PlayersGang)
            {
                foreach (var slot in slotsToCheck)
                {
                    if (gangMember.UsedItems.ContainsKey(slot) && gangMember.UsedItems[slot] != null)
                    {
                        result.Add(gangMember.UsedItems[slot]);
                    }
                }
            }

            return result;
        }
    }
}
﻿using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;

namespace Items
{
    public class ItemVisualizer
    {
        private Dictionary<int, Dictionary<ItemIdentifiers, Text>> _items = new Dictionary<int, Dictionary<ItemIdentifiers, Text>>();
        private Dictionary<ItemSlot, Dictionary<ItemIdentifiers, Text>> _itemsViaSlot = new Dictionary<ItemSlot, Dictionary<ItemIdentifiers, Text>>();

        public Dictionary<int, Image> ItemImages { get; private set; }
        public Dictionary<ItemSlot, Image> ItemImagesViaSlot { get; private set; }

        /// <summary>
        /// Creates new instance
        /// </summary>
        public ItemVisualizer()
        {
            ItemImages = new Dictionary<int, Image>();
            ItemImagesViaSlot = new Dictionary<ItemSlot, Image>();
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemImage"></param>
        public void AddItem(int key, Image itemImage)
        {
            _items.Add(key, new Dictionary<ItemIdentifiers, Text>());
            var itemTexts = itemImage.GetComponentsInChildren<Text>();

            ItemImages.Add(key, itemImage);

            _items[key].Add(ItemIdentifiers.Name, itemTexts.First(tx => tx.gameObject.name == "NameText"));
            _items[key].Add(ItemIdentifiers.Type, itemTexts.First(tx => tx.gameObject.name == "TypeText"));
            _items[key].Add(ItemIdentifiers.Property1Type, itemTexts.First(tx => tx.gameObject.name == "Property1Container"));
            _items[key].Add(ItemIdentifiers.Property2Type, itemTexts.First(tx => tx.gameObject.name == "Property2Container"));
            _items[key].Add(ItemIdentifiers.Property3Type, itemTexts.First(tx => tx.gameObject.name == "Property3Container"));
            _items[key].Add(ItemIdentifiers.Property1Val, itemTexts.First(tx => tx.gameObject.name == "Property1Text"));
            _items[key].Add(ItemIdentifiers.Property2Val, itemTexts.First(tx => tx.gameObject.name == "Property2Text"));
            _items[key].Add(ItemIdentifiers.Property3Val, itemTexts.First(tx => tx.gameObject.name == "Property3Text"));
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemImage"></param>
        public void AddItem(ItemSlot slot, Image itemImage)
        {
            _itemsViaSlot.Add(slot, new Dictionary<ItemIdentifiers, Text>());
            var itemTexts = itemImage.GetComponentsInChildren<Text>();

            ItemImagesViaSlot.Add(slot, itemImage);

            _itemsViaSlot[slot].Add(ItemIdentifiers.Name, itemTexts.First(tx => tx.gameObject.name == "NameText"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Type, itemTexts.First(tx => tx.gameObject.name == "TypeText"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Property1Type, itemTexts.First(tx => tx.gameObject.name == "Property1Container"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Property2Type, itemTexts.First(tx => tx.gameObject.name == "Property2Container"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Property3Type, itemTexts.First(tx => tx.gameObject.name == "Property3Container"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Property1Val, itemTexts.First(tx => tx.gameObject.name == "Property1Text"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Property2Val, itemTexts.First(tx => tx.gameObject.name == "Property2Text"));
            _itemsViaSlot[slot].Add(ItemIdentifiers.Property3Val, itemTexts.First(tx => tx.gameObject.name == "Property3Text"));
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<ItemIdentifiers, Text> GetItem(int key)
        {
            if (_items.ContainsKey(key))
            {
                return _items[key];
            }
            else
            {
                Debug.LogError("GetItem(): Key " + key.ToString() + " does not exist!");
            }

            return new Dictionary<ItemIdentifiers, Text>();
        }

        /// <summary>
        /// Show all weapons
        /// </summary>
        /// <param name="_actualMember"></param>
        public void VisualizeWeapons(IGangMember _actualMember)
        {
            foreach (var pair in _actualMember.UsedItems)
            {
                if (pair.Value != null)
                {
                    var itemToBeFilled = _itemsViaSlot[pair.Key];
                    FillItem(itemToBeFilled, pair.Value);
                }
            }
        }

        /// <summary>
        /// Visualizes an item via key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public void VisualizeItem(int key, IItem item)
        {
            var itemToBeFilled = _items[key];
            FillItem(itemToBeFilled, item);
        }

        /// <summary>
        /// Fills the4 items
        /// </summary>
        /// <param name="itemSlot"></param>
        /// <param name="item"></param>
        private void FillItem(Dictionary<ItemIdentifiers, Text> itemSlot, IItem item)
        {
            itemSlot[ItemIdentifiers.Name].text = item.Name;
            itemSlot[ItemIdentifiers.Property1Type].text = item.GetValue(ItemIdentifiers.Property1Type);
            itemSlot[ItemIdentifiers.Property2Type].text = item.GetValue(ItemIdentifiers.Property2Type);
            itemSlot[ItemIdentifiers.Property3Type].text = item.GetValue(ItemIdentifiers.Property3Type);
            itemSlot[ItemIdentifiers.Property1Val].text = item.GetValue(ItemIdentifiers.Property1Val);
            itemSlot[ItemIdentifiers.Property2Val].text = item.GetValue(ItemIdentifiers.Property2Val);
            itemSlot[ItemIdentifiers.Property3Val].text = item.GetValue(ItemIdentifiers.Property3Val);
        }
    }
}

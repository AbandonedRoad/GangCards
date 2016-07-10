using Assets.Script.Actions;
using Enum;
using Interfaces;
using Items;
using Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ItemHandler : MonoBehaviour
    {
        private Delegate _itemClickedResponse;
        private IItem _firstItem;
        private IItem _selectedItem;
        private GameObject _itemPanel;
        private ItemVisualizer _itemVisualizer = new ItemVisualizer();
        private List<Image> _itemSlots;
        private Button _filterWeaponButton;
        private Button _filterArmorButton;
        private Button _buyButton;
        private ItemSlot _itemTypeToBeSelected = ItemSlot.NotSet;
        private Text _priceText;
        private Text _priceValue;
        private bool _isShop;
        private int _shopLevel;
        private ItemType _filterForType = ItemType.NotSet;
        private List<IItem> _itemCollection;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _itemPanel = GameObject.Find("ItemSelectorPanelPrefab");

            var buttons = _itemPanel.GetComponentsInChildren<Button>();
            _filterWeaponButton = buttons.First(btn => btn.gameObject.name == "WeaponButton");
            _filterArmorButton = buttons.First(btn => btn.gameObject.name == "ArmorButton");
            _buyButton = buttons.First(btn => btn.gameObject.name == "BuyButton");

            var texts = _itemPanel.GetComponentsInChildren<Text>();
            _priceText = texts.First(btn => btn.gameObject.name == "PriceText");
            _priceValue = texts.First(btn => btn.gameObject.name == "PriceValue");

            _itemSlots = _itemPanel.GetComponentsInChildren<Image>().ToList();
            for (int i = 1; i < 7; i++)
            {
                var name = String.Concat("ItemSlot", i.ToString(), "Prefab");
                var actItem = _itemSlots.First(itm => itm.gameObject.name == name);
                _itemVisualizer.AddItem(i, actItem);

                _itemSlots.First(itm => itm.gameObject.name == name).gameObject.SetActive(false);
            }

            _itemPanel.SetActive(false);
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchItemPanel(bool isShop)
        {
            _isShop = isShop;

            _itemCollection = _isShop
                ? ItemSingleton.Instance.AvailableItems.Where(itm => itm.Level <= _shopLevel 
                    && (itm is Weapon && ((Weapon)itm).WeaponType != WeaponType.Bite && ((Weapon)itm).WeaponType != WeaponType.Claws)).ToList()
                : ItemSingleton.Instance.OwnedItems;

            LoadItems();
            _itemPanel.SetActive(!_itemPanel.activeSelf);
            if (_itemPanel.activeSelf)
            {
                _itemPanel.transform.SetAsLastSibling();

                _priceText.gameObject.SetActive(_isShop);
                _priceValue.gameObject.SetActive(_isShop);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemNumber"></param>
        public void ItemClicked(int itemNumber)
        {
            var name = _itemVisualizer.GetItem(itemNumber)[ItemIdentifiers.Name].text;
            var key = _itemVisualizer.GetItemKey(itemNumber);
            _selectedItem = _itemCollection.FirstOrDefault(itm => itm.Key == key);

            if (_itemClickedResponse != null)
            {
                _itemClickedResponse.DynamicInvoke(_selectedItem, _itemTypeToBeSelected);
                _itemClickedResponse = null;

                SwitchItemPanel(_isShop);
            }
            else if (_isShop)
            {
                // Show price.
                _priceValue.text = ItemSingleton.Instance.ReturnPriceForItem(_selectedItem).ToString() + "$";
                _buyButton.interactable = _selectedItem != null;
            }
        }

        /// <summary>
        /// Selects an item
        /// </summary>
        /// <param name="type"></param>
        public void OpenItemScreen(IGangMember assignTo, ItemSlot type, int shopLevel, Delegate itemClickedResponse)
        {
            _shopLevel = shopLevel;
            if (assignTo == null && _shopLevel == 0)
            {
                // shop level 0 mean we have no shop
                return;
            }

            _filterForType = (type == ItemSlot.Chest || type == ItemSlot.Pants)
                ? ItemType.Armor
                : ItemType.Weapon;

            _itemTypeToBeSelected = type;
            _itemClickedResponse = itemClickedResponse;
            _selectedItem = null;

            SwitchItemPanel(_shopLevel > 0);
        }

        /// <summary>
        /// Filter for specific type
        /// </summary>
        /// <param name="type"></param>
        public void FilterItems(int type)
        {
            _filterForType = type == 1
                ? ItemType.Weapon
                : ItemType.Armor;

            LoadItems();
        }

        /// <summary>
        /// Buy an item
        /// </summary>
        public void BuyItem()
        {
            StartCoroutine(BuyItemQuestion());
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        public void PreviousEntries()
        {
            if (_firstItem == null)
            {
                return;
            }

            var index = _itemCollection.IndexOf(_firstItem);
            if ((index - 6) < 0)
            {
                // Switch to first item
                _firstItem = _itemCollection.FirstOrDefault();
                return;
            }
            else
            {
                _firstItem = _itemCollection.ElementAt(index - 6);
            }

            LoadItems();
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        public void NextEntries()
        {
            if (_firstItem == null)
            {
                return;
            }

            var index = _itemCollection.IndexOf(_firstItem);
            if ((index + 6) >= _itemCollection.Count)
            {
                // We reached the end!
                return;
            }
            else
            {
                _firstItem = _itemCollection.ElementAt(index + 6);
            }

            LoadItems();
        }

        /// <summary>
        /// Buy the item bitch!
        /// </summary>
        /// <returns></returns>
        private IEnumerator BuyItemQuestion()
        {
            var price = ItemSingleton.Instance.ReturnPriceForItem(_selectedItem);
            if (_selectedItem != null && CharacterSingleton.Instance.AvailableMoney >= price )
            {
                // User has enough money.
                PrefabSingleton.Instance.InputHandler.AddQuestion("BuyWeaponsReally");
                yield return StartCoroutine(PrefabSingleton.Instance.InputHandler.WaitForAnswer());

                if (PrefabSingleton.Instance.InputHandler.AnswerGiven == 2)
                {
                    // User decided not to buy
                    yield break;
                }
                else
                {
                    CharacterSingleton.Instance.AvailableMoney -= price;
                    ItemSingleton.Instance.OwnedItems.Add(_selectedItem.Clone());
                }
            }
        }

        /// <summary>
        /// Loads all ietms.
        /// </summary>
        private void LoadItems()
        {
            if (_itemSlots == null)
            {
                return;
            }

            _firstItem = _firstItem ?? _itemCollection.FirstOrDefault();
            if (_firstItem == null)
            {
                // No items available.
                return;
            }

            var index = _itemCollection.IndexOf(_firstItem);
            for (int i = 1; i < 7; i++)
            {
                var item = _itemCollection.ElementAtOrDefault(index);
                index++;
                var name = String.Concat("ItemSlot", i.ToString(), "Prefab");
                _itemSlots.First(itm => itm.gameObject.name == name).gameObject.SetActive(item != null);
                if (item == null 
                    || (item.UsedInSlot != _itemTypeToBeSelected && _itemTypeToBeSelected != ItemSlot.NotSet)
                    || (item.ItemType != _filterForType && _filterForType != ItemType.NotSet))
                {
                    _itemSlots.First(itm => itm.gameObject.name == name).gameObject.SetActive(false);
                    continue;
                }

                var itemSlot = _itemVisualizer.GetItem(i);
                _itemVisualizer.VisualizeItem(i, item);
            }
        }
    }
}
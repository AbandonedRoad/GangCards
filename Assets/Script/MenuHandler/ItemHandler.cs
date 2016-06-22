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
        private Button _nextButton;
        private Button _previousButton;
        private Button _buyButton;
        private ItemSlot _itemTypeToBeSelected = ItemSlot.NotSet;
        private Text _priceText;
        private Text _priceValue;
        private bool _isShop;
        private ItemType _filterForType = ItemType.NotSet;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _itemPanel = GameObject.Find("ItemSelectorPanelPrefab");

            var buttons = _itemPanel.GetComponentsInChildren<Button>();
            _filterWeaponButton = buttons.First(btn => btn.gameObject.name == "WeaponButton");
            _filterArmorButton = buttons.First(btn => btn.gameObject.name == "ArmorButton");
            _nextButton = buttons.First(btn => btn.gameObject.name == "NextButton");
            _previousButton = buttons.First(btn => btn.gameObject.name == "PreviousButton");
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
            _selectedItem = ItemSingleton.Instance.OwnedItems.FirstOrDefault(itm => itm.Name == name);

            if (_itemClickedResponse != null)
            {
                _itemClickedResponse.DynamicInvoke(_selectedItem, _itemTypeToBeSelected);
                _itemClickedResponse = null;

                SwitchItemPanel(_isShop);
            }
            else if (_isShop)
            {
                // Buy stuff!
                _buyButton.interactable = _selectedItem != null;
            }
        }

        /// <summary>
        /// Selects an item
        /// </summary>
        /// <param name="type"></param>
        public void SelectItem(IGangMember assignTo, ItemSlot type, bool isShop, Delegate itemClickedResponse)
        {
            if (assignTo == null && !isShop)
            {
                return;
            }

            _filterForType = (type == ItemSlot.Chest || type == ItemSlot.Pants)
                ? ItemType.Armor
                : ItemType.Weapon;

            _itemTypeToBeSelected = type;
            _itemClickedResponse = itemClickedResponse;
            _selectedItem = null;

            SwitchItemPanel(isShop);
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

        public void BuyItem()
        {
            StartCoroutine(BuyItemQuestion());
        }

        /// <summary>
        /// Buy the item bitch!
        /// </summary>
        /// <returns></returns>
        private IEnumerator BuyItemQuestion()
        {
            if (_selectedItem == null)
            {
                if (CharacterSingleton.Instance.AvailableMoney < 5)
                {
                    // Genug Geld da?
                }

                PrefabSingleton.Instance.InputHandler.AddQuestion("BuyWeaponsReally");
                yield return StartCoroutine(PrefabSingleton.Instance.InputHandler.WaitForAnswer());

                if (PrefabSingleton.Instance.InputHandler.AnswerGiven == 2)
                {
                    // User decided not to buy
                    yield break;
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

            _firstItem = _firstItem ?? ItemSingleton.Instance.OwnedItems.First();

            var index = ItemSingleton.Instance.OwnedItems.IndexOf(_firstItem);
            for (int i = 1; i < 7; i++)
            {
                var item = ItemSingleton.Instance.OwnedItems.ElementAtOrDefault(index);
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
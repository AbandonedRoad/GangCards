using Assets.Script.Actions;
using Enum;
using Interfaces;
using Items;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ItemHandler : MonoBehaviour
    {
        public GameObject Panel
        {
            get { return _itemPanel; }
        }

        private Delegate _itemClickedResponse;
        private IItem _firstItem;
        private GameObject _itemPanel;
        private ItemVisualizer _itemVisualizer = new ItemVisualizer();
        private List<Image> _itemSlots;
        private Button _filterWeaponButton;
        private Button _filterArmorButton;
        private Button _nextButton;
        private Button _previousButton;
        private ItemSlot _itemTypeToBeSelected = ItemSlot.NotSet;

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
        public void SwitchItemPanel()
        {
            LoadItems();
            _itemPanel.SetActive(!_itemPanel.activeSelf);
            if (_itemPanel.activeSelf)
            {
                _itemPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemNumber"></param>
        public void ItemClicked(int itemNumber)
        {
            var name = _itemVisualizer.GetItem(itemNumber)[ItemIdentifiers.Name].text;
            var item = ItemSingleton.Instance.OwnedItems.FirstOrDefault(itm => itm.Name == name);

            if (_itemClickedResponse != null)
            {
                _itemClickedResponse.DynamicInvoke(item, _itemTypeToBeSelected);
                _itemClickedResponse = null;

                SwitchItemPanel();
            }
        }

        /// <summary>
        /// Selects an item
        /// </summary>
        /// <param name="type"></param>
        public void SelectItem(ItemSlot type, Delegate itemClickedResponse)
        {
            _itemTypeToBeSelected = type;
            _itemClickedResponse = itemClickedResponse;

            SwitchItemPanel();
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
                if (item == null || item.UsedInSlot != _itemTypeToBeSelected)
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
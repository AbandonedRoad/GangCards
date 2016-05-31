using Enum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;
using System.Linq;
using Singleton;
using System;

namespace Humans
{
    public class GangVisualizer
    {
        private Dictionary<string, Dictionary<string, Text>> _gangs = new Dictionary<string, Dictionary<string, Text>>();
        private Dictionary<string, Image> _gangImages = new Dictionary<string, Image>();
        private Dictionary<string, IGangMember> _gangMembers = new Dictionary<string, IGangMember>();

        /// <summary>
        /// Inits visualizeer
        /// </summary>
        public void Init()
        {
            _gangMembers.Clear();
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemImage"></param>
        public void AddDetailledMember(string key, Image itemImage)
        {
            _gangs.Add(key, new Dictionary<string, Text>());
            var itemTexts = itemImage.GetComponentsInChildren<Text>();

            _gangImages.Add(key, itemImage);

            _gangs[key].Add("Name", itemTexts.First(tx => tx.gameObject.name == "NameText"));
            _gangs[key].Add("StreeName", itemTexts.First(tx => tx.gameObject.name == "StreetNameText"));
            _gangs[key].Add("Accuracy", itemTexts.First(tx => tx.gameObject.name == "AccuracyText"));
            _gangs[key].Add("Courage", itemTexts.First(tx => tx.gameObject.name == "CourageText"));
            _gangs[key].Add("Initiative", itemTexts.First(tx => tx.gameObject.name == "InitiativeText"));
            _gangs[key].Add("Intelligence", itemTexts.First(tx => tx.gameObject.name == "IntelligenceText"));
            _gangs[key].Add("Strength", itemTexts.First(tx => tx.gameObject.name == "StrengthText"));
            _gangs[key].Add("Level", itemTexts.First(tx => tx.gameObject.name == "LevelText"));
        }

        /// <summary>
        /// Adds a new item
        /// </summary>
        /// <param name="key"></param>
        /// <param name="itemImage"></param>
        public void AddShortMember(string key, Image itemImage)
        {
            _gangs.Add(key, new Dictionary<string, Text>());
            var itemTexts = itemImage.GetComponentsInChildren<Text>();

            _gangImages.Add(key, itemImage);

            _gangs[key].Add("Name", itemTexts.First(tx => tx.gameObject.name == "NameText"));
            _gangs[key].Add("StreeName", itemTexts.First(tx => tx.gameObject.name == "StreetNameText"));
        }

        /// <summary>
        /// Gets an item.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, Text> GetItem(string key)
        {
            if (_gangs.ContainsKey(key))
            {
                return _gangs[key];
            }
            else
            {
                Debug.LogError("GetItem(): Key " + key.ToString() + " does not exist!");
            }

            return new Dictionary<string, Text>();
        }

        /// <summary>
        /// Visualizes an item via key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public void VisualizeMember(string key, IGangMember member)
        {
            var itemToBeFilled = _gangs[key];

            _gangMembers.Add(key, member);

            FillItem(itemToBeFilled, member);
        }

        /// <summary>
        /// An entry is selected.
        /// </summary>
        /// <param name="key"></param>
        public IGangMember SelectEntry(string key)
        {
            var itemToBeFilled = _gangs[key];

            _gangImages.Values.ToList().ForEach(spr => spr.sprite = ResourceSingleton.Instance.BackgroundDeSelected);

            if (_gangMembers.ContainsKey(key))
            {
                _gangImages[key].sprite = ResourceSingleton.Instance.BackgroundSelected;
                return _gangMembers[key];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Hides or shows a slot. Depends if a member does exist or not.
        /// </summary>
        /// <param name="key">field to be adapted</param>
        /// <param name="isVisible">Visible or not.</param>
        public void HideShowItems(string key, bool isVisible)
        {
            var itemToBeManipulated = _gangs[key];
            itemToBeManipulated.Values.ToList().ForEach(go => go.gameObject.SetActive(isVisible));
            _gangImages[key].gameObject.SetActive(isVisible);
        }

        /// <summary>
        /// Gets the selected entry
        /// </summary>
        /// <returns></returns>
        public IGangMember GetSelectedItem()
        {
            var selected = _gangImages.Values.ToList().FirstOrDefault(spr => spr.gameObject.activeSelf
                && spr.sprite == ResourceSingleton.Instance.BackgroundSelected);
            if (selected == null)
            {
                // Nothing is selected
                return null;
            }

            var pair = _gangImages.First(par => par.Value.sprite == ResourceSingleton.Instance.BackgroundSelected);
            return _gangMembers[pair.Key];
        }

        /// <summary>
        /// Fills the4 items
        /// </summary>
        /// <param name="itemSlot"></param>
        /// <param name="item"></param>
        private void FillItem(Dictionary<string, Text> itemSlot, IGangMember member)
        {
            itemSlot["Name"].text = member.Name;
            itemSlot["StreeName"].text = member.ActiveStreeName;

            if (!itemSlot.ContainsKey("Accuracy"))
            {
                // We are painting a member which only has a short profile.
                return;
            }

            itemSlot["Accuracy"].text = member.Accuracy.ToString();
            itemSlot["Courage"].text = member.Courage.ToString();
            itemSlot["Initiative"].text = member.Initiative.ToString();
            itemSlot["Intelligence"].text = member.Intelligence.ToString();
            itemSlot["Strength"].text = member.Strength.ToString();
            itemSlot["Level"].text = member.Level.ToString();
        }
    }
}
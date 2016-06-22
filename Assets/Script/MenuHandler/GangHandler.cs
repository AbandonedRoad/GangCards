using Enum;
using Interfaces;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class GangHandler : MonoBehaviour
    {
        private IGangMember _actualMember;
        private GameObject _gangPanel;
        private Button _previousButton;
        private Button _nextButton;
        private Button _previousStreetNameButton;
        private Button _nextStreetNameButton;
        private Dictionary<ItemSlot, Button> _itemButtons = new Dictionary<ItemSlot, Button>();
        private Dictionary<ItemSlot, Text> _itemTexts = new Dictionary<ItemSlot, Text>();
        private Text _name;
        private Text _streeName;
        private Text _intelligence;
        private Text _strength;
        private Text _accuracy;
        private Text _courage;
        private Text _initiative;
        private Text _level;
        private Text _gangName;
        private Text _gangLevel;
        private Text _locationText;
        private GameObject _memberInfosContainer;
        private Image _gangImage;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _gangPanel = GameObject.Find("GangPanel");

            _memberInfosContainer = _gangPanel.GetComponentsInChildren<Transform>().First(go => go.gameObject.name == "MemberInfos").gameObject;

            var texts = _gangPanel.GetComponentsInChildren<Text>().ToList();
            _name = texts.First(tx => tx.gameObject.name == "NameText");
            _streeName = texts.First(tx => tx.gameObject.name == "StreetNameText");
            _intelligence = texts.First(tx => tx.gameObject.name == "IntelligenceText");
            _strength = texts.First(tx => tx.gameObject.name == "StrengthText");
            _accuracy = texts.First(tx => tx.gameObject.name == "AccuracyText");
            _courage = texts.First(tx => tx.gameObject.name == "CourageText");
            _initiative = texts.First(tx => tx.gameObject.name == "InitiativeText");
            _level = texts.First(tx => tx.gameObject.name == "LevelText");
            _gangName = texts.First(tx => tx.gameObject.name == "GangNameText");
            _gangLevel = texts.First(tx => tx.gameObject.name == "GangLevelText");
            _locationText = texts.First(tx => tx.gameObject.name == "LocationText");

            var images = _gangPanel.GetComponentsInChildren<Image>().ToList();
            _gangImage = images.FirstOrDefault(img => img.gameObject.name == "GangImage");

            var buttons = _gangPanel.GetComponentsInChildren<Button>();
            _nextButton = buttons.First(tx => tx.gameObject.name == "NextButton");
            _previousButton = buttons.First(tx => tx.gameObject.name == "PreviousButton");
            _nextStreetNameButton = buttons.First(tx => tx.gameObject.name == "NextStreetNameButton");
            _previousStreetNameButton = buttons.First(tx => tx.gameObject.name == "PreviousStreetNameButton");

            _itemButtons.Add(ItemSlot.Chest, buttons.First(tx => tx.gameObject.name == "ChestButton"));
            _itemButtons.Add(ItemSlot.Knife, buttons.First(tx => tx.gameObject.name == "KnifeButton"));
            _itemButtons.Add(ItemSlot.MainWeapon, buttons.First(tx => tx.gameObject.name == "MainWeaponButton"));
            _itemButtons.Add(ItemSlot.Pants, buttons.First(tx => tx.gameObject.name == "PantsButton"));
            _itemButtons.Add(ItemSlot.Pistol, buttons.First(tx => tx.gameObject.name == "PistolButton"));

            _itemTexts.Add(ItemSlot.Chest, texts.First(tx => tx.gameObject.name == "ChestText"));
            _itemTexts.Add(ItemSlot.Knife, texts.First(tx => tx.gameObject.name == "KnifeText"));
            _itemTexts.Add(ItemSlot.MainWeapon, texts.First(tx => tx.gameObject.name == "MainWeaponText"));
            _itemTexts.Add(ItemSlot.Pants, texts.First(tx => tx.gameObject.name == "PantsText"));
            _itemTexts.Add(ItemSlot.Pistol, texts.First(tx => tx.gameObject.name == "PistolText"));

            _nextButton.onClick.AddListener(() => NextGangster());
            _previousButton.onClick.AddListener(() => PreviousGangster());

            _nextStreetNameButton.onClick.AddListener(() => NextStreeName());
            _previousStreetNameButton.onClick.AddListener(() => PreviousStreeName());

            foreach (ItemSlot slot in System.Enum.GetValues(typeof(ItemSlot)))
            {
                if (slot == ItemSlot.NotSet)
                {
                    continue;
                }
                var useSlot = slot;
                _itemButtons[useSlot].onClick.AddListener(() => PrefabSingleton.Instance.ItemHandler.SelectItem(_actualMember, useSlot, false, new Func<IItem, ItemSlot, bool>(NewItemSelected)));
            }

            GUIHelper.ReplaceText(texts);

            _memberInfosContainer.SetActive(false);
            FillLabels();

            SwitchGangPanel();
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchGangPanel()
        {
            _gangPanel.SetActive(!_gangPanel.activeSelf);
            if (_gangPanel.activeSelf)
            {
                _actualMember = _actualMember ?? CharacterSingleton.Instance.PlayersGang.FirstOrDefault();
                if (_actualMember != null)
                {
                    _gangImage.sprite = ResourceSingleton.Instance.Logos[_actualMember.GangAssignment];
                }
                FillLabels();

                _gangPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// This is fired if an item was selected.
        /// </summary>
        private bool NewItemSelected(IItem selectedItem, ItemSlot desiredSlot)
        {
            _actualMember.UsedItems[desiredSlot] = selectedItem;
            LoadItemsFromGangster();

            return true;
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        private void PreviousGangster()
        {
            if (_actualMember == null)
            {
                _actualMember = CharacterSingleton.Instance.PlayersGang.FirstOrDefault();
            }
            else
            {
                var index = CharacterSingleton.Instance.PlayersGang.IndexOf(_actualMember);
                if (index == 0)
                {
                    _actualMember = CharacterSingleton.Instance.PlayersGang.LastOrDefault();
                }
                else
                {
                    index--;
                    _actualMember = CharacterSingleton.Instance.PlayersGang.ElementAt(index);
                }
            }
            FillLabels();
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        private void PreviousStreeName()
        {
            if (_actualMember == null || _actualMember.StreetName.Count == 0)
            {
                return;
            }

            var index = _actualMember.StreetName.IndexOf(_actualMember.ActiveStreeName);
            if (index == 0)
            {
                _actualMember.ActiveStreeName = _actualMember.StreetName.LastOrDefault();
            }
            else
            {
                index--;
                _actualMember.ActiveStreeName = _actualMember.StreetName.ElementAt(index);
            }

            FillLabels();
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        private void NextStreeName()
        {
            if (_actualMember == null || _actualMember.StreetName.Count == 0)
            {
                return;
            }

            var index = _actualMember.StreetName.IndexOf(_actualMember.ActiveStreeName);
            if (index == _actualMember.StreetName.Count - 1)
            {
                _actualMember.ActiveStreeName = _actualMember.StreetName.FirstOrDefault();
            }
            else
            {
                index++;
                _actualMember.ActiveStreeName = _actualMember.StreetName.ElementAt(index);
            }

            FillLabels();
        }

        /// <summary>
        /// Get next gangster
        /// </summary>
        private void NextGangster()
        {
            if (_actualMember == null)
            {
                _actualMember = CharacterSingleton.Instance.PlayersGang.FirstOrDefault();
            }
            else
            {
                var index = CharacterSingleton.Instance.PlayersGang.IndexOf(_actualMember);
                if (index == CharacterSingleton.Instance.PlayersGang.Count -1)
                {
                    _actualMember = CharacterSingleton.Instance.PlayersGang.FirstOrDefault();
                }
                else
                {
                    index++;
                    _actualMember = CharacterSingleton.Instance.PlayersGang.ElementAt(index);
                }
            }
            FillLabels();
        }

        /// <summary>
        /// Loads actual items.
        /// </summary>
        private void LoadItemsFromGangster()
        {
            if (_actualMember == null)
            {
                return;
            }

            foreach (var pair in _actualMember.UsedItems)
            {
                var actCol = _itemTexts[pair.Key].color;              
                _itemTexts[pair.Key].color = pair.Value == null ? Color.gray : Color.black;
            }
        }

        /// <summary>
        /// Fills the labels.
        /// </summary>
        /// <param name="member"></param>
        private void FillLabels()
        {
            _memberInfosContainer.SetActive(_actualMember != null);

            _gangName.text = HelperSingleton.Instance.SplitUp(CharacterSingleton.Instance.GangOfPlayer.ToString());
            _gangLevel.text = CharacterSingleton.Instance.GangLevel.ToString();

            _name.text = _actualMember != null ? _actualMember.Name : String.Empty;
            _streeName.text = _actualMember != null ? _actualMember.ActiveStreeName : String.Empty;
            _intelligence.text = _actualMember != null ? _actualMember.Intelligence.ToString() : String.Empty;
            _strength.text = _actualMember != null ? _actualMember.Strength.ToString() : String.Empty;
            _level.text = _actualMember != null ? _actualMember.Level.ToString() : String.Empty;
            _initiative.text = _actualMember != null ? _actualMember.Initiative.ToString() : String.Empty;
            _accuracy.text = _actualMember != null ? _actualMember.Accuracy.ToString() : String.Empty;
            _courage.text = _actualMember != null ? _actualMember.Courage.ToString() : String.Empty;

            var locText = String.Empty;
            bool found = CharacterSingleton.Instance.PlayerMembersInCar.Contains(_actualMember)
                ? ResourceSingleton.Instance.GetText("GangMemberLocationCar", out locText)
                : ResourceSingleton.Instance.GetText("GangMemberLocationHQ", out locText);
            _locationText.text = locText;

            LoadItemsFromGangster();
        }
    }
}
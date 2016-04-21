using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Singleton;
using Interfaces;
using Enum;

namespace Menu
{
    public class FightingHandler : MonoBehaviour
    {
        private GameObject _fightingPanel;
        private Text _combatLog;
        private Dictionary<int, Image> _nextPictures = new Dictionary<int, Image>();
        private Dictionary<int, Text> _nextNames = new Dictionary<int, Text>();
        private List<IGangMember> _allCombatants;
        private Dictionary<int, Image> _actions = new Dictionary<int, Image>();
        private IGangMember _actualMember;
        private Button _nextRoundButton;
        private Button _fleeButton;
        private bool _isPlayersTurn;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _fightingPanel = GameObject.Find("FightPanelPrefab");

            var texts = _fightingPanel.GetComponentsInChildren<Text>();
            _combatLog = texts.First(tx => tx.name == "CombatLogText");
            _nextNames.Add(0, texts.First(img => img.gameObject.name == "NameNext1Text"));
            _nextNames.Add(1, texts.First(img => img.gameObject.name == "NameNext2Text"));
            _nextNames.Add(2, texts.First(img => img.gameObject.name == "NameNext3Text"));
            _nextNames.Add(3, texts.First(img => img.gameObject.name == "NameNext4Text"));
            _nextNames.Add(4, texts.First(img => img.gameObject.name == "NameNext5Text"));

            var images = _fightingPanel.GetComponentsInChildren<Image>();
            _nextPictures.Add(0, images.First(img => img.gameObject.name == "Next1Image"));
            _nextPictures.Add(1, images.First(img => img.gameObject.name == "Next2Image"));
            _nextPictures.Add(2, images.First(img => img.gameObject.name == "Next3Image"));
            _nextPictures.Add(3, images.First(img => img.gameObject.name == "Next4Image"));
            _nextPictures.Add(4, images.First(img => img.gameObject.name == "Next5Image"));

            _actions.Add(1, images.First(img => img.gameObject.name == "ItemMainWeapon"));
            _actions.Add(2, images.First(img => img.gameObject.name == "ItemPistol"));
            _actions.Add(3, images.First(img => img.gameObject.name == "ItemKnife"));

            var buttons = _fightingPanel.GetComponentsInChildren<Button>();
            _nextRoundButton = buttons.First(btn => btn.gameObject.name == "NextRoundButton");
            _fleeButton = buttons.First(btn => btn.gameObject.name == "FleeButton");

            SwitchFightingPanel(false);
        }

        /// <summary>
        /// Updates.
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// An action was clicked.
        /// </summary>
        /// <param name="slot"></param>
        public void ActionClicked(int slot)
        {
            var text = ResourceSingleton.Instance.GetText("FightAction" + slot.ToString());
            _combatLog.text = text + Environment.NewLine + _combatLog.text;

            float damage = 0f;
            ItemSlot type = ItemSlot.NotSet;
            switch (slot)
            {
                case 1:     // Main Weapon
                    type = ItemSlot.MainWeapon;
                    break;
                case 2:     // Pistol
                    type = ItemSlot.Pistol;
                    break;
                case 3:     // Knife
                    type = ItemSlot.Knife;
                    break;
                default:
                    Debug.LogError("FightingHandler.ActionClicked(int slot): Number " + slot + " is not valid!");
                    break;
            }

            bool attackSuccessful = _actualMember.UsedItems[type].ItemStragegy.ExecuteAction();
            var result = _actualMember.UsedItems[type].ItemStragegy.GetOutpt();

            _combatLog.text = result.Message;
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchFightingPanel(bool newVisibility)
        {
            _fightingPanel.SetActive(newVisibility);
            if (_fightingPanel.activeSelf)
            {
                _fightingPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// Switches teh round
        /// </summary>
        public void NextRound()
        {
            _isPlayersTurn = !_isPlayersTurn;
            var nextIndex = _allCombatants.IndexOf(_actualMember) + 1;
            _actualMember = _allCombatants.Count >= nextIndex ? _allCombatants.First() : _allCombatants.ElementAt(nextIndex);
            UpdateActionBar();
            UpdateButtons();

            if (!_isPlayersTurn)
            {
                HanldeAIPlayers();
            }
        }

        /// <summary>
        /// Flee form Battle
        /// </summary>
        public void FleeFromBattle()
        {
            SwitchFightingPanel(false);
        }

        /// <summary>
        /// Starts a fight!
        /// </summary>
        public void StartFight(List<IGangMember> opponents)
        {
            _allCombatants = opponents.ToList();
            _allCombatants.AddRange(CharacterSingleton.Instance.PlayerMembersInCar);
            _allCombatants = _allCombatants.OrderBy(mem => mem.Intiative).ToList();
            _actualMember = _allCombatants.First();

            _isPlayersTurn = CharacterSingleton.Instance.PlayerMembersInCar.Contains(_actualMember);

            UpdateActionBar();
            UpdateButtons();

            SwitchFightingPanel(true);
        }

        /// <summary>
        /// Handle attacks from AI Players.
        /// </summary>
        private void HanldeAIPlayers()
        {
            _actualMember.UsedItems[ItemSlot.MainWeapon].ItemStragegy.ExecuteAction();
            var result = _actualMember.UsedItems[ItemSlot.MainWeapon].ItemStragegy.GetOutpt();
            _combatLog.text = result.Message;
        }

        /// <summary>
        /// Updates the buttons.
        /// </summary>
        private void UpdateButtons()
        {
            _fleeButton.interactable = _isPlayersTurn;
            // _nextRoundButton.interactable = _isPlayersTurn;

            foreach (var pair in _actions)
            {
                pair.Value.gameObject.SetActive(_isPlayersTurn);
            }
        }

        /// <summary>
        /// Updates the action bar.
        /// </summary>
        private void UpdateActionBar()
        {
            // Which person is in charge?
            _isPlayersTurn = CharacterSingleton.Instance.PlayerMembersInCar.Contains(_actualMember);

            var actualMember = _actualMember;

            var personIndex = _allCombatants.IndexOf(actualMember);
            for (int i = 0; i < 5; i++)
            {
                actualMember = _allCombatants.Count > personIndex ? _allCombatants.ElementAt(personIndex) : _allCombatants.First();
                _nextNames[i].text = actualMember.Name;
                _nextPictures[i].sprite = ResourceSingleton.Instance.ImageFaces[actualMember.ImageName];

                personIndex = _allCombatants.IndexOf(actualMember) + 1;
            }
        }
    }
}

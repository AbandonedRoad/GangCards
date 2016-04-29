using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Singleton;
using Interfaces;
using Enum;
using System.Collections;
using Items;

namespace Menu
{
    public class FightingHandler : MonoBehaviour
    {
        private GameObject _fightingPanel;
        private Text _combatLog;
        private Dictionary<int, Image> _nextPictures = new Dictionary<int, Image>();
        private Dictionary<int, Text> _nextNames = new Dictionary<int, Text>();
        private Dictionary<int, Text> _nextHPs = new Dictionary<int, Text>();
        private List<IGangMember> _allCombatants;
        private ItemVisualizer _itemVisualizer = new ItemVisualizer();
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
            var images = _fightingPanel.GetComponentsInChildren<Image>();
            for (int i = 0; i < 10; i++)
            {
                _nextNames.Add(i, texts.First(img => img.gameObject.name == String.Concat("NameNext", (i + 1), "Text")));
                _nextPictures.Add(i, images.First(img => img.gameObject.name == String.Concat("Next", (i + 1), "Image")));
                _nextHPs.Add(i, texts.First(img => img.gameObject.name == String.Concat("LifeText", (i + 1))));
            }

            _combatLog = texts.First(tx => tx.name == "CombatLogText");

            _itemVisualizer.AddItem(ItemSlot.MainWeapon, images.First(img => img.gameObject.name == "ItemMainWeapon"));
            _itemVisualizer.AddItem(ItemSlot.Pistol, images.First(img => img.gameObject.name == "ItemPistol"));
            _itemVisualizer.AddItem(ItemSlot.Knife, images.First(img => img.gameObject.name == "ItemKnife"));

            var buttons = _fightingPanel.GetComponentsInChildren<Button>();
            _nextRoundButton = buttons.First(btn => btn.gameObject.name == "NextRoundButton");
            _fleeButton = buttons.First(btn => btn.gameObject.name == "FleeButton");

            SwitchFightingPanel(false);
        }

        /// <summary>
        /// An action was clicked.
        /// </summary>
        /// <param name="slot"></param>
        public void ActionClicked(int slot)
        {
            StartCoroutine(HandleAttack(slot, true));
        }

        /// <summary>
        /// First part of the player attack fight
        /// </summary>
        /// <returns></returns>
        private IEnumerator HandleAttack(int slot, bool isPlayer)
        {
            var textKey = isPlayer 
                ? "FightAction" + slot.ToString() 
                : "FightActionAI" + slot.ToString();
            var text = ResourceSingleton.Instance.GetText(textKey);
            _combatLog.text = text + Environment.NewLine + _combatLog.text;

            yield return new WaitForSeconds(1f);

            if (isPlayer)
            {
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
                var result = _actualMember.UsedItems[type].ItemStragegy.GetOutpt(true);
                _combatLog.text = result.Message + Environment.NewLine + _combatLog.text;
            }
            else
            {
                _actualMember.UsedItems[ItemSlot.MainWeapon].ItemStragegy.ExecuteAction();
                var result = _actualMember.UsedItems[ItemSlot.MainWeapon].ItemStragegy.GetOutpt(false);
                _combatLog.text = result.Message + Environment.NewLine + _combatLog.text;

                _nextRoundButton.interactable = true;
            }
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchFightingPanel(bool newVisibility)
        {
            _fightingPanel.SetActive(newVisibility);
            if (_fightingPanel.activeSelf)
            {
                _combatLog.text = String.Empty;
                _fightingPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// Switches teh round
        /// </summary>
        public void NextRound()
        {
            _nextRoundButton.interactable = false;

            var nextIndex = _allCombatants.IndexOf(_actualMember) + 1;
            _actualMember = _allCombatants.Count > nextIndex ?  _allCombatants.ElementAt(nextIndex) : _allCombatants.First();
            UpdateActionBar();
            UpdateButtons();
            UpdateItemSlots();

            if (!_isPlayersTurn)
            {
                StartCoroutine(HandleAttack(1, false));
            }
        }

        /// <summary>
        /// Update the item slot
        /// </summary>
        private void UpdateItemSlots()
        { 
            foreach (var image in _itemVisualizer.ItemImages)
            {
                image.Value.gameObject.SetActive(_isPlayersTurn);
            }

            if (_isPlayersTurn)
            {
                _itemVisualizer.VisualizeWeapons(_actualMember);
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
            UpdateItemSlots();

            SwitchFightingPanel(true);
        }

        /// <summary>
        /// Updates the buttons.
        /// </summary>
        private void UpdateButtons()
        {
            _fleeButton.interactable = _isPlayersTurn;
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
            for (int i = 0; i < 10; i++)
            {
                actualMember = _allCombatants.Count > personIndex ? _allCombatants.ElementAt(personIndex) : _allCombatants.First();
                _nextNames[i].text = actualMember.Name;
                _nextPictures[i].sprite = ResourceSingleton.Instance.ImageFaces[actualMember.ImageName];
                _nextHPs[i].text = String.Concat("HP: ", actualMember.MaxHealth.ToString(), "/", actualMember.MaxHealth.ToString());

                personIndex = _allCombatants.IndexOf(actualMember) + 1;
            }
        }
    }
}

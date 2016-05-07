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
using UnityEngine.EventSystems;

namespace Menu
{
    public class FightingHandler : MonoBehaviour
    {
        private GameObject _fightingPanel;
        private Text _combatLog;
        private Dictionary<int, IGangMember> _combatantsViaKey = new Dictionary<int, IGangMember>();
        private Dictionary<int, Text> _nextPictureText = new Dictionary<int, Text>();
        private Dictionary<int, Text> _nextNames = new Dictionary<int, Text>();
        private Dictionary<int, Text> _nextHPs = new Dictionary<int, Text>();
        private Dictionary<int, EventTrigger> _eventTrigger = new Dictionary<int, EventTrigger>();
        private Dictionary<Image, int> _imageKeys = new Dictionary<Image, int>();
        private List<IGangMember> _allCombatants;
        private ItemVisualizer _itemVisualizer = new ItemVisualizer();
        private IGangMember _actualMember;
        private Button _nextRoundButton;
        private Button _fleeButton;
        private bool _isPlayersTurn;
        private ItemSlot _slotClicked;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _fightingPanel = GameObject.Find("FightPanelPrefab");

            var texts = _fightingPanel.GetComponentsInChildren<Text>();
            var images = _fightingPanel.GetComponentsInChildren<Image>();
            var eventT = _fightingPanel.GetComponentsInChildren<EventTrigger>();
            for (int i = 0; i < 10; i++)
            {
                _imageKeys.Add(images.First(img => img.gameObject.name == String.Concat("Next", (i + 1), "Image")), i);
                _nextNames.Add(i, texts.First(img => img.gameObject.name == String.Concat("NameNext", (i + 1), "Text")));
                _nextPictureText.Add(i, texts.First(txt => txt.gameObject.name == String.Concat("CharacterText", (i + 1))));
                _nextHPs.Add(i, texts.First(img => img.gameObject.name == String.Concat("LifeText", (i + 1))));
                _eventTrigger.Add(i, eventT.First(es => es.gameObject.name == String.Concat("Next", (i + 1), "Image")));
                _combatantsViaKey.Add(i, null);
            }

            _eventTrigger.Values.ToList().ForEach(et => HelperSingleton.Instance.AddEventTrigger(et, EnterImage, EventTriggerType.PointerEnter));
            _eventTrigger.Values.ToList().ForEach(et => HelperSingleton.Instance.AddEventTrigger(et, LeaveImage, EventTriggerType.PointerExit));

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

            while (true)
            {
                _actualMember = _allCombatants.Count > nextIndex ? _allCombatants.ElementAt(nextIndex) : _allCombatants.First();
                if (_actualMember.HealthStatus != HealthStatus.Dead)
                {
                    break;
                }
            }
            
            UpdateActionBar();
            UpdateButtons();
            UpdateItemSlots();

            if (!_isPlayersTurn)
            {
                StartCoroutine(HandleAttack(ItemSlot.MainWeapon, false));
            }
        }

        /// <summary>
        /// On Pointer Enter
        /// </summary>
        public void ClickImage(int itemId)
        {
            if (!_isPlayersTurn)
            {
                // We are not in charge! Go away!
                return;
            }

            StartCoroutine(HandleAttack(_slotClicked, true));
        }

        /// <summary>
        /// An action was clicked.
        /// </summary>
        /// <param name="slot"></param>
        public void ActionClicked(int slot)
        {
            var type = ConvertIdToSlot(slot);

            _slotClicked = ConvertIdToSlot(slot);
            _itemVisualizer.DeSelectAll();
            _itemVisualizer.SelectItem(_slotClicked);
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
            _allCombatants = _allCombatants.OrderBy(mem => mem.Initiative).ToList();
            _actualMember = _allCombatants.First();

            _isPlayersTurn = CharacterSingleton.Instance.PlayerMembersInCar.Contains(_actualMember);

            UpdateActionBar();
            UpdateButtons();
            UpdateItemSlots();

            SwitchFightingPanel(true);
        }

        /// <summary>
        /// First part of the player attack fight
        /// </summary>
        /// <returns></returns>
        private IEnumerator HandleAttack(ItemSlot slot, bool isPlayer)
        {
            IGangMember clickedGangMember = null;
            string textKey = String.Empty;
            if (isPlayer)
            {
                textKey = "FightAction" + slot.ToString();
                var clickedEnemey = GetNextImageViaRayCast();
                if (clickedEnemey != null)
                {
                    var key = _imageKeys[clickedEnemey.GetComponent<Image>()];
                    clickedGangMember = _combatantsViaKey[key];
                }
            }
            else
            {
                textKey = "FightActionAI" + slot.ToString();
            }

            // Replace to the name of the enemey, if needed
            var text = ReplaceVariables(ResourceSingleton.Instance.GetText(textKey), clickedGangMember);
            _combatLog.text = text + Environment.NewLine + _combatLog.text;

            yield return new WaitForSeconds(1f);

            if (isPlayer)
            {
                bool attackSuccessful = _actualMember.UsedItems[slot].ItemStragegy.ExecuteAction();
                var result = _actualMember.UsedItems[slot].ItemStragegy.GetOutpt(true);
                _combatLog.text = ReplaceVariables(result.Message, clickedGangMember) + Environment.NewLine + _combatLog.text;

                if (attackSuccessful)
                {
                    clickedGangMember.Health -= result.Value;
                }
            }
            else
            {
                bool attackSuccessful = _actualMember.UsedItems[ItemSlot.MainWeapon].ItemStragegy.ExecuteAction();
                var result = _actualMember.UsedItems[ItemSlot.MainWeapon].ItemStragegy.GetOutpt(false);
                _combatLog.text = ReplaceVariables(result.Message) + Environment.NewLine + _combatLog.text;

                _nextRoundButton.interactable = true;

                if (attackSuccessful)
                {
                    _actualMember.Health -= result.Value;
                }
            }

            IsFightOver();

            UpdateActionBar();
        }

        /// <summary>
        /// Check if the fight is over!
        /// </summary>
        private void IsFightOver()
        {
            if (_allCombatants.Where(comb => comb.GangAssignment == CharacterSingleton.Instance.GangOfPlayer).All(comb => comb.HealthStatus == HealthStatus.Dead))
            {
                // All Players gang members are dead! Player lost!
                Debug.Log("YOU LOST!");
            }
            else if (_allCombatants.Where(comb => comb.GangAssignment != CharacterSingleton.Instance.GangOfPlayer).All(comb => comb.HealthStatus == HealthStatus.Dead))
            {
                // All Enemey gang members are dead! Player won!
                Debug.Log("YOU WON!");
            }
        }

        /// <summary>
        /// Converts the ID to teh Slot Enum
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        private ItemSlot ConvertIdToSlot(int slot)
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

            return type;
        }

        /// <summary>
        /// Replace all known variables
        /// </summary>
        /// <param name="text">Text to be replaced</param>
        /// <param name="memberToUse">Member to use. If null, actual member in charge is used</param>
        /// <returns></returns>
        private string ReplaceVariables(string text, IGangMember memberToUse = null)
        {
            return text.Replace("@enemey", memberToUse != null 
                ? memberToUse.Name
                : _actualMember.Name);
        }

        /// <summary>
        /// On Pointer Enter
        /// </summary>
        private void EnterImage()
        {
            if (!_isPlayersTurn)
            {
                return;
            }

            var go = GetNextImageViaRayCast();
            if (go != null)
            {
                var key = _imageKeys[go.GetComponent<Image>()];
                if (_combatantsViaKey[key].HealthStatus == HealthStatus.Dead)
                {
                    // Dead already! Leave!
                    return;
                }
                _nextNames[key].fontStyle = FontStyle.Italic;
                _nextPictureText[key].fontStyle = FontStyle.Italic;
                _nextHPs[key].fontStyle = FontStyle.Italic;
            }
        }

        /// <summary>
        /// Gets the GameObject of the imaeg which was acutally clicked.
        /// </summary>
        /// <returns></returns>
        private GameObject GetNextImageViaRayCast()
        {
            PointerEventData pe = new PointerEventData(EventSystem.current);
            pe.position = Input.mousePosition;
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pe, hits);
            var hit = hits.FirstOrDefault(rayHit => rayHit.gameObject.name.StartsWith("Next") && rayHit.gameObject.name.EndsWith("Image"));
            return hit.gameObject;
        }

        /// <summary>
        /// Leave field
        /// </summary>
        private void LeaveImage()
        {
            _nextNames.Values.ToList().ForEach(tx => tx.fontStyle = FontStyle.Normal);
            _nextPictureText.Values.ToList().ForEach(tx => tx.fontStyle = FontStyle.Normal);
            _nextHPs.Values.ToList().ForEach(tx => tx.fontStyle = FontStyle.Normal);
        }

        /// <summary>
        /// Update the item slot
        /// </summary>
        private void UpdateItemSlots()
        {
            _itemVisualizer.ItemImages.ToList().ForEach(itm => itm.Value.gameObject.SetActive(_isPlayersTurn));
            if (_isPlayersTurn)
            {
                _itemVisualizer.VisualizeWeapons(_actualMember);
            }
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
                _combatantsViaKey[i] = actualMember;
                _nextNames[i].text = actualMember.Name;
                _nextPictureText[i].text = actualMember.HealthStatus == HealthStatus.Dead
                    ? String.Empty
                    : actualMember.GangAssignment.ToString();
                _nextHPs[i].text = String.Concat("HP: ", actualMember.Health.ToString(), "/", actualMember.MaxHealth.ToString(), Environment.NewLine, actualMember.HealthStatus.ToString());
                personIndex = _allCombatants.IndexOf(actualMember) + 1;

                _imageKeys.First(pr => pr.Value == i).Key.sprite = actualMember.HealthStatus == HealthStatus.Dead
                    ? PrefabSingleton.Instance.FightingSkullSprite
                    : PrefabSingleton.Instance.FightingRegularSprite;

            }
        }
    }
}

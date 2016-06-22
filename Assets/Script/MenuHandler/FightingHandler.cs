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
        private Text _actionPoints;
        private Dictionary<Image, int> _imageKeys = new Dictionary<Image, int>();
        private List<IGangMember> _allCombatants;
        private ItemVisualizer _itemVisualizer = new ItemVisualizer();
        private IGangMember _actualMember;
        private Button _nextRoundButton;
        private Button _fleeButton;
        private Text _fleeText;
        private bool _isPlayersTurn;
        private ItemSlot _slotClicked;
        private Delegate _callBack;

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

            // Get Special text which are only preent for the actual character
            _actionPoints = texts.First(img => img.gameObject.name == "ActionPointsText");

            _eventTrigger.Values.ToList().ForEach(et => HelperSingleton.Instance.AddEventTrigger(et, EnterImage, EventTriggerType.PointerEnter));
            _eventTrigger.Values.ToList().ForEach(et => HelperSingleton.Instance.AddEventTrigger(et, LeaveImage, EventTriggerType.PointerExit));

            _combatLog = texts.First(tx => tx.name == "CombatLogText");
            _fleeText = texts.First(tx => tx.name == "FleeText");

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
            else
            {
                // Reset stuff which may have changed.
                _fleeText.text = "Flee";
            }
        }

        /// <summary>
        /// Switches teh round
        /// </summary>
        public void NextRound()
        {
            // Reset Action Points
            _actualMember.ActionPoints = _actualMember.MaxActionPoints;

            var nextIndex = _allCombatants.IndexOf(_actualMember) + 1;

            int counter = 0;
            while (true)
            {
                counter++;
                _actualMember = _allCombatants.Count > nextIndex ? _allCombatants.ElementAt(nextIndex) : _allCombatants.First();
                if (_actualMember.HealthStatus != HealthStatus.Dead && _actualMember.HealthStatus != HealthStatus.Unconscious)
                {
                    break;
                }
                else
                {
                    nextIndex = _allCombatants.Count > (nextIndex + 1) ? nextIndex + 1 : 0;
                }

                if (counter > 100)
                {
                    Debug.LogError("Exited after 100 loop!");
                    break;
                }
            }
            
            UpdateActionBar();
            UpdateButtons();
            UpdateItemSlots();

            _nextRoundButton.interactable = _isPlayersTurn;

            var logTextPlayer = String.Empty;
            var logTextEnemey = String.Empty;

            ResourceSingleton.Instance.GetText("FightNextRoundYou", out logTextPlayer);
            ResourceSingleton.Instance.GetText("FightNextRoundEnemey", out logTextEnemey);

            _combatLog.text = String.Concat(_isPlayersTurn ? logTextPlayer : logTextEnemey, Environment.NewLine, _combatLog.text);

            if (!_isPlayersTurn)
            {
                StartCoroutine(HandleAIAttack());
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

            if (_slotClicked != ItemSlot.NotSet)
            {
                StartCoroutine(HandleAttack(_slotClicked, true));
            }
        }

        /// <summary>
        /// An action was clicked.
        /// </summary>
        /// <param name="slot"></param>
        public void ActionClicked(int slot)
        {
            if (!_isPlayersTurn)
            {
                // We are not in charge!
                return;
            }

            _slotClicked = ConvertIdToSlot(slot);

            if ((_actualMember.UsedItems[_slotClicked] as Weapon).ActionCosts > _actualMember.ActionPoints)
            {
                var text = String.Empty;
                ResourceSingleton.Instance.GetText("FightNotEnoughPoints", out text);
                _combatLog.text = String.Concat(text, Environment.NewLine, _combatLog.text);
                _itemVisualizer.DeSelectAll();
                _slotClicked = ItemSlot.NotSet;
                return;
            }

            _itemVisualizer.DeSelectAll();
            _itemVisualizer.SelectItem(_slotClicked);
        }

        /// <summary>
        /// Flee form Battle
        /// </summary>
        public void FleeFromBattle()
        {
            if (_fleeText.text == "Flee")
            {
                PrefabSingleton.Instance.PlayersCarScript.SetCarBackOnTheRoadWithTurn();
            }
            else
            {
                // TODO: Alle tot - was nun?
            }
            
            SwitchFightingPanel(false);
        }

        /// <summary>
        /// Starts a fight
        /// </summary>
        /// <param name="opponents">All opponents</param>
        /// <param name="callBack">Action which is fired after the fight is over.</param>
        public void StartFight(List<IGangMember> opponents, Delegate callBack)
        {
            _allCombatants = opponents.ToList();
            _allCombatants.AddRange(CharacterSingleton.Instance.PlayerMembersInCar);
            _allCombatants = _allCombatants.OrderBy(mem => mem.Initiative).ToList();
            _actualMember = _allCombatants.First();

            _isPlayersTurn = CharacterSingleton.Instance.PlayerMembersInCar.Contains(_actualMember);
            _callBack = callBack;

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
            string nextCombatLogEntry = String.Empty;
            IGangMember playerToBeAttacked = null;
            if (isPlayer)
            {
                textKey = "FightAction" + slot.ToString();
                var enemeyToBeAttacked = GetNextImageViaRayCast();
                if (enemeyToBeAttacked != null)
                {
                    var key = _imageKeys[enemeyToBeAttacked.GetComponent<Image>()];
                    clickedGangMember = _combatantsViaKey[key];
                }

                if (clickedGangMember.HealthStatus == HealthStatus.Dead)
                {
                    // Bereits tot? Kann nicht mehr drauf ballern!
                    var alreadyDead = String.Empty;
                    ResourceSingleton.Instance.GetText("FightAlreadyDead", out alreadyDead);
                    nextCombatLogEntry = ReplaceVariables(alreadyDead, _actualMember, clickedGangMember);
                    _combatLog.text = nextCombatLogEntry + Environment.NewLine + _combatLog.text;
                    _slotClicked = ItemSlot.NotSet;
                    _itemVisualizer.DeSelectAll();
                    yield break;
                }
                else if (clickedGangMember.HealthStatus == HealthStatus.Unconscious)
                {
                    PrefabSingleton.Instance.InputHandler.AddQuestion("FightQuestUnconscious");
                    yield return StartCoroutine(PrefabSingleton.Instance.InputHandler.WaitForAnswer());

                    if (PrefabSingleton.Instance.InputHandler.AnswerGiven == 2)
                    {
                        // User decided to stop fire.
                        yield break;
                    }
                }

                var text = String.Empty;
                ResourceSingleton.Instance.GetText(textKey, out text);
                nextCombatLogEntry = ReplaceVariables(text, _actualMember, clickedGangMember);               
            }
            else
            {
                // Evalute who is to be attached!
                playerToBeAttacked = _allCombatants.Where(pay => pay.GangAssignment == CharacterSingleton.Instance.GangOfPlayer
                    && pay.HealthStatus != HealthStatus.Dead)
                    .OrderBy(pay => pay.Health).FirstOrDefault();

                textKey = "FightActionAI" + slot.ToString();
                var text = String.Empty;
                ResourceSingleton.Instance.GetText(textKey, out text);
                nextCombatLogEntry = ReplaceVariables(text, playerToBeAttacked, _actualMember);
            }

            // Add to combat log
            _combatLog.text = nextCombatLogEntry + Environment.NewLine + _combatLog.text;

            yield return new WaitForSeconds(1f);

            if (isPlayer)
            {
                Weapon weapon = _actualMember.UsedItems[slot] as Weapon;
                bool attackSuccessful = weapon.ItemStragegy.ExecuteAction();
                var result = weapon.ItemStragegy.GetAttackOutpt(true);
                _combatLog.text = ReplaceVariables(result.Message, _actualMember, clickedGangMember) + Environment.NewLine + _combatLog.text;

                if (attackSuccessful)
                {
                    clickedGangMember.Health -= result.Value;
                }
                _actualMember.ActionPoints -= weapon.ActionCosts;
            }
            else
            {
                Weapon weapon = _actualMember.UsedItems[ItemSlot.MainWeapon] as Weapon;
                if (playerToBeAttacked == null)
                {
                    Debug.LogWarning("Nobody to attack");
                    yield break;
                }

                bool attackSuccessful = weapon.ItemStragegy.ExecuteAction();
                var result = weapon.ItemStragegy.GetAttackOutpt(false);
                _combatLog.text = ReplaceVariables(result.Message, playerToBeAttacked, _actualMember) + Environment.NewLine + _combatLog.text;

                if (attackSuccessful)
                {
                    playerToBeAttacked.Health -= result.Value;
                }

                _nextRoundButton.interactable = true;
                _actualMember.ActionPoints -= weapon.ActionCosts;
            }

            _slotClicked = ItemSlot.NotSet;
            _itemVisualizer.DeSelectAll();

            StartCoroutine(IsFightOver());

            UpdateActionBar();
        }

        /// <summary>
        /// Handle AI Attack
        /// </summary>
        /// <returns></returns>
        private IEnumerator HandleAIAttack()
        {
            var actionPoints = ((Weapon)_actualMember.UsedItems[ItemSlot.MainWeapon]).ActionCosts;
            for (int i = 0; i < 25; i++)
            {
                _actualMember.ActionPoints -= actionPoints;
                if (_actualMember.ActionPoints > 0)
                {
                    StartCoroutine(HandleAttack(ItemSlot.MainWeapon, false));
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    yield break;
                }
            }

            Debug.LogWarning("25 Loops done!");
        }

        /// <summary>
        /// Check if the fight is over!
        /// </summary>
        private IEnumerator IsFightOver()
        {
            if (_allCombatants.Where(comb => comb.GangAssignment == CharacterSingleton.Instance.GangOfPlayer).All(comb => comb.HealthStatus == HealthStatus.Dead))
            {   
                // Player lost!
                // All members are dead!
                CharacterSingleton.Instance.PlayerMembersInCar.Clear();
                _nextRoundButton.gameObject.SetActive(false);
                _fleeText.text = "Close";
                var text = String.Empty;
                ResourceSingleton.Instance.GetText("FightYouLost", out text);
                _combatLog.text = String.Concat(text, Environment.NewLine, _combatLog.text);
                
                _callBack.DynamicInvoke(false);
            }
            else if (_allCombatants.Where(comb => comb.GangAssignment != CharacterSingleton.Instance.GangOfPlayer).All(comb => comb.HealthStatus == HealthStatus.Dead))
            {
                // All Enemey gang members are dead! Player won!
                var text = String.Empty;
                ResourceSingleton.Instance.GetText("FightYouWon", out text);
                _combatLog.text = String.Concat(text, Environment.NewLine, _combatLog.text);

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("bodies",
                    _allCombatants.Where(mem => mem.GangAssignment != CharacterSingleton.Instance.GangOfPlayer && mem.HealthStatus == HealthStatus.Dead).Count().ToString());
                parameters.Add("space", (6 - CharacterSingleton.Instance.PlayerMembersInCar.Count()).ToString());
                PrefabSingleton.Instance.InputHandler.AddQuestion("FightQuestTakeBodies", parameters);
                yield return StartCoroutine(PrefabSingleton.Instance.InputHandler.WaitForAnswer());

                if (PrefabSingleton.Instance.InputHandler.AnswerGiven == 1)
                {
                    Debug.Log("Leichen mitnehmen");
                }
                else
                {
                    Debug.Log("Leichen da lassen");
                }

                SwitchFightingPanel(false);
                PrefabSingleton.Instance.PlayersCarScript.SetCarBackOnTheRoadWithTurn();

                _callBack.DynamicInvoke(true);
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
        /// <param name="personWhoAttacks">Person who is attacking right now.</param>
        /// <param name="personWhoIsAttacked">Person who is attacked right now.</param>
        /// <returns></returns>
        private string ReplaceVariables(string text, IGangMember personWhoAttacks, IGangMember personWhoIsAttacked)
        {
            var result = text.Replace("@enemey", personWhoIsAttacked.Name);
            result = result.Replace("@player", personWhoAttacks.Name);

            return result;
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

                if (key == 0)
                {
                    _actionPoints.fontStyle = FontStyle.Italic;
                }
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
            _actionPoints.fontStyle = FontStyle.Normal;
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
            _itemVisualizer.VisualizeWeapons(_actualMember);
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

            // Set Special text fields
            _actionPoints.text = String.Concat("AP: ", actualMember.ActionPoints.ToString(), "/", actualMember.MaxActionPoints.ToString());

            for (int i = 0; i < 10; i++)
            {
                actualMember = _allCombatants.Count > personIndex ? _allCombatants.ElementAt(personIndex) : _allCombatants.First();
                _combatantsViaKey[i] = actualMember;
                _nextNames[i].text = actualMember.Name;
                _nextPictureText[i].text = (actualMember.HealthStatus == HealthStatus.Dead || actualMember.GangAssignment == Gangs.NotSet)
                    ? String.Empty
                    : actualMember.GangAssignment.ToString();
                _nextHPs[i].text = String.Concat("HP: ", actualMember.Health.ToString(), "/", actualMember.MaxHealth.ToString(), " ", actualMember.HealthStatus.ToString());
                personIndex = _allCombatants.IndexOf(actualMember) + 1;

                _imageKeys.First(pr => pr.Value == i).Key.sprite = actualMember.HealthStatus == HealthStatus.Dead
                    ? ResourceSingleton.Instance.FightingSkullSprite
                    : ResourceSingleton.Instance.FightingRegularSprite;
            }
        }
    }
}

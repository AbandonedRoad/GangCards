using System;
using System.Linq;
using Enum;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Singleton;
using Interfaces;

namespace Menu
{
    public class FightingHandler : MonoBehaviour
    {
        private GameObject _fightingPanel;
        private List<Button> _actionButtons;
        private List<Text> _actionButtonsText;
        private Text _combatLog;
        private Dictionary<int, Image> _nextPictures;
        private Dictionary<int, Text> _nextNames;

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

            SwitchFightingPanel(false);
        }

        /// <summary>
        /// Updates.
        /// </summary>
        void Update()
        {

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
        /// Starts a fight!
        /// </summary>
        public void StartFight(List<IGangMember> opponents)
        {
            for (int i = 0; i < 5; i++)
            {
                var actualMember = CharacterSingleton.Instance.PlayerMembersInCar.Count > i ? CharacterSingleton.Instance.PlayerMembersInCar.ElementAt(i) : null;
                if (actualMember == null)
                {
                    break;
                }
                _nextNames[i].text = actualMember.Name;
                _nextPictures[i].sprite = ResourceSingleton.Instance.ImageFaces[actualMember.ImageName];
            }   
        }
    }
}

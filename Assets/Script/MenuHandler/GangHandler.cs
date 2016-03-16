using Assets.Script.Characters;
using Singleton;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class GangHandler : MonoBehaviour
    {
        public GameObject Panel
        {
            get { return _gangPanel; }
        }

        private GangMember _actualMember;
        private GameObject _gangPanel;
        private Button _previousButton;
        private Button _nextButton;
        private Button _previousStreetNameButton;
        private Button _nextStreetNameButton;
        private Text _name;
        private Text _streeName;
        private Text _intelligence;
        private Text _strength;
        private Text _level;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _gangPanel = GameObject.Find("GangPanel");

            var texts = _gangPanel.GetComponentsInChildren<Text>();
            _name = texts.First(tx => tx.gameObject.name == "NameText");
            _streeName = texts.First(tx => tx.gameObject.name == "StreetNameText");
            _intelligence = texts.First(tx => tx.gameObject.name == "IntelligenceText");
            _strength = texts.First(tx => tx.gameObject.name == "StrengthText");
            _level = texts.First(tx => tx.gameObject.name == "LevelText");

            var buttons = _gangPanel.GetComponentsInChildren<Button>();
            _nextButton = buttons.First(tx => tx.gameObject.name == "NextButton");
            _previousButton = buttons.First(tx => tx.gameObject.name == "PreviousButton");
            _nextStreetNameButton = buttons.First(tx => tx.gameObject.name == "NextStreetNameButton");
            _previousStreetNameButton = buttons.First(tx => tx.gameObject.name == "PreviousStreetNameButton");

            _nextButton.onClick.AddListener(() => NextGangster());
            _previousButton.onClick.AddListener(() => PreviousGangster());

            _nextStreetNameButton.onClick.AddListener(() => NextGangster());
            _previousStreetNameButton.onClick.AddListener(() => PreviousGangster());

            FillLabels(null);

            SwitchGangPanel();
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchGangPanel()
        {
            _gangPanel.SetActive(!_gangPanel.active);
            if (_gangPanel.activeSelf)
            {
                _gangPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        private void PreviousGangster()
        {
            if (_actualMember == null)
            {
                _actualMember = CharacterSingleton.Instance.AiPlayers.FirstOrDefault();
            }
            else
            {
                var index = CharacterSingleton.Instance.AiPlayers.IndexOf(_actualMember);
                if (index == 0)
                {
                    _actualMember = CharacterSingleton.Instance.AiPlayers.LastOrDefault();
                }
                else
                {
                    index--;
                    _actualMember = CharacterSingleton.Instance.AiPlayers.ElementAt(index);
                }
            }
            FillLabels(_actualMember);
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        private void PreviousStreeName()
        {
            if (_actualMember == null)
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

            FillLabels(_actualMember);
        }

        /// <summary>
        /// Gets the previous gangster
        /// </summary>
        private void NextStreeName()
        {
            if (_actualMember == null)
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

            FillLabels(_actualMember);
        }

        /// <summary>
        /// Get next gangster
        /// </summary>
        private void NextGangster()
        {
            if (_actualMember == null)
            {
                _actualMember = CharacterSingleton.Instance.AiPlayers.FirstOrDefault();
            }
            else
            {
                var index = CharacterSingleton.Instance.AiPlayers.IndexOf(_actualMember);
                if (index == CharacterSingleton.Instance.AiPlayers.Count -1)
                {
                    _actualMember = CharacterSingleton.Instance.AiPlayers.FirstOrDefault();
                }
                else
                {
                    index++;
                    _actualMember = CharacterSingleton.Instance.AiPlayers.ElementAt(index);
                }
            }
            FillLabels(_actualMember);
        }

        /// <summary>
        /// Fills the labels.
        /// </summary>
        /// <param name="member"></param>
        private void FillLabels(GangMember member)
        {
            _name.text = member != null ? member.Name : String.Empty;
            _streeName.text = member != null ? member.ActiveStreeName : String.Empty;
            _intelligence.text = member != null ? member.Intelligence.ToString() : String.Empty;
            _strength.text = member != null ? member.Strength.ToString() : String.Empty;
            _level.text = member != null ? member.Level.ToString() : String.Empty;
        }
    }
}
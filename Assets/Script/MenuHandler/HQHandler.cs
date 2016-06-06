using Humans;
using Interfaces;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class HQHandler : MonoBehaviour
    {
        private GameObject _panel;
        private Text _enterExitButtonText;
        private List<IGangMember> _membersInHQ;
        private List<IGangMember> _membersInCar;
        private GangVisualizer _visualizer = new GangVisualizer();
        private Text _amountHQText;
        private Text _amountCarText;

        public int AnswerGiven { get; private set; }

        /// <summary>
        /// Starts form
        /// </summary>
        void Start()
        {
            _panel = GameObject.Find("HQPanelPrefab");

            var buttons = _panel.GetComponentsInChildren<Button>();
            var images = _panel.GetComponentsInChildren<Image>();
            var texts = _panel.GetComponentsInChildren<Text>();

            _amountHQText = texts.FirstOrDefault(txt => txt.gameObject.name == "GangMembersInHQAmountText");
            _amountCarText = texts.FirstOrDefault(txt => txt.gameObject.name == "GangMembersInCarAmountText");
            _enterExitButtonText = texts.First(txt => txt.name == "EnterExitCarText");

            _visualizer.AddDetailledMember("HQD", images.First(itm => itm.gameObject.name == "HQDetailPrefab"));

            // Add all for the car for HQ.
            for (int i = 1; i < 7; i++)
            {
                var key = String.Concat("HQ", i.ToString());
                var name = String.Concat(key, "Prefab");
                var actHQ = images.First(itm => itm.gameObject.name == name);
                _visualizer.AddShortMember(key, actHQ);
            }

            // Add all for the car for car.
            for (int i = 1; i < 5; i++)
            {
                var key = String.Concat("Car", i.ToString());
                var name = String.Concat(key, "Prefab");
                var actCar = images.First(itm => itm.gameObject.name == name);
                _visualizer.AddDetailledMember(key, actCar);
            }

            SwitchPanel();
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchPanel()
        {
            _panel.SetActive(!_panel.activeSelf);
            if (_panel.activeSelf)
            {
                _panel.transform.SetAsLastSibling();
                Refresh();
            }
        }

        /// <summary>
        /// Loops list backwards
        /// </summary>
        public void PrevoiusGangMembers()
        {
            var prepList = _membersInHQ.Where(el => _membersInHQ.IndexOf(el) < _membersInHQ.Count - 1).ToList();
            var lastEntry = _membersInHQ.ElementAt(_membersInHQ.Count - 1);
            _membersInHQ = new List<IGangMember>();
            _membersInHQ.Add(lastEntry);
            _membersInHQ.AddRange(prepList);
        }

        /// <summary>
        /// Loops list forward
        /// </summary>
        public void NextGangMembers()
        {
            var prepList = _membersInHQ.Where(el => _membersInHQ.IndexOf(el) > 0).ToList();
            var firstEntry = _membersInHQ.ElementAt(0);
            _membersInHQ = prepList;
            _membersInHQ.Add(firstEntry);

            PaintLists();
        }

        /// <summary>
        /// A new member is sleected.
        /// </summary>
        /// <param name="key"></param>
        public void SelectMember(string key)
        {
            var member = _visualizer.SelectEntry(key);

            if (member == null)
            {
                return;
            }

            _enterExitButtonText.text = CharacterSingleton.Instance.PlayerMembersInCar.Contains(member)
                ? "Exit Car"
                : "Enter Car";
        }

        /// <summary>
        /// Enters or exits the car.
        /// </summary>
        public void EnterExitCar()
        {
            var gangMember = _visualizer.GetSelectedItem();
            if (gangMember == null)
            {
                // Nothing is selected
                return;
            }

            if (!CharacterSingleton.Instance.PlayerMembersInCar.Contains(gangMember))
            {
                // Not in the car! Enter it damn it!
                CharacterSingleton.Instance.PlayerMembersInCar.Add(gangMember);
            }
            else
            {
                // In the car! Leave it damn it!
                CharacterSingleton.Instance.PlayerMembersInCar.Remove(gangMember);
            }

            Refresh();

            if (_visualizer.GetSelectedItem() == null)
            {
                // Nothing is selected
                _enterExitButtonText.text = "<Select member>";
            }
        }

        /// <summary>
        /// HQ will be left.
        /// </summary>
        public void LeaveHQ()
        {
            SwitchPanel();
            PrefabSingleton.Instance.PlayersCarScript.SetCarBackOnTheRoadWithTurn();
        }

        /// <summary>
        /// Refrehes GUI.
        /// </summary>
        private void Refresh()
        {
            _membersInCar = CharacterSingleton.Instance.PlayerMembersInCar.ToList();
            _membersInHQ = CharacterSingleton.Instance.PlayersGang.Except(_membersInCar).ToList();

            PaintLists();
        }

        /// <summary>
        /// Paint the Panel
        /// </summary>
        private void PaintLists()
        {
            var total = CharacterSingleton.Instance.PlayersGang.Count;
            var car = CharacterSingleton.Instance.PlayerMembersInCar.Count;
            var hq = CharacterSingleton.Instance.PlayersGang.Count - CharacterSingleton.Instance.PlayerMembersInCar.Count;

            _amountHQText.text = String.Concat(hq.ToString(), "/", total.ToString());
            _amountCarText.text = String.Concat(car.ToString(), "/4");

            // Paint HQ
            _visualizer.Init();
            _visualizer.HideShowItems("HQD", _membersInHQ.Any());
            if (_membersInHQ.Any())
            {
                _visualizer.VisualizeMember("HQD", _membersInHQ.ElementAt(0));
            }

            for (int i = 1; i < 7; i++)
            {
                var key = String.Concat("HQ", i.ToString());

                _visualizer.HideShowItems(key, _membersInHQ.Count > i);
                if (_membersInHQ.Count > i)
                {
                    _visualizer.VisualizeMember(key, _membersInHQ.ElementAt(i));
                }
            }

            // Paint Car
            for (int i = 0; i < 4; i++)
            {
                var key = String.Concat("Car", (i + 1).ToString());
                _visualizer.HideShowItems(key, _membersInCar.Count > i);
                if (_membersInCar.Count > i)
                {
                    _visualizer.VisualizeMember(key, _membersInCar.ElementAt(i));
                }
            }
        }
    }
}
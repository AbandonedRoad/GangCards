using Singleton;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.MenuHandler
{
    public class TopBarHandler : MonoBehaviour
    {
        public GameObject Panel
        {
            get { return _headerPanel; }
        }

        private GameObject _headerPanel;
        private Text _gangAmountText;
        private Text _cashText;
        private Text _timeOfDay;
        private DayNightController _dayNightController;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Awake()
        {
            _headerPanel = GameObject.Find("HeaderBarPrefab");

            var dayNight = GameObject.Find("DayNightCycle_Sun");
            _dayNightController = dayNight.GetComponent<DayNightController>();

            var texts = _headerPanel.GetComponentsInChildren<Text>();
            _gangAmountText = texts.FirstOrDefault(tx => tx.name == "GangsterAmountText");
            _cashText = texts.FirstOrDefault(tx => tx.name == "MoneyAmountText");
            _timeOfDay = texts.FirstOrDefault(tx => tx.name == "TimeOfDayText");
        }

        /// <summary>
        /// Updates the header
        /// </summary>
        void Update()
        {
            _gangAmountText.text = CharacterSingleton.Instance.PlayersGang.Count.ToString();
            _cashText.text = String.Concat(CharacterSingleton.Instance.AvailableMoney.ToString(), "$");
            _timeOfDay.text = _dayNightController.timeString;
        }
    }
}

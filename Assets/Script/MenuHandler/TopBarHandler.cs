using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private ActionContainer _container;
        private GameObject _headerPanel;
        private Text _gangAmountText;
        private Text _cashText;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _headerPanel = GameObject.Find("HeaderBarPrefab");

            var texts = _headerPanel.GetComponentsInChildren<Text>();
            _gangAmountText = texts.FirstOrDefault(tx => tx.name == "GangsterAmountText");
            _cashText = texts.FirstOrDefault(tx => tx.name == "MoneyAmountText");
        }

        /// <summary>
        /// Updates the header
        /// </summary>
        void Update()
        {
            _gangAmountText.text = CharacterSingleton.Instance.PlayersGang.Count.ToString();
            _cashText.text = "1500$";
        }
    }
}

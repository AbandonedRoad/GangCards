using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Assets.Script.Actions;

namespace Menu
{
    public class DebugHandler : MonoBehaviour
    {
        public GameObject DebugPanel { get; private set; }
        public bool CreateCrossings { get { return _createCrossings.isOn; } }

        private Button _addGangMembers;
        private Toggle _createCrossings;

        void Awake()
        {
            DebugPanel = GameObject.Find("DebugPanelPrefab");

            _addGangMembers = DebugPanel.GetComponentsInChildren<Button>().First(btn => btn.name == "AddGangMembersButton");

            _addGangMembers.onClick.AddListener(() => AddGangMembers());

            SwitchDebugPanel();
        }

        private void AddGangMembers()
        {
            var hire = new HireGangMembers("1", "1", "1");
            hire.AddProspectToGang();
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchDebugPanel()
        {
            DebugPanel.SetActive(!DebugPanel.activeSelf);
            if (DebugPanel.activeSelf)
            {
                DebugPanel.transform.SetAsLastSibling();
            }
        }
    }
}

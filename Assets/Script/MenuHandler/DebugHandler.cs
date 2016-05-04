﻿using Singleton;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Actions;
using Enum;

namespace Menu
{
    public class DebugHandler : MonoBehaviour
    {
        public GameObject DebugPanel { get; private set; }

        private Button _addGangMembers;

        void Awake()
        {
            DebugPanel = GameObject.Find("DebugPanelPrefab");

            _addGangMembers = DebugPanel.GetComponentsInChildren<Button>().First(btn => btn.name == "AddGangMembersButton");

            _addGangMembers.onClick.AddListener(() => AddGangMembers(1));

            SwitchDebugPanel();
        }

        /// <summary>
        /// Adds members to the gang
        /// </summary>
        public void AddGangMembers(int amount)
        {
            var hire = new HireGangMembers("1", "1", "1");

            for (int i = 0; i < amount; i++)
            {
                var member = hire.AddProspectToGang();
                member.PostProcessInit(Gangs.Wheelers);
                member.UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(1277109520, member.Level, member);
            }            
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
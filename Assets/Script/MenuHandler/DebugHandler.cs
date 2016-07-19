using Singleton;
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
        private Button _addMoneyButton;

        void Awake()
        {
            DebugPanel = GameObject.Find("DebugPanelPrefab");

            var buttons = DebugPanel.GetComponentsInChildren<Button>();
            _addGangMembers = buttons.First(btn => btn.name == "AddGangMembersButton");
            _addMoneyButton = buttons.First(btn => btn.name == "AddMoneyButton");

            _addGangMembers.onClick.AddListener(() => AddGangMembers(1));
            _addMoneyButton.onClick.AddListener(() => AddMoney(1000));

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
                member.PostProcessInit(Gangs.WheelersOfDecay);
                member.UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(WeaponType.Rifle, member.Level, member);
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

        /// <summary>
        /// Adds members to the gang
        /// </summary>
        private void AddMoney(int amount)
        {
            CharacterSingleton.Instance.AvailableMoney += amount;
        }
    }
}
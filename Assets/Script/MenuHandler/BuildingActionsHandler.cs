using Assets.Script;
using Assets.Script.Actions;
using Enum;
using Interfaces;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class BuildingActionsHandler : MonoBehaviour
    {
        public GameObject Panel
        {
            get { return _buildingActionsPanel; }
        }

        private ActionContainer _container;
        private GameObject _buildingActionsPanel;
        private List<Text> _actionTexts;
        private List<Button> _actionButtons;
        private List<Text> _actionButtonsText;
        private Button _leaveButton;
        private Text _entryText;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _buildingActionsPanel = GameObject.Find("BuildingActionsPanelPrefab");

            var texts = _buildingActionsPanel.GetComponentsInChildren<Text>().ToList();
            _entryText = texts.First(btn => btn.name == "EntryText");
            _leaveButton = _buildingActionsPanel.GetComponentsInChildren<Button>().First(btn => btn.name == "LeaveButton");

            ActionHelper.PrepareInstances(_buildingActionsPanel, ref _actionTexts, ref _actionButtons, ref _actionButtonsText, 4);

            _actionButtons.ForEach(btn => btn.onClick.AddListener(() => ActionHelper.ExecuteAction(btn.name, _container.CreatedActions)));
            _leaveButton.onClick.AddListener(() => Leave());

            GUIHelper.ReplaceText(texts);

            SwitchBuildingActionsPanel(false);
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchBuildingActionsPanel(bool show)
        {
            _buildingActionsPanel.SetActive(show);
            if (_buildingActionsPanel.activeSelf)
            {
                _buildingActionsPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// Passes new actions into this container
        /// </summary>
        public void PassActions(ActionContainer container)
        {
            _container = container;
            if (_container.CreatedActions == null)
            {
                _container.CreatedActions = new IAction[4];
            }

            ActionHelper.PrepareActions(container, _actionTexts, _actionButtons, _actionButtonsText);

            _entryText.text = ResourceSingleton.Instance.CreateActionText(container.TextRessourcePrefix, "Entry");
        }

        /// <summary>
        /// Leaves actual screen.
        /// </summary>
        private void Leave()
        {
            SwitchBuildingActionsPanel(false);
            PrefabSingleton.Instance.PlayersCarScript.SetCarBackOnTheRoad();
        }
    }
}
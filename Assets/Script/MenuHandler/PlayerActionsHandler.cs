using Assets.Script.Actions;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class PlayerActionsHandler : MonoBehaviour
    {
        public GameObject Panel
        {
            get { return _actionsPanel; }
        }

        private ActionContainer _container;
        private GameObject _actionsPanel;
        private List<Button> _actionButtons;
        private List<Text> _actionTexts;
        private List<Text> _actionButtonsText;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _actionsPanel = GameObject.Find("PlayerOptionsPanel");

            ActionHelper.PrepareInstances(_actionsPanel, ref _actionTexts, ref _actionButtons, ref _actionButtonsText, 3);

            _actionButtons.ForEach(btn => btn.onClick.AddListener(() => ExecuteAction(btn.name)));

            SwitchPlayerActionPanel(false);
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchPlayerActionPanel(bool show)
        {
            _actionsPanel.SetActive(show);
            if (_actionsPanel.activeSelf)
            {
                _actionsPanel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// Passes new actions into this container
        /// </summary>
        /// <param name="container"></param>
        public void PassActions(ActionContainer container)
        {
            _container = container;
            if (container.CreatedActions == null)
            {
                _container.CreatedActions = new IAction[3];
            }
            ActionHelper.PrepareActions(container, _actionTexts, _actionButtons, _actionButtonsText);

            SwitchPlayerActionPanel(true);
        }

        /// <summary>
        /// Execute button action
        /// </summary>
        /// <param name="actionButton"></param>
        private void ExecuteAction(string actionButton)
        {
            ActionHelper.ExecuteAction(actionButton, _container.CreatedActions);

            SwitchPlayerActionPanel(false);
        }
    }
}
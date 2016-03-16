using System;
using System.Linq;
using Enum;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Actions;
using System.Collections.Generic;
using Singleton;
using System.Reflection;
using Actions;

namespace Menu
{
    public class ActionAfterEffactsHandler : MonoBehaviour
    {
        public GameObject Panel
        {
            get { return _actionsAfterEffects; }
        }

        private GameObject _actionsAfterEffects;
        private ActionContainerMethod _actionContainer;
        private MethodInfo[] _methodCalls;
        private List<Button> _actionButtons;
        private List<Text> _actionButtonsText;
        private ITextPresenter _textPresenter;
        private Text _textPart1;
        private Text _textPart2;

        /// <summary>
        /// Start this instance.
        /// </summary>
        void Start()
        {
            _actionsAfterEffects = GameObject.Find("ActionAfterEffactsPrefab");

            var texts = _actionsAfterEffects.GetComponentsInChildren<Text>();
            _textPart1 = texts.First(txt => txt.gameObject.name == "TextPart1Text");
            _textPart2 = texts.First(txt => txt.gameObject.name == "TextPart2Text");

            ActionHelper.PrepareInstances(_actionsAfterEffects, ref _actionButtons, ref _actionButtonsText, 3);
            ActionHelper.SetActiveStatus(null, _actionButtons, _actionButtonsText, false);

            _actionButtons.ForEach(btn => btn.onClick.AddListener(() => ActionHelper.ExecuteMethod(btn.name, _actionContainer.Instance, _methodCalls)));

            SwitchAfterEffectsPanel(false);
        }

        /// <summary>
        /// Updates.
        /// </summary>
        void Update()
        {
            if (_textPresenter != null)
            {
                // S how text
                _textPart1.text = _textPresenter.TextPart1;
                _textPart2.text = _textPresenter.TextPart2;
            }

            if (_actionContainer != null)
            {
                // Update buttons, if needed.
                for (int i = 0; i < _actionContainer.MethodButtonsActive.Length; i++)
                {
                    _actionButtons[i].gameObject.SetActive(_actionContainer.MethodButtonsActive[i]);
                }
            }
        }

        /// <summary>
        /// Passes new actions into this container
        /// </summary>
        public void PassActions(ActionContainerMethod container)
        {
            _actionContainer = container;
            _textPresenter = _actionContainer.Instance as ITextPresenter;
            _methodCalls = new MethodInfo[container.MethodCalls.Length];
            ActionHelper.PrepareActions(container, _methodCalls, _actionButtons, _actionButtonsText);

            _textPart1.text = ResourceSingleton.Instance.CreateActionText(container.TextRessourcePrefix, "Entry");

            SwitchAfterEffectsPanel(true);
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchAfterEffectsPanel(bool newVisibility)
        {
            _actionsAfterEffects.SetActive(newVisibility);
            if (_actionsAfterEffects.activeSelf)
            {
                _actionsAfterEffects.transform.SetAsLastSibling();
            }
        }
    }
}
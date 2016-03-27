using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class DebugHandler : MonoBehaviour
    {
        public GameObject DebugPanel { get; private set; }
        public bool CreateCrossings { get { return _createCrossings.isOn; } }

        private Button _duplicateBlockButton;
        private Toggle _createCrossings;

        void Awake()
        {
            DebugPanel = GameObject.Find("DebugPanel");

            _duplicateBlockButton = DebugPanel.GetComponentsInChildren<Button>().First(btn => btn.name == "DuplicateButton");
            _createCrossings = DebugPanel.GetComponentsInChildren<Toggle>().First(btn => btn.name == "ReplaceCrossings");

            //_duplicateBlockButton.onClick.AddListener(() => StartCoroutine(PrefabSingleton.Instance.LevelGeneratorAftermath.DuplicateHandling(false)));

            SwitchDebugPanel();
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

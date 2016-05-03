using Enum;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class ChooseYourGangHandler : MonoBehaviour
    {
        private GameObject _panel;
        private Dictionary<GameObject, KeyValuePair<Text, Text>> _assignment = new Dictionary<GameObject, KeyValuePair<Text, Text>>();

        /// <summary>
        /// Awake.
        /// </summary>
        void Awake()
        {
            _panel = GameObject.Find("ChoseGangPanelPrefab");
            var images = _panel.GetComponentsInChildren<Image>().Where(go => go.name.EndsWith("Image")).ToList();

            foreach (var image in images)
            {
                var trigger = image.GetComponent<EventTrigger>();
                HelperSingleton.Instance.AddEventTrigger(trigger, OnPointerEnter, EventTriggerType.PointerEnter);
                HelperSingleton.Instance.AddEventTrigger(trigger, OnPointerExit, EventTriggerType.PointerExit);

                var texts = image.GetComponentsInChildren<Text>();
                _assignment.Add(image.gameObject, 
                    new KeyValuePair<Text, Text>(texts.First(tx => tx.gameObject.name.EndsWith("Title")), texts.First(tx => tx.gameObject.name.EndsWith("Text"))));
                _assignment[image.gameObject].Value.gameObject.SetActive(false);
            }            

            SwitchChooseYourGangPanel();
        }

        /// <summary>
        /// Gang has been selected
        /// </summary>
        /// <param name="gangId"></param>
        public void GangClicked(int gangId)
        {
            CharacterSingleton.Instance.GangOfPlayer = (Gangs)gangId;
            SwitchChooseYourGangPanel();
        }

        /// <summary>
        /// On Pointer Enter
        /// </summary>
        private void OnPointerEnter()
        {
            PointerEventData pe = new PointerEventData(EventSystem.current);
            pe.position = Input.mousePosition;
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pe, hits);
            {
                var hit = hits.FirstOrDefault(rr => _assignment.Keys.Contains(rr.gameObject));
                if (hit.gameObject != null)
                {
                    _assignment[hit.gameObject].Key.gameObject.SetActive(false);
                    _assignment[hit.gameObject].Value.gameObject.SetActive(true);
                }                
            }
        }

        /// <summary>
        /// Leave field
        /// </summary>
        private void OnPointerExit()
        {
            foreach (var pair in _assignment.Values)
            {
                pair.Key.gameObject.SetActive(true);
                pair.Value.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchChooseYourGangPanel()
        {
            _panel.SetActive(!_panel.activeSelf);
            if (_panel.activeSelf)
            {
                _panel.transform.SetAsLastSibling();
            }
        }
    }
}

using Enum;
using Interfaces;
using Singleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class ChooseYourGangHandler : MonoBehaviour
    {
        private GameObject _panel;
        private List<Image> _images;
        private List<Component> _cc_controls;
        private InputField _nameInputField;
        private Dictionary<GameObject, KeyValuePair<Text, Text>> _assignment = new Dictionary<GameObject, KeyValuePair<Text, Text>>();
        private Button _moreButton;
        private Button _lessButton;
        private Button _createButton;
        private int _pointsToSpare = 6;
        private MemberSkills _actualSkill = MemberSkills.NotSet;
        private IGangMember _player;
        private Text _pointsLeftText;
        private Text _initative;
        private Text _courage;
        private Text _intelligence;
        private Text _strength;
        private Text _accuracy;


        /// <summary>
        /// Awake.
        /// </summary>
        void Start()
        {
            _panel = GameObject.Find("ChoseGangPanelPrefab");
            _images = _panel.GetComponentsInChildren<Image>().Where(go => go.name.EndsWith("Image")).ToList();
            _cc_controls = _panel.GetComponentsInChildren<Component>().Where(cntrl => cntrl.gameObject.name.StartsWith("CC_")).ToList();

            _lessButton = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Button) && cntrl.gameObject.name == "CC_LessButton") as Button;
            _moreButton = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Button) && cntrl.gameObject.name == "CC_MoreButton") as Button;
            _createButton = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Button) && cntrl.gameObject.name == "CC_CreateButton") as Button;
            _nameInputField = _cc_controls.First(cntrl => cntrl.GetType() == typeof(InputField) && cntrl.gameObject.name == "CC_InputFieldName") as InputField;
            _pointsLeftText = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name == "CC_RemainingPoints") as Text;

            _initative = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name == "CC_InitiativeValue") as Text;
            _courage = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name == "CC_CourageValue") as Text;
            _intelligence = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name == "CC_IntelligenceValue") as Text;
            _strength = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name == "CC_StrengthValue") as Text;
            _accuracy = _cc_controls.First(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name == "CC_AccuracyValue") as Text;

            _moreButton.onClick.AddListener(() => ModifyPoints(-1));
            _lessButton.onClick.AddListener(() => ModifyPoints(1));
            _createButton.onClick.AddListener(() => CreateCharacter());

            // Set OnPointerEnter for Value fields.
            foreach (var text in _cc_controls.Where(cntrl => cntrl.GetType() == typeof(Text) && cntrl.gameObject.name.EndsWith("Value")))
            {
                var trigger = text.GetComponent<EventTrigger>();
                HelperSingleton.Instance.AddEventTrigger(trigger, OnPointerEnterPoints, EventTriggerType.PointerEnter);
            }

            // Set OnPoimterEnter for Images
            foreach (var image in _images)
            {
                var trigger = image.GetComponent<EventTrigger>();
                HelperSingleton.Instance.AddEventTrigger(trigger, OnPointerEnter, EventTriggerType.PointerEnter);
                HelperSingleton.Instance.AddEventTrigger(trigger, OnPointerExit, EventTriggerType.PointerExit);

                var texts = image.GetComponentsInChildren<Text>();
                _assignment.Add(image.gameObject, 
                    new KeyValuePair<Text, Text>(texts.First(tx => tx.gameObject.name.EndsWith("Title")), texts.First(tx => tx.gameObject.name.EndsWith("Text"))));
                _assignment[image.gameObject].Value.gameObject.SetActive(false);
            }

            // Replace 
            var allTexts = _panel.GetComponentsInChildren<Text>().ToList();
            GUIHelper.ReplaceText(allTexts);

            _cc_controls.ForEach(cntrl => cntrl.gameObject.SetActive(false));

            SwitchChooseYourGangPanel();
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

        /// <summary>
        /// Gang has been selected
        /// </summary>
        /// <param name="gangId">The clicked gang</param>
        public void GangClicked(int gangId)
        {
            CharacterSingleton.Instance.GangOfPlayer = (Gangs)gangId;

            _images.ForEach(img => img.gameObject.SetActive(false));
            _cc_controls.ForEach(cntrl => cntrl.gameObject.SetActive(true));

            _player = CharacterSingleton.Instance.GenerateAIPlayer(1);
            _player.Accuracy = 8;
            _player.Courage = 8;
            _player.Initiative = 8;
            _player.Intelligence = 8;
            _player.Strength = 8;
        }

        /// <summary>
        /// Creates the main character.
        /// </summary>
        public void CreateCharacter()
        {
            if (!string.IsNullOrEmpty(_nameInputField.text) && _pointsToSpare == 0)
            {
                _player.Name = _nameInputField.text;

                CharacterSingleton.Instance.PlayersGang.Add(_player);

                SwitchChooseYourGangPanel();
            }
            else
            {
                PrefabSingleton.Instance.InputHandler.AddQuestion("ChoseGangSpendAll");
            }            
        }

        /// <summary>
        /// Adapts Points
        /// </summary>
        /// <param name="pointChange"></param>
        private void ModifyPoints(int pointChange)
        {
            if (_pointsToSpare == 0 && pointChange < 0)
            {
                return;
            }

            bool abort = false;
            switch (_actualSkill)
            {
                case MemberSkills.Strength:
                    abort = _player.Strength == 5 && pointChange == 1;
                    _player.Strength -= abort ? 0 : pointChange;
                    break;
                case MemberSkills.Initiative:
                    abort = _player.Initiative == 5 && pointChange == 1;
                    _player.Initiative -= abort ? 0 : pointChange;
                    break;
                case MemberSkills.Courage:
                    abort = _player.Courage == 5 && pointChange == 1;
                    _player.Courage -= abort ? 0 : pointChange;
                    break;
                case MemberSkills.Intelligence:
                    abort = _player.Intelligence == 5 && pointChange == 1;
                    _player.Intelligence -= abort ? 0 : pointChange;
                    break;
                case MemberSkills.Accuracy:
                    abort = _player.Accuracy == 5 && pointChange == 1;
                    _player.Accuracy -= abort ? 0 : pointChange;
                    break;
                default:
                    break;
            }

            if (!abort)
            {
                _pointsToSpare += pointChange;
                _pointsLeftText.text = _pointsToSpare.ToString();

                _initative.text = _player.Initiative.ToString();
                _courage.text = _player.Courage.ToString();
                _intelligence.text = _player.Intelligence.ToString();
                _strength.text = _player.Strength.ToString();
                _accuracy.text = _player.Accuracy.ToString();
            }
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
        /// User hoovered above value.
        /// </summary>
        private void OnPointerEnterPoints()
        {
            PointerEventData pe = new PointerEventData(EventSystem.current);
            pe.position = Input.mousePosition;
            var hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pe, hits);
            {
                var hit = hits.FirstOrDefault(label => label.gameObject.name.EndsWith("Value"));
                if (hit.gameObject != null)
                {
                    _lessButton.transform.position = new Vector3(_lessButton.transform.position.x, hit.gameObject.transform.position.y, _lessButton.transform.position.z);
                    _moreButton.transform.position = new Vector3(_moreButton.transform.position.x, hit.gameObject.transform.position.y, _moreButton.transform.position.z);

                    var name = hit.gameObject.name.Substring(3, hit.gameObject.name.Length - 8);
                    if (System.Enum.IsDefined(typeof(MemberSkills), name))
                    {
                        _actualSkill = (MemberSkills)System.Enum.Parse(typeof(MemberSkills), name, true);
                    }
                    else
                    {
                        Debug.LogError(name);
                    }
                }
            }
        }
    }
}
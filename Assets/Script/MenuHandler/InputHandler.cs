using Singleton;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class InputHandler : MonoBehaviour
    {
        private GameObject _panel;
        private Button _answer1Button;
        private Button _answer2Button;
        private Text _title;
        private Text _question;
        private Text _answer1Text;
        private Text _answer2Text;

        public int AnswerGiven { get; private set; }

        /// <summary>
        /// Starts form
        /// </summary>
        void Start()
        {
            _panel = GameObject.Find("InputPanelPrefab");

            var buttons = _panel.GetComponentsInChildren<Button>();
            _answer1Button = buttons.First(btn => btn.name == "Answer1Button");
            _answer2Button = buttons.First(btn => btn.name == "Answer2Button");

            _answer1Button.onClick.AddListener(() => AnswerClicked(1));
            _answer2Button.onClick.AddListener(() => AnswerClicked(2));

            var texts = _panel.GetComponentsInChildren<Text>();
            _title = texts.First(btn => btn.name == "QuestionTitleText");
            _question = texts.First(btn => btn.name == "QuestionText");
            _answer1Text = texts.First(btn => btn.name == "Answer1Text");
            _answer2Text = texts.First(btn => btn.name == "Answer2Text");

            SwitchPanel();
        }

        /// <summary>
        /// Switchs the scene end panel.
        /// </summary>
        public void SwitchPanel()
        {
            _panel.SetActive(!_panel.activeSelf);
            if (_panel.activeSelf)
            {
                _panel.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// Adds a question.
        /// </summary>
        public void AddQuestion(string questionKey, Dictionary<string, string> parameters = null)
        {
            SwitchPanel();
            AnswerGiven = 0;

            _title.text = ResourceSingleton.Instance.GetText(string.Concat(questionKey, "Title"));
            _question.text = ResourceSingleton.Instance.GetText(questionKey);
            _answer1Text.text = ResourceSingleton.Instance.GetText(string.Concat(questionKey, "A1"));
            _answer2Text.text = ResourceSingleton.Instance.GetText(string.Concat(questionKey, "A2"));

            if (parameters != null)
            {
                foreach (var pair in parameters)
                {
                    _question.text = _question.text.Replace(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// Waits for an Answer
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForAnswer()
        {
            while(AnswerGiven == 0)
            {
                yield return StartCoroutine(WaitMore());
            }

            yield return null;
        }

        /// <summary>
        /// Wait a little bit more!
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitMore()
        {
            yield return new WaitForSeconds(.25f);
        }

        /// <summary>
        /// Adds members to the gang
        /// </summary>
        private void AnswerClicked(int answer)
        {
            AnswerGiven = answer;

            SwitchPanel();
        }
    }
}
using Singleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class ProfileSelectorHandler : MonoBehaviour
    {
		private InputField[] _profileFields = new InputField[5];
		private Text[] _profileButtons = new Text[5];

        private GameObject _profileSelectorPanel;

        public delegate void ActualProfileChanged(object sender, EventArgs e);
        public event EventHandler ProfileChanged;

        public virtual void OnProfileChanged(EventArgs e)
        {
            if (ProfileChanged != null)
            {
                ProfileChanged(this, e);
            }
        }

        /// <summary>
        /// Startup
        /// </summary>
        void Start()
        {
            _profileSelectorPanel = GameObject.Find("NewGamePanelPrefab");

            if (_profileSelectorPanel != null)
            {
                IEnumerable<InputField> fields = _profileSelectorPanel.GetComponentsInChildren<InputField>();
                _profileFields[0] = fields.First(ip => ip.name == "InputFieldProfile1");
                _profileFields[1] = fields.First(ip => ip.name == "InputFieldProfile2");
                _profileFields[2] = fields.First(ip => ip.name == "InputFieldProfile3");
                _profileFields[3] = fields.First(ip => ip.name == "InputFieldProfile4");
                _profileFields[4] = fields.First(ip => ip.name == "InputFieldProfile5");

                IEnumerable<Text> buttons = _profileSelectorPanel.GetComponentsInChildren<Text>();
                _profileButtons[0] = buttons.First(ip => ip.name == "TextProfile1");
                _profileButtons[1] = buttons.First(ip => ip.name == "TextProfile2");
                _profileButtons[2] = buttons.First(ip => ip.name == "TextProfile3");
                _profileButtons[3] = buttons.First(ip => ip.name == "TextProfile4");
                _profileButtons[4] = buttons.First(ip => ip.name == "TextProfile5");

                _profileSelectorPanel.SetActive(false);
            }
        }

        /// <summary>
        /// Switchs the options panel.
        /// </summary>
        public void SwitchProfileSelectorPanel()
        {
            _profileSelectorPanel.SetActive(!_profileSelectorPanel.activeSelf);

            if (_profileSelectorPanel.activeSelf)
            {
                _profileSelectorPanel.transform.SetAsLastSibling();

                for (int i = 0; i < 5; i++)
                {
                    _profileFields[i].text = PrefabSingleton.Instance.ProfileContainer.Profile[i];
                    _profileButtons[i].text = String.IsNullOrEmpty(PrefabSingleton.Instance.ProfileContainer.Profile[i]) ? "Create" : "Choose";
                }
            }
        }

        /// <summary>
        /// Creates the profile.
        /// </summary>
        /// <param name="profileNumber">Profile number.</param>
        public void CreateSwitchProfile(int profileNumber)
        {
            if (_profileButtons[profileNumber].text == "Create" && String.IsNullOrEmpty(_profileFields[profileNumber].text))
            {
                // Nothing to create...
                return;
            }

            if (_profileButtons[profileNumber].text == "Create")
            {
                PrefabSingleton.Instance.ProfileContainer.Profile[profileNumber] = _profileFields[profileNumber].text;
                PrefabSingleton.Instance.ChooseYourGangHandler.SwitchChooseYourGangPanel();
            }

            PrefabSingleton.Instance.ProfileContainer.ActiveProfile = _profileFields[profileNumber].text;

            SaveProfiles();

            OnProfileChanged(EventArgs.Empty);

            _profileSelectorPanel.SetActive(false);
        }

        /// <summary>
        /// Saves the profiles.
        /// </summary>
        public void SaveProfiles()
        {
            if (PrefabSingleton.Instance.ProfileContainer == null)
            {
                return;
            }

            // Save.
            BinaryFormatter formatter = new BinaryFormatter();

            string filePath = Application.persistentDataPath + "/profiles.dat";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            FileStream file = File.Open(filePath, FileMode.OpenOrCreate);

            formatter.Serialize(file, PrefabSingleton.Instance.ProfileContainer);
            file.Flush();
            file.Close();
        }
    }
}

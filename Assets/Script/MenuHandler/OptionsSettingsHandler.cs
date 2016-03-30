using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Singleton;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace Menu
{
	public class OptionsSettingsHandler : MonoBehaviour
	{
		private GameObject _settingsPanel;
		private Toggle _ssaoToggle;
		private Toggle _aaToggle;
		private Toggle _sfxToggle;
		private Toggle _musicToggle;


		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start()
		{
			_settingsPanel = GameObject.Find("OptionsSettingsPanelPrefab");

			if (_settingsPanel != null)
			{
				IEnumerable<Toggle> toggle = _settingsPanel.GetComponentsInChildren<Toggle>();
				_ssaoToggle = toggle.First(tg => tg.name == "SSAOToggle");
				_aaToggle = toggle.First(tg => tg.name == "AntiAliasingToggle");
				_musicToggle = toggle.First(tg => tg.name == "MusicToggle");
				_sfxToggle = toggle.First(tg => tg.name == "SoundToggle");
                _settingsPanel.SetActive(false);
			}

			var inst = PrefabSingleton.Instance;
		}

		/// <summary>
		/// Switchs the options panel.
		/// </summary>
		public void SwitchOptionsSettingsPanel()
		{
			_settingsPanel.SetActive(!_settingsPanel.activeSelf);

			if (_settingsPanel.activeSelf)
			{
				_settingsPanel.transform.SetAsLastSibling();
				_ssaoToggle.isOn = PrefabSingleton.Instance.ProfileContainer.SSAOIsActive;
				_aaToggle.isOn = PrefabSingleton.Instance.ProfileContainer.AAIsActive;
				_sfxToggle.isOn = PrefabSingleton.Instance.ProfileContainer.SFXsActive;
				_musicToggle.isOn = PrefabSingleton.Instance.ProfileContainer.MusicIsActive;
			}
			else
			{
				PrefabSingleton.Instance.ProfileSelectorHandler.SaveProfiles();
			}
		}

		/// <summary>
		/// Raises the value changed event.
		/// </summary>
		/// <param name="newValue">If set to <c>true</c> new value.</param>
		public void OnValueChanged()
		{
			PrefabSingleton.Instance.ProfileContainer.SSAOIsActive = _ssaoToggle.isOn;
            PrefabSingleton.Instance.ProfileContainer.AAIsActive = _aaToggle.isOn;
            PrefabSingleton.Instance.ProfileContainer.SFXsActive = _sfxToggle.isOn;
            PrefabSingleton.Instance.ProfileContainer.MusicIsActive = _musicToggle.isOn;
        }
	}
}
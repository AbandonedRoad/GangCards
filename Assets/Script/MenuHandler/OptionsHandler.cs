using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Singleton;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Misc;
using UnityEngine.SceneManagement;
using Enum;

namespace Menu
{
	public class OptionsHandler : MonoBehaviour 
	{
        private GameObject _blackBackground;
        private GameObject _optionsPanel;
        private Text _actualProfileText;
        private Button _continueButton;
        private List<FileInfo> _files;

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake()
		{
			PrefabSingleton.Instance.Init();

            _optionsPanel = GameObject.Find("OptionsPanelPrefab");
            _blackBackground = GameObject.Find("BlackBackgroundPanel");
            PrefabSingleton.Instance.ProfileSelectorHandler.ProfileChanged += HandleProfileChanged;
            _continueButton = _optionsPanel.GetComponentsInChildren<Button>().First(btn => btn.gameObject.name == "ContinueButton");

            DirectoryInfo dir = new DirectoryInfo("Assets/Resources/");
            _files = dir.GetFiles("*.*", SearchOption.AllDirectories).ToList();

            LoadProfiles();

            _actualProfileText = _optionsPanel.GetComponentsInChildren<Text>().FirstOrDefault(tx => tx.name == "ActualProfileName");
            _actualProfileText.text = PrefabSingleton.Instance.ProfileContainer.ActiveProfile;
            LoadSceneData();
        }

		/// <summary>
		/// Handles the profile changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void HandleProfileChanged (object sender, EventArgs e)
		{
			PrefabSingleton.Instance.LoadGameData();
            _actualProfileText.text = PrefabSingleton.Instance.ProfileContainer.ActiveProfile;

            _continueButton.interactable = true;
        }

		/// <summary>
		/// Switchs the options panel.
		/// </summary>
		public void SwitchOptionsPanel()
		{
            if (_optionsPanel.activeSelf && String.IsNullOrEmpty(PrefabSingleton.Instance.ProfileContainer.ActiveProfile))
            {
                // No Profile is selected - do not allow close
                return;
            }

			_optionsPanel.SetActive(!_optionsPanel.activeSelf);

            _blackBackground.SetActive(_optionsPanel.activeSelf);

            Time.timeScale = _optionsPanel.activeSelf ? 0: 1;
        }

		/// <summary>
		/// Exit the game.
		/// </summary>
		public void Exit()
		{
            Save();

			Application.Quit();
		}

		/// <summary>
		/// Save the specified gameData.
		/// </summary>
		/// <param name="gameData">Game data.</param>
		public void Save()
		{
			// Save general game data.
			SaveGameData();

			// Save all Buildings
			SceneData sceneData = new SceneData();
            List<GameObject> allCars = new List<GameObject>(GameObject.FindGameObjectsWithTag("Car"));
            allCars.AddRange(GameObject.FindGameObjectsWithTag("PlayersCar").ToList());
			foreach (GameObject car in allCars) 
			{
				GOInfo goInfo = new GOInfo();

                goInfo.PrefabName = car.gameObject.name;
                goInfo.GOType = GOType.Car;
				goInfo.SetVector(GOVectorProperty.Position, car.transform.position);
				goInfo.SetVector(GOVectorProperty.Rotation, car.transform.rotation.eulerAngles);

				sceneData.GameObjects.Add(goInfo);
			}

			// Save GangInfo
			foreach (var gangMember in CharacterSingleton.Instance.PlayersGang) 
			{
				sceneData.GangMembers.Add(gangMember);
			}

            sceneData.Money = CharacterSingleton.Instance.AvailableMoney;
            sceneData.GangOfPlayer = CharacterSingleton.Instance.GangOfPlayer;

            // Save Camera Data
            sceneData.SetVector(SceneVectorProperty.MainCameraPosition, Camera.main.transform.position);
			sceneData.SetVector(SceneVectorProperty.MainCameraRotation, Camera.main.transform.rotation.eulerAngles);
			// sceneData.SetVector(SceneVectorProperty.MiniMapCameraPosition, PrefabSingleton.Instance.MiniMapCamera.transform.position);
			// sceneData.SetVector(SceneVectorProperty.MiniMapCameraRotation, PrefabSingleton.Instance.MiniMapCamera.transform.rotation.eulerAngles);

			BinaryFormatter formatter = new BinaryFormatter();

			string filePath = String.Concat(Application.persistentDataPath, "/", PrefabSingleton.Instance.ProfileContainer.ActiveProfile,
                "_", PrefabSingleton.Instance.ActualGameData.ActualScene, ".dat");
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
		
			FileStream file = File.Open(filePath, FileMode.OpenOrCreate);

			formatter.Serialize(file, sceneData);
			file.Flush();
			file.Close();

			HelperSingleton.Instance.LogMessages.Add(new LogInfo("Successfully saved to: " + Application.persistentDataPath));
		}

		/// <summary>
		/// Raises the level was loaded event.
		/// </summary>
		/// <param name="level">Level.</param>
		void OnLevelWasLoaded(int level) 
		{
			LoadSceneData();
		}

        /// <summary>
        /// Load Profiles
        /// </summary>
        private void LoadProfiles()
        {
            // Load.
            BinaryFormatter formatter = new BinaryFormatter();
            var filePath = Application.persistentDataPath + "/profiles.dat";

            if (File.Exists(filePath))
            {
                FileStream file = File.Open(filePath, FileMode.Open);
                Debug.Log("Checking Profile: " + file);
                PrefabSingleton.Instance.ProfileContainer = formatter.Deserialize(file) as PlayerProfile;

                file.Close();
            }
            else
            {
                Debug.LogWarning("No Profile file!");
            }
        }

        /// <summary>
        /// Loads the scene data.
        /// </summary>
        public void LoadSceneData()
		{
            if (String.IsNullOrEmpty(PrefabSingleton.Instance.ProfileContainer.ActiveProfile))
            {
                // No active user - nothing to load.
                return;
            }            

            string zoneToLoad = String.Concat(Application.persistentDataPath, "/", PrefabSingleton.Instance.ProfileContainer.ActiveProfile + "_" + SceneManager.GetActiveScene().name, ".dat");
            Debug.Log("Checking Scene: " + zoneToLoad);

            // Initialize the Prefabs.
            // PrefabSingleton.Instance.Init();
            if (File.Exists(zoneToLoad))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream file = File.Open(zoneToLoad, FileMode.Open);
				SceneData sceneData = formatter.Deserialize(file) as SceneData;
				file.Close();
					
				if (sceneData == null)
				{
					throw new Exception("Scene data is NULL!");
				}
					
				HandleSceneData(sceneData);
					
				Time.timeScale = 1;
			}
			else 
			{
				HelperSingleton.Instance.LogMessages.Add(new LogInfo("The Save game for Zone '" + SceneManager.GetActiveScene().name + "' does not exist! Abort"));
			}
				
			// Init all Singletons.
		}

		/// <summary>
		/// Saves the game data.
		/// </summary>
		/// <param name="data">Data.</param>
		private void SaveGameData()
		{
			// Actual scene
			PrefabSingleton.Instance.ActualGameData.ActualScene = SceneManager.GetActiveScene().name;

			// Save.
			BinaryFormatter formatter = new BinaryFormatter();

			string filePath = Application.persistentDataPath + "/game" + PrefabSingleton.Instance.ProfileContainer.ActiveProfile + ".dat";
            Debug.Log("Saving: " + filePath);
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			
			FileStream file = File.Open(filePath, FileMode.OpenOrCreate);
			
			formatter.Serialize(file, PrefabSingleton.Instance.ActualGameData);
			file.Flush();
			file.Close();
		}

		/// <summary>
		/// Handles the game data.
		/// </summary>
		private void HandleSceneData(SceneData sceneData)
		{
			// Destroy all Stuff - they will be loaded again.
			GameObject.FindGameObjectsWithTag("Car").ToList().ForEach(go => Destroy(go));
            GameObject.FindGameObjectsWithTag("PlayersCar").ToList().ForEach(go => Destroy(go));

			// Loop GameObjects
			foreach (GOInfo go in sceneData.GameObjects) 
			{
				try 
				{
                    var fullPath = _files.FirstOrDefault(path => path.FullName.Contains(go.PrefabName));

                    var removedExt = Path.ChangeExtension(fullPath.FullName, String.Empty);
                    removedExt = removedExt.Substring(0, removedExt.Length - 1);
                    var splitUp = removedExt.Split(Path.DirectorySeparatorChar).ToList();

                    var index = splitUp.IndexOf("Resources") + 1;
                    var prefabPath = String.Empty;
                    for (int i = index ; i < splitUp.Count; i++)
                    {
                        prefabPath = Path.Combine(prefabPath, splitUp[i]);
                    }

                    GameObject createdGO = null;
                    createdGO = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;


                    Instantiate(createdGO, go.GetVector(GOVectorProperty.Position), Quaternion.Euler(go.GetVector(GOVectorProperty.Rotation)));
                } 

				catch (Exception ex) 
				{
					HelperSingleton.Instance.LogMessages.Add(new LogInfo("Loading failed! " + ex.InnerException != null ? ex.InnerException.Message : ex.Message));
				}
			}

            foreach (var member in sceneData.GangMembers)
            {
                CharacterSingleton.Instance.AddAIPlayer(member);
            }

            CharacterSingleton.Instance.AvailableMoney = sceneData.Money;
            CharacterSingleton.Instance.GangOfPlayer = sceneData.GangOfPlayer;


            // Read Camera data
            Camera.main.transform.position = sceneData.GetVector(SceneVectorProperty.MainCameraPosition);
			Camera.main.transform.rotation = Quaternion.Euler (sceneData.GetVector(SceneVectorProperty.MainCameraRotation));
			// PrefabSingleton.Instance.MiniMapCamera.transform.position = sceneData.GetVector(SceneVectorProperty.MiniMapCameraPosition);
			// PrefabSingleton.Instance.MiniMapCamera.transform.rotation = Quaternion.Euler (sceneData.GetVector(SceneVectorProperty.MiniMapCameraRotation));

			HelperSingleton.Instance.LogMessages.Add(new LogInfo("Loading succeeded!"));
		}
	}
}
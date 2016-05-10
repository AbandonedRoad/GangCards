using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;
using Menu;
using Misc;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Singleton
{
    public class PrefabSingleton
    {
        private static PrefabSingleton _instance;

        public ActionAfterEffactsHandler ActionAfterEffactsHandler { get; private set; }
        public PlayerActionsHandler ActionsHandler { get; private set; }
        public BuildingActionsHandler BuildingActionsHandler { get; private set; }
        public ItemHandler ItemHandler { get; private set; }
        public DebugHandler DebugHandler { get; private set; }
        public OptionsSettingsHandler OptionsSettingsHandler { get; private set; }
        public OptionsHandler OptionsHandler { get; private set; }
        public ProfileSelectorHandler ProfileSelectorHandler { get; private set; }
        public FightingHandler FightingHandler { get; private set; }
        public ChooseYourGangHandler ChooseYourGangHandler { get; private set; }
        public InputHandler InputHandler { get; private set; }

        public RegularUpdate RegularUpdate { get; private set; }
        public PlayerProfile ProfileContainer { get; set; }
        public GameData ActualGameData { get; set; }
        public Sprite FightingSkullSprite { get; private set; }
        public Sprite FightingRegularSprite { get; private set; }

        public GameObject PlayersCar { get; set; }
        public MoveCars PlayersCarScript { get; set; }

        /// <summary>
        /// Gets instance
        /// </summary>
        public static PrefabSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PrefabSingleton();
                    _instance.Init();
                    _instance.LoadImages();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        public void Init()
        {
            // Scripts
            var handlingPrefab = GameObject.Find("HandlingObjectPrefab");
            ActionAfterEffactsHandler = handlingPrefab.GetComponent<ActionAfterEffactsHandler>();
            ActionsHandler = handlingPrefab.GetComponent<PlayerActionsHandler>();
            BuildingActionsHandler = handlingPrefab.GetComponent<BuildingActionsHandler>();
            RegularUpdate = handlingPrefab.GetComponent<RegularUpdate>();
            ItemHandler = handlingPrefab.GetComponent<ItemHandler>();
            DebugHandler = handlingPrefab.GetComponent<DebugHandler>();
            OptionsSettingsHandler = handlingPrefab.GetComponent<OptionsSettingsHandler>();
            OptionsHandler = handlingPrefab.GetComponent<OptionsHandler>();
            ProfileSelectorHandler = handlingPrefab.GetComponent<ProfileSelectorHandler>();
            FightingHandler = handlingPrefab.GetComponent<FightingHandler>();
            ChooseYourGangHandler = handlingPrefab.GetComponent<ChooseYourGangHandler>();
            InputHandler = handlingPrefab.GetComponent<InputHandler>();

            PlayersCar = GameObject.Find("PlayerCarsPrefab");
            PlayersCarScript = PlayersCar.GetComponent<MoveCars>();

            ProfileContainer = new PlayerProfile();
            ActualGameData = new GameData();
        }

        /// <summary>
        /// Loads relevant images
        /// </summary>
        private void LoadImages()
        {
            var sprites = Resources.LoadAll<Sprite>("Faces");
            FightingSkullSprite = sprites.First(spr => spr.name == "Skull");
            FightingRegularSprite = sprites.First(spr => spr.name == "EnemyBorder");
        }

        /// <summary>
        /// Create the specified toBeCreated and pos.
        /// </summary>
        /// <param name="toBeCreated">To be created.</param>
        /// <param name="pos">Position.</param>
        public GameObject Create(GameObject toBeCreated, Vector3? pos = null)
        {
            GameObject result;
            result = GameObject.Instantiate(toBeCreated);
            result.transform.position = pos.HasValue ? pos.Value : result.transform.position;
            result.name = result.name.Substring(0, result.name.Length - 7);

            return result;
        }

        /// <summary>
        /// Load this instance.
        /// </summary>
        public void LoadGameData()
        {
            string path = Application.persistentDataPath + "/game" + PrefabSingleton.Instance.ProfileContainer.ActiveProfile + ".dat";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);
                PrefabSingleton.Instance.ActualGameData = formatter.Deserialize(file) as GameData;

                file.Close();
            }
            else
            {
                PrefabSingleton.Instance.ActualGameData = new GameData();
            }
        }
    }
}
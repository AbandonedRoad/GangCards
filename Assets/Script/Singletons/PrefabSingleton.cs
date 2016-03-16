using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;
using Menu;

namespace Singleton
{
    public class PrefabSingleton
    {
        private static PrefabSingleton _instance;

        public ActionAfterEffactsHandler ActionAfterEffactsHandler { get; private set; }
        public PlayerActionsHandler ActionsHandler { get; private set; }
        public BuildingActionsHandler BuildingActionsHandler { get; private set; }

        public RegularUpdate RegularUpdate { get; private set; }

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

            PlayersCar = GameObject.Find("PlayerCarsPrefab");
            PlayersCarScript = PlayersCar.GetComponent<MoveCars>();
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
    }
}
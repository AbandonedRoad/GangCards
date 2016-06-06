using Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Singleton
{
    class WorldHelperSingleton
    {
        private static WorldHelperSingleton _instance;

        /// <summary>
        /// Gets instance
        /// </summary>
        public static WorldHelperSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new WorldHelperSingleton();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Create a few cars.
        /// </summary>
        /// <param name="amount"></param>
        public void CreateCars(int amount)
        {
            var allStreets = GameObject.FindGameObjectsWithTag("StreeSpawnPoint");

            var streetParent = GameObject.Find("_Streets");
            var carsParent = GameObject.Find("_Cars");

            for (int i = 0; i < amount; i++)
            {
                var streetToBeUsed = allStreets.ElementAt(Random.Range(0, allStreets.Count()));

                var toBeCreated = PrefabSingleton.Instance.AllCars.ElementAt(Random.Range(0, PrefabSingleton.Instance.AllCars.Count));
                var created = PrefabSingleton.Instance.Create(toBeCreated);
                created.transform.SetParent(streetParent.transform);
                created.tag = "Car";

                var angle = Math.Round(streetToBeUsed.transform.rotation.eulerAngles.y, 0);
                if (angle == 90 || angle == 270)
                {
                    created.transform.position =
                        new Vector3(streetToBeUsed.transform.position.x - 5f, streetToBeUsed.transform.position.y, streetToBeUsed.transform.position.z - 1.9f);
                    created.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                }
                else if (angle == 0 || angle == 180)
                {
                    created.transform.position =
                        new Vector3(streetToBeUsed.transform.position.x + 1.9f, streetToBeUsed.transform.position.y, streetToBeUsed.transform.position.z - 5f);
                    created.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                var moveCars = created.AddComponent<MoveCars>();
                moveCars.Speed = 0.3f;
                var rigidBody = created.AddComponent<Rigidbody>();
                rigidBody.mass = 1;
                rigidBody.angularDrag = 0.05f;
                rigidBody.drag = 0;
                rigidBody.useGravity = false;
                rigidBody.isKinematic = true;
                created.transform.SetParent(carsParent.transform);
            }
        }
    }
}

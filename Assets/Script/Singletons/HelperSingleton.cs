using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Assets.Script;
using System.Globalization;
using Misc;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Singleton
{
    public class HelperSingleton
    {
        private Collider _terrainCollider;
        private static HelperSingleton _instance;

        public GameObject SelectedObject { get; set; }      // The object, which is build right now.
        public List<LogInfo> LogMessages { get; private set; }

        /// <summary>
        /// Gets instance
        /// </summary>
        public static HelperSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new HelperSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            _terrainCollider = Terrain.activeTerrain.GetComponent<Collider>();
            LogMessages = new List<LogInfo>();
        }

        /// <summary>
        /// Sets the cursor.
        /// </summary>
        /// <param name="newCursor">New cursor.</param>
        public GameObject GetTopMostGO(GameObject gameObject, bool getLastTagged)
        {
            if (gameObject == null)
            {
                return null;
            }

            GameObject intermediateResult = null;
            GameObject parent = gameObject;
            while (parent.transform.parent != null)
            {
                var newParent = parent.transform.parent.gameObject;

                if (getLastTagged && newParent.tag != "Container")
                {
                    intermediateResult = newParent;
                }

                if (newParent.tag == "Container")
                {
                    // This is only a container - we reached the end!
                    return parent;
                }

                parent = newParent;
            }

            if (parent.tag == "Container" && getLastTagged)
            {
                // If we wanted to return the last tagged, return it.
                return intermediateResult != null
                    ? intermediateResult
                    : parent;
            }

            return parent;
        }

        /// <summary>
        /// Creates a new seed for the next level
        /// </summary>
        /// <returns></returns>
        public int CreateSeed(int? newSeed = null)
        {
            int seed = newSeed.HasValue
                ? newSeed.Value
                : (DateTime.Now.Hour * DateTime.Now.Minute) + (DateTime.Now.Second * 4) - (DateTime.Now.Second * 2);

            Debug.Log("Last seed: " + seed.ToString());

            return seed;
        }

        /// <summary>
        /// Gets the GO which is the nearest to the player.
        /// </summary>
        /// <param name="">.</param>
        /// <param name="myPosition">My position.</param>
        public GameObject GetNearestGameObject(IEnumerable<GameObject> objects, Vector3 myPosition)
        {
            objects = objects.Distinct();
            Dictionary<GameObject, float> distanceToObject = new Dictionary<GameObject, float>();

            if (!objects.Any() || objects.All(ob => ob == null))
            {
                return null;
            }

            foreach (var go in objects)
            {
                distanceToObject.Add(go, (go.transform.position - myPosition).magnitude);
            }

            return distanceToObject.OrderBy(pair => pair.Value).First().Key;
        }

        /// <summary>
        /// Returns the center of the gameobject, using the collidor attached to the go. If no collidier is attached, transform.position is returned.
        /// </summary>
        /// <returns>The center of game object.</returns>
        /// <param name="gameObject">Game object.</param>
        public Vector3 GetCenterOfGameObject(GameObject gameObject)
        {
            var collidor = gameObject.GetComponent<Collider>();
            if (collidor != null)
            {
                return collidor.bounds.center;
            }

            return gameObject.transform.position;
        }

        /// <summary>
        /// Splits up a string, whereby each uppercase letter gets a new word.
        /// </summary>
        /// <returns>The string split up.</returns>
        /// <param name="splitUp">String to be split.</param>
        public string SplitUp(string splitUp)
        {
            string output = String.Empty;
            foreach (char letter in splitUp)
            {
                if (Char.IsUpper(letter) && output.Length > 0)
                    output += " " + letter;
                else
                    output += letter;
            }

            return output;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <returns>The size.</returns>
        /// <param name="objectToCheck">Object to check.</param>
        /// <param name="getForMaster">If TRUE, the MASTERRENDERTAG is used - this one is used for level blocks - DEFAULT!
        /// Otherwise RENDEROBJECT is used - it is used for all other objects</param>
        public Vector3 GetSize(GameObject objectToCheck, bool getForMaster = true)
        {
            string tag = getForMaster
                ? "MasterRenderObject"
                : "RenderObject";

            if (objectToCheck == null)
            {
                Debug.LogError("Object to check is null!");
            }

            // Rotate everything to the same rotation before checking, otherwise sizes will eb diffent
            Quaternion oldRotation = objectToCheck.transform.rotation;
            objectToCheck.transform.rotation = Quaternion.Euler(Vector3.zero);

            // TODO: This does not work in all cases, as a level block may contain severla other pieces which do also contain a RenderObject.
            // When trying to allign to level blocks they may overlap. Seed == 109.

            Renderer renderer = objectToCheck.tag == tag
                ? objectToCheck.GetComponent<Renderer>()
                : objectToCheck.GetComponentsInChildren<Renderer>(true).ToList().FirstOrDefault(rend => rend.gameObject.tag == tag);
            if (renderer == null)
            {
                Debug.LogError(String.Concat("No ", tag, " found for: ", objectToCheck.name));
            }

            var result = renderer != null
                ? renderer.bounds.size
                : Vector3.one;

            // Rotate back;
            objectToCheck.transform.rotation = oldRotation;

            return result;
        }

        /// <summary>
        /// Waits for an amount of frames.
        /// </summary>
        /// <param name="frameCount">Amount of frames to be waited.</param>
        /// <returns></returns>
        public IEnumerator WaitForFrames(int frameCount)
        {
            if (frameCount <= 0)
            {
                throw new ArgumentOutOfRangeException("frameCount", "Cannot wait for less that 1 frame");
            }

            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }

        /// <summary>
        /// Returns 
        /// </summary>
        /// <returns></returns>
        public Vector3 GetTerrainMousePosition()
        {
            if (_terrainCollider == null)
            {
                Debug.LogWarning("No Terrain Collider!");
                return Vector3.zero;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_terrainCollider.Raycast(ray, out hit, Mathf.Infinity))
            {
                return hit.point;
            }

            return Vector3.one;
        }

        public void AddEventTrigger(EventTrigger eventTrigger, UnityAction action, EventTriggerType triggerType)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action()); // you can capture and pass the event data to the listener

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.triggers.Add(entry);
        }

        /// <summary>
        /// Add event Trigger
        /// </summary>
        /// <param name="action"></param>
        /// <param name="triggerType"></param>
        public void AddEventTrigger(EventTrigger eventTrigger, UnityAction<BaseEventData> action, EventTriggerType triggerType)
        {
            // Create a nee TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
            trigger.AddListener((eventData) => action(eventData)); // you can capture and pass the event data to the listener

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry() { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            eventTrigger.triggers.Add(entry);
        }
    }
}
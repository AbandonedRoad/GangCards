using System;
using Singleton;
using UnityEngine;
using Interfaces;

namespace Assets.Script.Actions
{
    public class ActionBase : IAction
    {
        public string ButtonText { get { return ResourceSingleton.Instance.CreateActionText(string.Empty, this.GetType().Name.ToString()); } }

        /// <summary>
        /// Cleans this action up.
        /// </summary>
        public virtual void CleanUp()
        {
            PrefabSingleton.Instance.RegularUpdate.RemoveListenener(this);
        }

        public virtual void ExecuteAction()
        { }

        /// <summary>
        /// Close this shit.
        /// </summary>
        public void Leave()
        {
            PrefabSingleton.Instance.ActionAfterEffactsHandler.SwitchAfterEffectsPanel(false);
        }

        /// <summary>
        /// Checks if an action succeds. Basic implementation. May be overriden.
        /// </summary>
        /// <param name="buildingLevel"></param>
        /// <returns>TRUE if success!</returns>
        public virtual bool ActionSucceeds(int buildingLevel)
        {
            var factor = buildingLevel - CharacterSingleton.Instance.GangLevel;
            var chance = 0;
            if (factor < -3)
            {
                chance = 100;
            }
            else if (factor < -2)
            {
                chance = 75;
            }
            else if (factor == 0)
            {
                chance = 50;
            }
            else if (factor == 1)
            {
                chance = 25;
            }
            Debug.Log("Your chance at " + this.GetType().ToString() + ": " + chance.ToString());

            float value = UnityEngine.Random.Range(0f, 100f);
            return (value <= chance);
        }
    }
}

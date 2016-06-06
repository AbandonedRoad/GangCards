using Enum;
using Singleton;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class UseCar : ActionBase
    {
        /// <summary>
        /// Executes current Actionm
        /// </summary>
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            PrefabSingleton.Instance.RegularUpdate.AddListenener(this, () => MoveCar());
        }

        /// <summary>
        /// Moves the car.
        /// </summary>
        private void MoveCar()
        {
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                PrefabSingleton.Instance.PlayersCarScript.Speed = 0.2f;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                PrefabSingleton.Instance.PlayersCarScript.Speed = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                PrefabSingleton.Instance.PlayersCarScript.TranslateNewDirection(Directions.Left);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                PrefabSingleton.Instance.PlayersCarScript.TranslateNewDirection(Directions.Right);
            }
        }
    }
}
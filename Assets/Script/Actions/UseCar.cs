using Enum;
using Singleton;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class UseCar : ActionBase
    {
        private MoveCars _playersCar;

        /// <summary>
        /// Creates new instance
        /// </summary>
        public UseCar()
        {
            _playersCar = HelperSingleton.Instance.SelectedObject.GetComponent<MoveCars>();
        }

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
                _playersCar.Speed = 0.2f;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                _playersCar.Speed = 0f;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                _playersCar.TranslateNewDirection(Directions.Left);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                _playersCar.TranslateNewDirection(Directions.Right);
            }
        }
    }
}
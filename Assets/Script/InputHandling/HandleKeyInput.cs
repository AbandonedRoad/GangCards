using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InputHandling
{
    public class HandleKeyInput : MonoBehaviour
    {
        public void Start()
        {

        }

        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    PrefabSingleton.Instance.DebugHandler.SwitchDebugPanel();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PrefabSingleton.Instance.OptionsHandler.SwitchOptionsPanel();
            }
        }
    }
}

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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PrefabSingleton.Instance.OptionsHandler.SwitchOptionsPanel();
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Singleton
{
    public class BuildingSingleton
    {
        private static BuildingSingleton _instance;

        /// <summary>
        /// Gets instance
        /// </summary>
        public static BuildingSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BuildingSingleton();
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
            
        }

        /// <summary>
        /// Shows the panel, if needed.
        /// </summary>
        /// <param name="show"></param>
        public void ShowBuildingActionPanel(bool show)
        {
            PrefabSingleton.Instance.BuildingActionsHandler.SwitchBuildingActionsPanel(show);
        }
    }
}
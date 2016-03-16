using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Linq;
using Menu;

namespace Singleton
{
    public class SettingsSingleton
    {
        private static SettingsSingleton _instance;

        public Language Language { private set; get; }

        /// <summary>
        /// Gets instance
        /// </summary>
        public static SettingsSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SettingsSingleton();
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
        }
    }
}
using UnityEngine.UI;
using System.Linq;

namespace Singleton
{
    public class DebugSingleton
    {
        private static DebugSingleton _instance;

        /// <summary>
        /// Gets instance
        /// </summary>
        public static DebugSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DebugSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Returns if the Debug Mode is on!
        /// </summary>
        public bool IsEnabled
        {
            get { return PrefabSingleton.Instance.DebugHandler.DebugPanel.activeSelf; }
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        private void Init()
        {
            if (!IsEnabled)
            {
                return;
            }
        }

        /// <summary>
        /// Updates all Debug Data
        /// </summary>
        public void UpdateDebugDate()
        {
            if (!IsEnabled)
            {
                return;
            }
        }
    }
}
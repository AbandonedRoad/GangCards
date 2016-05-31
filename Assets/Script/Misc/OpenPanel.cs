using Debugor;
using Singleton;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System;

namespace Assets.Script.Misc
{
    public class OpenPanel : MonoBehaviour, IDebug
    {
        public string Handler;
        public string Method;

        private MethodInfo info;
        private object _instance;

        /// <summary>
        /// Startup
        /// </summary>
        public void Start()
        {
            var props = PrefabSingleton.Instance.GetType().GetProperties();
            var handler = props.FirstOrDefault(prop => prop.Name == Handler);

            if (handler == null)
            {
                Debug.LogError("The Handler " + Handler + " does not exit!");
                return;
            }

            _instance = handler.GetValue(PrefabSingleton.Instance, null);
            var methods = handler.PropertyType.GetMethods();
            info = methods.FirstOrDefault(met => met.Name == Method);

            if (info == null)
            {
                Debug.LogError("The Method " + Method + " does not exit!");
            }
        }

        /// <summary>
        /// Fire if exist!
        /// </summary>
        /// <param name="other"></param>
        public void OnTriggerEnter(Collider other)
        {
            info.Invoke(_instance, null);
        }

        public void Execute()
        {
            info.Invoke(_instance, null);
        }
    }
}

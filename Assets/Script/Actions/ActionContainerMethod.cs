using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Actions
{
    public class ActionContainerMethod
    {
        public string TextRessourcePrefix;
        public string[] Parameters;

        public IAction Instance { get; set; }
        public bool[] MethodButtonsActive { get; private set; }
        public string[] MethodCalls { get; private set; }
        public string[] MethodTexts { get; private set; }

        /// <summary>
        /// Sets method calls into this action container.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="methodCalls"></param>
        public void SetMethods(IAction instance, string[] methodCalls, string[] methodTexts)
        {
            Instance = instance;
            MethodCalls = methodCalls;
            MethodTexts = methodTexts;
            MethodButtonsActive = new bool[methodCalls.Length];

            for (int i = 0; i < MethodButtonsActive.Length; i++)
            {
                MethodButtonsActive[i] = true;
            }
        }
    }
}

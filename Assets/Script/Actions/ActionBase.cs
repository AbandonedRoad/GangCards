using System;
using Singleton;

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
    }
}

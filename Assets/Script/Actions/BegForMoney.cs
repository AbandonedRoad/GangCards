using Actions;
using Enum;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class BegForMoney : ActionBase, ITextPresenter
    {
        private int _beggingLevel;
        private string _textPart1;
        private string _textPart2;

        public ActionContainerMethod ActionContainer { get; private set; }
        public string TextPart1 { get { return _textPart1; } }
        public string TextPart2 { get { return _textPart2; } }

        public BegForMoney(string beggingLevel)
        {
            if (!int.TryParse(beggingLevel, out _beggingLevel))
            {
                Debug.LogError("Parse failed! Values are not integer!");
            }
        }

        /// <summary>
        /// Executes this action
        /// </summary>
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            ActionContainer = new ActionContainerMethod();
            ActionContainer.SetMethods(this, new string[] { "Leave" }, new string[] { ResourceSingleton.Instance.GetSpecialText(SpecialText.Close)});
            ActionContainer.Parameters = new string[0];

            PrefabSingleton.Instance.ActionAfterEffactsHandler.PassActions(ActionContainer);

            EvaluateBegging();
        }

        /// <summary>
        /// Checks if begging succeeds.
        /// </summary>
        private void EvaluateBegging()
        {
            var greatSuccess = ActionSucceeds(_beggingLevel);

            _textPart1 = ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part1");

            _textPart2 = greatSuccess
                ? ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part2")
                : ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part2Fail");

            if (greatSuccess)
            {
                var moneyEarned = Math.Round(UnityEngine.Random.Range(1f, 10f), 2);
                CharacterSingleton.Instance.AvailableMoney += (float)moneyEarned;
                _textPart2 += String.Concat(moneyEarned.ToString(), "$");
            }
            else
            {
                var damageEarned = UnityEngine.Random.Range(1, 4);
                _textPart2 += damageEarned.ToString();
            }
        }
    }
}

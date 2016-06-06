using Actions;
using Enum;
using Singleton;
using System;
using System.Collections;
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
        private IEnumerator EvaluateBegging()
        {
            var dicingResult = ActionSucceeds(_beggingLevel);

            _textPart1 = ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part1");

            yield return new WaitForSeconds(0.75f);

            _textPart2 = (dicingResult == DicingResult.Success || dicingResult == DicingResult.GreatSuccess)
                ? ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part2")
                : ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part2Fail");

            if (dicingResult == DicingResult.Success || dicingResult == DicingResult.GreatSuccess)
            {
                var moneyEarned = dicingResult == DicingResult.Success
                    ? Math.Round(UnityEngine.Random.Range(1f, 5), 2)
                    : Math.Round(UnityEngine.Random.Range(5f, 10f), 2);
                CharacterSingleton.Instance.AvailableMoney += (float)moneyEarned;
                _textPart2 += String.Concat(moneyEarned.ToString(), "$");
            }
            else
            {
                var damageEarned = dicingResult == DicingResult.Failure
                    ? UnityEngine.Random.Range(1, 3)
                    : UnityEngine.Random.Range(3, 6);
                _textPart2 += damageEarned.ToString();
            }
        }
    }
}

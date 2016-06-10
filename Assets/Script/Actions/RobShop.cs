using Actions;
using Enum;
using Interfaces;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script.Actions
{
    public class RobShop : ActionBase, ITextPresenter
    {
        private int _shopLevel;
        private string _textPart1;
        private string _textPart2;
        private List<IGangMember> _opponents = new List<IGangMember>();


        public ActionContainerMethod ActionContainer { get; private set; }
        public string TextPart1 { get { return _textPart1; } }
        public string TextPart2 { get { return _textPart2; } }

        /// <summary>
        /// Creates instnace
        /// </summary>
        /// <param name="shopLevel"></param>
        public RobShop(string shopLevel)
        {
            if (!int.TryParse(shopLevel, out _shopLevel))
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
            ActionContainer.SetMethods(this, new string[] { "Leave" }, new string[] { ResourceSingleton.Instance.GetSpecialText(SpecialText.Close) });
            ActionContainer.Parameters = new string[0];

            PrefabSingleton.Instance.ActionAfterEffactsHandler.PassActions(ActionContainer);

            EvaluateRobbing();
        }

        /// <summary>
        /// Will be fired once the screen closes
        /// </summary>
        public override void ExecuteAfterClose()
        {
            base.ExecuteAfterClose();

            if (_opponents.Any())
            {
                PrefabSingleton.Instance.FightingHandler.StartFight(_opponents, new Func<bool, bool>(FightIsOver));
            }
        }

        /// <summary>
        /// Checks if robbing is successful
        /// </summary>
        private void EvaluateRobbing()
        {
            var dicingResult = ActionSucceeds(_shopLevel);

            _textPart1 = ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part1");

            _textPart2 = (dicingResult == DicingResult.Success || dicingResult == DicingResult.GreatSuccess)
                ? ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part2")
                : ResourceSingleton.Instance.CreateActionText(this.GetType().Name.ToString(), "Part2Fail");

            if (dicingResult == DicingResult.Success || dicingResult == DicingResult.GreatSuccess)
            {
                var moneyEarned = dicingResult == DicingResult.Success
                    ? Math.Round(UnityEngine.Random.Range(10f, 15), 2)
                    : Math.Round(UnityEngine.Random.Range(15f, 30f), 2);
                moneyEarned = moneyEarned * _shopLevel;
                CharacterSingleton.Instance.AvailableMoney += (float)moneyEarned;
                _textPart2 += String.Concat(moneyEarned.ToString(), "$");
            }
            else
            {
                int loops = dicingResult == DicingResult.Failure
                    ? 1
                    : UnityEngine.Random.Range(2, 4);
                for (int i = 0; i < loops; i++)
                {
                    // Up to 3 opponents.
                    var opponent = CharacterSingleton.Instance.GenerateAIPlayer(_shopLevel);
                    ItemSingleton.Instance.ReturnAppropiateWeapon(_shopLevel, opponent);
                    _opponents.Add(opponent);
                }
            }
        }

        /// <summary>
        /// Aftermath, after the fight is over.
        /// </summary>
        /// <param name="victory"></param>
        /// <returns></returns>
        private bool FightIsOver(bool victory)
        {
            if (victory)
            {

            }



            return true;
        }
    }
}

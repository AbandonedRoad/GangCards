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
    public class BuyWeapons : ActionBase, ITextPresenter
    {
        private int _shopLevel;
        private string _textPart1;
        private string _textPart2;

        public ActionContainerMethod ActionContainer { get; private set; }
        public string TextPart1 { get { return _textPart1; } }
        public string TextPart2 { get { return _textPart2; } }

        /// <summary>
        /// Creates instnace
        /// </summary>
        /// <param name="shopLevel"></param>
        public BuyWeapons(string shopLevel)
        {
            if (!int.TryParse(shopLevel, out _shopLevel))
            {
                Debug.LogError("Parse failed! Value are not integer!");
            }
        }

        /// <summary>
        /// Executes this action
        /// </summary>
        public override void ExecuteAction()
        {
            base.ExecuteAction();

            EvaluateItems();
        }

        /// <summary>
        /// Will be fired once the screen closes
        /// </summary>
        public override void ExecuteAfterClose()
        {
            base.ExecuteAfterClose();
        }

        /// <summary>
        /// Checks if robbing is successful
        /// </summary>
        private void EvaluateItems()
        {
            PrefabSingleton.Instance.ItemHandler.SelectItem(null, ItemSlot.NotSet, true, null);
        }
    }
}

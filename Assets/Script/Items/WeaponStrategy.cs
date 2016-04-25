using Enum;
using Interfaces;
using Items;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    public class WeaponStrategy : IItemStrategy
    {
        private readonly Weapon _parent;
        private bool _attackSuccessful;

        public WeaponStrategy(Weapon parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Determines if teh attack is successful.
        /// </summary>
        /// <returns></returns>
        public bool ExecuteAction()
        {
            var value = UnityEngine.Random.Range(1f, 10f);
            _attackSuccessful = value < 6;

            return _attackSuccessful;
        }

        /// <summary>
        /// Perpares the output
        /// </summary>
        /// <returns></returns>
        public IItemStrategyOutput GetOutpt(bool isPlayer)
        {

            var text = _attackSuccessful
                ? ResourceSingleton.Instance.GetText(isPlayer ? "FightActionSuccess" : "FightActionAISuccess")
                : ResourceSingleton.Instance.GetText(isPlayer ? "FightActionFail" : "FightActionAIFail");

            var damage = _attackSuccessful
                ? _parent.DamageRanges[ItemIdentifiers.Property1Val].GetDamage()
                : new KeyValuePair<DamageType, float>(DamageType.NotSet, 0);

            var rounded = Math.Round(damage.Value);
            text = text.Replace("@damage", rounded.ToString());

            return new WeaponStrategyOutput(text, damage.Key, damage.Value);
        }
    }
}

using CharacterBase;
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
    [Serializable]
    public class WeaponStrategy : IItemStrategy
    {
        private readonly Weapon _parent;
        private SkillDicingOutput _attackResult;

        /// <summary>
        /// CTZOR
        /// </summary>
        /// <param name="parent"></param>
        public WeaponStrategy(Weapon parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Determines if the attack is successful.
        /// </summary>
        /// <returns></returns>
        public bool ExecuteAction()
        {
            _attackResult = CharacterSingleton.Instance.CheckSkill(_parent.NeededSkill, _parent.AssignedTo);

            return _attackResult.Successful;
        }

        /// <summary>
        /// Perpares the output for the combast log
        /// </summary>
        /// <returns></returns>
        public IItemStrategyOutput GetAttackOutpt(bool isPlayer)
        {
            var text = String.Empty;
            var entryFound = _attackResult.Successful
                ? ResourceSingleton.Instance.GetText(isPlayer ? "FightActionSuccess" : "FightActionAISuccess", out text)
                : ResourceSingleton.Instance.GetText(isPlayer ? "FightActionFail" : "FightActionAIFail", out text);
            text = text.Replace("@diceResult", _attackResult.GetOutput());

            var damage = _attackResult.Successful
                ? _parent.DamageRanges[ItemIdentifiers.Property1Val].GetDamage()
                : new KeyValuePair<DamageType, int>(DamageType.NotSet, 0);

            text = text.Replace("@damage", damage.Value.ToString());

            return new WeaponStrategyOutput(text, damage.Key, damage.Value);
        }

        /// <summary>
        /// Returns which kind and which amount of damage this weapon does.
        /// </summary>
        /// <returns></returns>
        public string GetDamageOutput()
        {
            var output = String.Empty;
            foreach (var range in _parent.DamageRanges)
            {
                output += String.IsNullOrEmpty(output) ? "Dmg: " : ", ";
                output += range.Value.CreateDisplayValue();
            }

            return output;
        }
    }
}

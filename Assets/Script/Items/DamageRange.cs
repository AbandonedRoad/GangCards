using Enum;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    public class DamageRange
    {
        public DamageType Type { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public DamageRange(DamageType type, int minValue, int maxValue)
        {
            Type = type;
            MinDamage = minValue;
            MaxDamage = maxValue;
        }

        /// <summary>
        /// Returns the actual dmage
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<DamageType, int> GetDamage()
        {
            var damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
            return new KeyValuePair<DamageType, int>(Type, damage);
        }

        /// <summary>
        /// Creates the display value
        /// </summary>
        /// <returns></returns>
        public string CreateDisplayType()
        {
            var value1 = String.Empty;
            var value2 = String.Empty;

            ResourceSingleton.Instance.GetText(String.Concat("Fight", Type.ToString()), out value1);
            ResourceSingleton.Instance.GetText("FightDamage", out value2);

            return String.Concat(value1, " (", value2, ")");
        }

        /// <summary>
        /// Creates the display value
        /// </summary>
        /// <returns></returns>
        public string CreateDisplayValue()
        {
            return String.Concat(MinDamage.ToString(), "-", MaxDamage.ToString());
        }
    }
}

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
        public PropertyType Type { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public DamageRange(PropertyType type, int minValue, int maxValue)
        {
            Type = type;
            MinDamage = minValue;
            MaxDamage = maxValue;
        }

        /// <summary>
        /// Returns the actual dmage
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<PropertyType, int> GetDamage()
        {
            var damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
            return new KeyValuePair<PropertyType, int>(Type, damage);
        }

        /// <summary>
        /// Creates the display value
        /// </summary>
        /// <returns></returns>
        public string CreateDisplayType()
        {
            return String.Concat(ResourceSingleton.Instance.GetText(String.Concat("Fight", Type.ToString())), " (", ResourceSingleton.Instance.GetText("FightDamage"), ")");
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

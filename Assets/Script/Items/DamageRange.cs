using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Items
{
    public class DamageRange
    {
        public DamageType Type { get; private set; }
        public float MinDamage { get; private set; }
        public float MaxDamage { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="type"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public DamageRange(DamageType type, float minValue, float maxValue)
        {
            Type = type;
            MinDamage = minValue;
            MaxDamage = maxValue;
        }

        /// <summary>
        /// Returns the actual dmage
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<DamageType, float> GetDamage()
        {
            var damage = UnityEngine.Random.Range(MinDamage, MaxDamage);
            return new KeyValuePair<DamageType, float>(Type, damage);
        }

        /// <summary>
        /// Creates the display value
        /// </summary>
        /// <returns></returns>
        public string CreateDisplayType()
        {
            string damgeType = String.Empty;
            switch (Type)
            {
                case DamageType.CutDamage:
                    damgeType = "Cutting Damage";
                    break;
                default:
                    break;
            }

            return damgeType;
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

using Enum;
using Singleton;
using System;
using System.Collections.Generic;

namespace Items
{
    public class DamageRange
    {
        public DamageType Type { get; private set; }
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }

        /// <summary>
        /// Creates a new instance with the given values.
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
        /// Creates a new instance but calculates values depending on the entries.
        /// </summary>
        /// <param name="damageType"></param>
        /// <param name="weaponType"></param>
        /// <param name="level"></param>
        public DamageRange(DamageType damageType, WeaponType weaponType, int level)
        {
            int fromDamage = 0;
            int toDamage = 0;
            switch (weaponType)
            {
                case WeaponType.Blade:
                    fromDamage = 2;
                    toDamage = 5;
                    break;
                case WeaponType.Pistol:
                    fromDamage = 6;
                    toDamage = 10;
                    break;
                case WeaponType.Rifle:
                    fromDamage = 8;
                    toDamage = 14;
                    break;
                case WeaponType.Claws:
                    fromDamage = 3;
                    toDamage = 6;
                    break;
                case WeaponType.Bite:
                    fromDamage = 2;
                    toDamage = 5;
                    break;
                default:
                    break;
            }

            float factor = level * 0.75f;
            Type = damageType;
            MinDamage = level > 1 
                ? (int)Math.Round(fromDamage * factor)
                : fromDamage;
            MaxDamage = level > 1
                ? (int)Math.Round(toDamage * factor)
                : toDamage;
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

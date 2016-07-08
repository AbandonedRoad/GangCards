using System;
using UnityEngine;

namespace Enum
{
    public enum DamageType
    {
        CutDamage,
        BulletDamage,
        NotSet
    }

    public static partial class EnumExtensions
    {
        /// <summary>
        /// Get the damage type, which fits teh weapon type
        /// </summary>
        /// <param name="dmgType"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DamageType GetDamageTypeForWeaponType(this DamageType dmgType, WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Pistol:
                    return DamageType.BulletDamage;
                case WeaponType.Bite:
                    return DamageType.CutDamage;
                case WeaponType.Blade:
                    return DamageType.CutDamage;
                case WeaponType.Claws:
                    return DamageType.CutDamage;
                case WeaponType.Rifle:
                    return DamageType.BulletDamage;
                default:
                    Debug.LogError(type.ToString() + " is not supported in GetDamageTypeForWeaponType");
                    throw new ArgumentException();
            }
        }
    }
}

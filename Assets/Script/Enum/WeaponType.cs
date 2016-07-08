using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Enum
{
    public enum WeaponType
    {
        NotSet,
        Blade,
        Pistol,
        Rifle,
        Claws,
        Bite
    }

    public static partial class EnumExtensions
    {
        /// <summary>
        /// Get matching skill for weapon type
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns></returns>
        public static Skills GetNeededSkillForWeaponType(this WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Bite:
                    return Skills.Thrusting;
                case WeaponType.Blade:
                    return Skills.Thrusting;
                case WeaponType.Claws:
                    return Skills.Thrusting;
                case WeaponType.Pistol:
                    return Skills.Pistols;
                case WeaponType.Rifle:
                    return Skills.HeavyMachineGuns;
                default:
                    Debug.LogError(weaponType.ToString() + " is not supported in GetNeededSkillForWeaponType");
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Returns matching item slot for weapon type.
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns></returns>
        public static ItemSlot GetMatchingItemSlot(this WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Blade:
                    return ItemSlot.Knife;
                case WeaponType.Pistol:
                    return ItemSlot.Pistol;
                case WeaponType.Rifle:
                    return ItemSlot.MainWeapon;
                case WeaponType.Bite:
                    return ItemSlot.MainWeapon;
                case WeaponType.Claws:
                    return ItemSlot.MainWeapon;
                default:
                    Debug.LogError(weaponType.ToString() + " is not supported in GetMatchingItemSlot");
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Returns needed action points for weapon type.
        /// </summary>
        /// <param name="weaponType"></param>
        /// <returns></returns>
        public static int GetActionCost(this WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.Blade:
                    return 5;
                case WeaponType.Pistol:
                    return 7;
                case WeaponType.Rifle:
                    return 9;
                case WeaponType.Bite:
                    return 5;
                case WeaponType.Claws:
                    return 5;
                default:
                    Debug.LogError(weaponType.ToString() + " is not supported in GetActionCost");
                    throw new ArgumentException();
            }
        }
    }
}

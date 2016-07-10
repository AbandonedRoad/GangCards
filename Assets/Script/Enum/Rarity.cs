using System;
using UnityEngine;

namespace Enum
{
    public enum Rarity
    {
        Normal,
        Rare,
        VeryRare,
        Unique
    }

    public static partial class EnumExtensions
    {
        /// <summary>
        /// Returns a Rarity for an imte.
        /// </summary>
        /// <param name="rarity"></param>
        /// <returns></returns>
        public static Rarity GetRandomRarity(this Rarity rarity)
        {
            switch (UnityEngine.Random.Range(1, 4))
            {
                case 1:
                    return Rarity.Normal;
                case 2:
                    return Rarity.Rare;
                case 3:
                    return Rarity.VeryRare;
                default:
                    Debug.LogError("This is not supported in GetRandomRarity");
                    throw new ArgumentException();
            }
        }
    }
}
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
        public static Rarity GetRandomRarity(this Rarity rarity)
        {
            UnityEngine.Random.seed = DateTime.Now.Millisecond;
            int rand = UnityEngine.Random.Range(1, 4);

            switch (rand)
            {
                case 1:
                    return Rarity.Normal;
                case 2:
                    return Rarity.Rare;
                case 3:
                    return Rarity.VeryRare;
                default:
                    Debug.LogError(rand.ToString() + " is not supported in GetRandomRarity");
                    throw new ArgumentException();
            }
        }
    }
}
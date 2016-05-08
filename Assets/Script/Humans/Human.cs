using Interfaces;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Assets.Script.Characters
{
    public class Human : CharacterBase, IGangMember
    {
        /// <summary>
        /// Creates new gang member
        /// </summary>
        /// <param name="desiredLevel"></param>
        public Human(int desiredLevel)
            : base()
        {
            StreetName = new List<string>();
            CreateHuman(desiredLevel);
        }

        /// <summary>
        /// Creates a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        private void CreateHuman(int desiredLevel)
        {
            Strength = 8 + Random.Range(0, desiredLevel + 1);
            Intelligence = 8 + Random.Range(0, desiredLevel + 1);
            Initiative = 8 + Random.Range(0, desiredLevel + 1);
            Accuracy = 8 + Random.Range(0, desiredLevel + 1);
            Courage = 8 + Random.Range(0, desiredLevel + 1);
            ActionPoints = 8 + (Initiative + desiredLevel);
            MaxActionPoints = ActionPoints;

            Level = desiredLevel;

            var factor = (Level > 1 ? Level * 0.75f * Level : 1);
            MaxHealth = (int)Math.Round(25 + (Strength * factor));
            Health = MaxHealth;
        }
    }
}
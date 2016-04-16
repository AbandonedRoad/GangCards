using Interfaces;
using System.Collections.Generic;
using UnityEngine;

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
            Strength = Random.Range(1, desiredLevel * 3);
            Intelligence = Random.Range(1, desiredLevel * 3);
            Level = desiredLevel;
        }
    }
}
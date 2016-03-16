using Assets.Script.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Characters
{
    public class GangMember : CharacterBase, IAICharacter
    {
        /// <summary>
        /// Creates new gang member
        /// </summary>
        /// <param name="desiredLevel"></param>
        public GangMember(int desiredLevel)
        {
            StreetName = new List<string>();

            CreateAIPlayer(desiredLevel);
        }

        /// <summary>
        /// Creates a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        private void CreateAIPlayer(int desiredLevel)
        {
            Name = "Sam Schmidt";
            StreetName.Add("Das Messer");
            ActiveStreeName = "Das Messer";
            Strength = Random.Range(1, desiredLevel * 3);
            Intelligence = Random.Range(1, desiredLevel * 3);
            Level = desiredLevel;
        }
    }
}
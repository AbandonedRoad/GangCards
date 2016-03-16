using Assets.Script.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Singleton
{
    public class CharacterSingleton
    {
        private static CharacterSingleton _instance;

        public List<GangMember> AiPlayers { get; private set; }

        /// <summary>
        /// Gets instance
        /// </summary>
        public static CharacterSingleton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CharacterSingleton();
                    _instance.Init();
                }

                return _instance;
            }
        }

        /// <summary>
        /// Init this instance
        /// </summary>
        private void Init()
        {
            AiPlayers = new List<GangMember>();
        }

        /// <summary>
        /// Generates a new AI Player.
        /// </summary>
        /// <param name="desiredLevel"></param>
        /// <returns></returns>
        public GangMember GenerateAIPlayer(int desiredLevel)
        {
            return new GangMember(desiredLevel);
        }

        /// <summary>
        /// Adds a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        public void AddAIPlayer(GangMember newMember)
        {
            AiPlayers.Add(newMember);
        }
    }
}

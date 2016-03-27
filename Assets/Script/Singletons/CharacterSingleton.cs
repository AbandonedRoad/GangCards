using Assets.Script.Characters;
using Characters;
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
        private NameCreator _nameCreator = new NameCreator();

        public List<GangMember> PlayersGang { get; private set; }
        public string GangName { get; set; }
        public int GangLevel { get; set; }

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
            PlayersGang = new List<GangMember>();
            GangName = "Crumps";
            GangLevel = 1;
        }

        /// <summary>
        /// Generates a new AI Player.
        /// </summary>
        /// <param name="desiredLevel"></param>
        /// <returns></returns>
        public GangMember GenerateAIPlayer(int desiredLevel)
        {
            var member = new GangMember(desiredLevel);

            var name = _nameCreator.GenerateName(UnityEngine.Random.Range(0, 2) == 0);
            member.Name = String.Concat(name[0], " ", name[1]);

            return member;
        }

        /// <summary>
        /// Adds a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        public void AddAIPlayer(GangMember newMember)
        {
            PlayersGang.Add(newMember);
        }
    }
}

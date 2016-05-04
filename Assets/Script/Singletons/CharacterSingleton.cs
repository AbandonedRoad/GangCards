using Assets.Script.Characters;
using CharacterBase;
using Characters;
using Enum;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Singleton
{
    public class CharacterSingleton
    {
        private static CharacterSingleton _instance;
        private NameCreator _nameCreator = new NameCreator();

        public Gangs GangOfPlayer { get; set; }
        public float AvailableMoney { get; set; }
        public List<IGangMember> PlayersGang { get; private set; }
        public List<IGangMember> PlayerMembersInCar { get; private set; }
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
            PlayersGang = new List<IGangMember>();
            PlayerMembersInCar = new List<IGangMember>();
            GangName = "Crumps";
            GangOfPlayer = Gangs.Wheelers;
            GangLevel = 1;
            AvailableMoney = 0;
        }

        /// <summary>
        /// Generates a new AI Player.
        /// </summary>
        /// <param name="desiredLevel"></param>
        /// <returns></returns>
        public IGangMember GenerateAIPlayer(int desiredLevel)
        {
            var member = new Human(desiredLevel);

            var name = _nameCreator.GenerateName(UnityEngine.Random.Range(0, 2) == 0);
            member.Name = String.Concat(name[0], " ", name[1]);

            return member;
        }

        /// <summary>
        /// Adds a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        public void AddAIPlayer(IGangMember newMember)
        {
            PlayersGang.Add(newMember);
        }

        /// <summary>
        /// Checks if the skill 
        /// </summary>
        /// <param name="skillToCheck"></param>
        /// <param name="modifikator"></param>
        /// <returns></returns>
        public SkillDicingOutput CheckSkill(Skills skillToCheck, IGangMember memberToCheck, int modifikator = 0)
        {
            int value1 = 0;
            int value2 = 0;
            int value3 = 0;

            string[] skills = new string[1];
            switch (skillToCheck)
            {
                case Skills.Thrusting:
                    value1 = memberToCheck.Accuracy;
                    value2 = memberToCheck.Strength;
                    value3 = memberToCheck.Initiative;
                    skills = new string[] { "Accuracy", "Strength", "Initiative" };
                    break;
                case Skills.Pistols:
                    value1 = memberToCheck.Accuracy;
                    value2 = memberToCheck.Intelligence;
                    value3 = memberToCheck.Courage;
                    skills = new string[] { "Accuracy", "Intelligence", "Courage" };
                    break;
                case Skills.SubMachineGuns:
                    value1 = memberToCheck.Accuracy;
                    value2 = memberToCheck.Strength;
                    value3 = memberToCheck.Intelligence;
                    skills = new string[] { "Accuracy", "Strength", "Intelligence" };
                    break;
                case Skills.HeavyMachineGuns:
                    value1 = memberToCheck.Accuracy;
                    value2 = memberToCheck.Strength;
                    value3 = memberToCheck.Strength;
                    skills = new string[] { "Accuracy", "Strength", "Strength" };
                    break;
                case Skills.Explosives:
                    value1 = memberToCheck.Accuracy;
                    value2 = memberToCheck.Accuracy;
                    value3 = memberToCheck.Intelligence;
                    skills = new string[] { "Accuracy", "Accuracy", "Intelligence" };
                    break;
                default:
                    Debug.LogError("This skill is unknown!");
                    break;
            }

            int skill1 = Random.Range(1, 20);
            int skill2 = Random.Range(1, 20);
            int skill3 = Random.Range(1, 20);

            var total = value1 - skill1;
            total = total + value2 - skill2;
            total = total + value3 - skill3;

            return new SkillDicingOutput(skills, new int[] { value1, value2, value3 }, new int[] { skill1, skill2, skill3 }, total >= 0);
        }
    }
}

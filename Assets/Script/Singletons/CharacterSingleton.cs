using Assets.Script.Characters;
using CharacterBase;
using Characters;
using Enum;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public int GangLevel { get { return GetGangLevel(false); } }
        public int GangCarLevel { get { return GetGangLevel(true); } }

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
        /// Initialize
        /// </summary>
        private void Init()
        {
            PlayersGang = new List<IGangMember>();
            PlayerMembersInCar = new List<IGangMember>();
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

            var name = _nameCreator.GenerateName(Random.Range(0, 2) == 0);
            member.Name = String.Concat(name[0], " ", name[1]);

            return member;
        }

        /// <summary>
        /// Checks if the skill 
        /// </summary>
        /// <param name="skillToCheck"></param>
        /// <param name="modifikator"></param>
        /// <returns></returns>
        public SkillDicingOutput CheckSkill(Skills skillToCheck, IGangMember memberToCheck, int modifikator = 0)
        {
            int skill1 = 0;
            int skill2 = 0;
            int skill3 = 0;

            string[] skills = new string[1];
            switch (skillToCheck)
            {
                case Skills.Thrusting:
                    skill1 = memberToCheck.Accuracy;
                    skill2 = memberToCheck.Strength;
                    skill3 = memberToCheck.Initiative;
                    skills = new string[] { "Accuracy", "Strength", "Initiative" };
                    break;
                case Skills.Pistols:
                    skill1 = memberToCheck.Accuracy;
                    skill2 = memberToCheck.Intelligence;
                    skill3 = memberToCheck.Courage;
                    skills = new string[] { "Accuracy", "Intelligence", "Courage" };
                    break;
                case Skills.SubMachineGuns:
                    skill1 = memberToCheck.Accuracy;
                    skill2 = memberToCheck.Strength;
                    skill3 = memberToCheck.Intelligence;
                    skills = new string[] { "Accuracy", "Strength", "Intelligence" };
                    break;
                case Skills.HeavyMachineGuns:
                    skill1 = memberToCheck.Accuracy;
                    skill2 = memberToCheck.Strength;
                    skill3 = memberToCheck.Strength;
                    skills = new string[] { "Accuracy", "Strength", "Strength" };
                    break;
                case Skills.Explosives:
                    skill1 = memberToCheck.Accuracy;
                    skill2 = memberToCheck.Accuracy;
                    skill3 = memberToCheck.Intelligence;
                    skills = new string[] { "Accuracy", "Accuracy", "Intelligence" };
                    break;
                default:
                    Debug.LogError("This skill is unknown!");
                    break;
            }

            int diced1 = Random.Range(1, 20);
            int diced2 = Random.Range(1, 20);
            int diced3 = Random.Range(1, 20);

            var total1 = diced1 - skill1;
            var total2 = diced2 - skill2;
            var total3 = diced3 - skill3;

            var total = total1 + total2 + total3;

            return new SkillDicingOutput(skillToCheck, modifikator, skills, new int[] { skill1, skill2, skill3 }, new int[] { diced1, diced2, diced3 }, total >= 0);
        }

        /// <summary>
        /// Init this instance
        /// </summary>
        private int GetGangLevel(bool returnForCarOnly)
        {
            float average;
            if (returnForCarOnly)
            {
                var totalLevel = Instance.PlayerMembersInCar.Sum(gm => gm.Level);
                average = totalLevel / (Instance.PlayerMembersInCar.Any() ? Instance.PlayerMembersInCar.Count : 1);
            }
            else
            {
                var totalLevel = Instance.PlayersGang.Sum(gm => gm.Level);
                average = totalLevel / (Instance.PlayersGang.Any() ? Instance.PlayersGang.Count : 1);
            }

            return (int)Math.Round(average, 0);
        }
    }
}
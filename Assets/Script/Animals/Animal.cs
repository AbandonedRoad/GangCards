﻿using Enum;
using Interfaces;
using Items;
using Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Script.Characters
{
    public class Animal : CharacterBase, IGangMember
    {
        /// <summary>
        /// Creates new instance
        /// </summary>
        /// <param name="desiredLevel"></param>
        public Animal(int desiredLevel, bool isCpu, AnimalType type)
            : base()
        {
            StreetName = new List<string>();
            CreateAnimal(desiredLevel);

            Initialize(type);
        }

        /// <summary>
        /// Prepares an animal
        /// </summary>
        /// <param name="type"></param>
        private void Initialize(AnimalType type)
        {
            switch (type)
            {
                case AnimalType.Dog:
                    UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(413357241, Level);
                    Name = "Frenzy pitbull";
                    break;
                case AnimalType.Bear:
                    UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(413357241, Level);
                    Name = "Vicious Grizzly";
                    break;
                case AnimalType.Harpy:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        private void CreateAnimal(int desiredLevel)
        {
            Strength = Random.Range(1, desiredLevel * 3);
            Intelligence = 1;
            Level = desiredLevel;
            Random.Range(3, desiredLevel * 4);
        }
    }
}

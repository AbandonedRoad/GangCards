using Enum;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;

namespace Assets.Script.Characters
{
    public class Animal : CharacterBase, IGangMember
    {
        public IAnimalStrategy AnimalStragegy { get; private set; }

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
                    AnimalStragegy = new DogStrategy(this);
                    Name = "Frenzy pitbull";
                    break;
                case AnimalType.Bear:
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
        }
    }
}

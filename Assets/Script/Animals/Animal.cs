using Enum;
using Interfaces;
using Singleton;
using System;
using System.Collections.Generic;
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

            PostProcess(type);
        }

        /// <summary>
        /// Prepares an animal
        /// </summary>
        /// <param name="type"></param>
        private void PostProcess(AnimalType type)
        {
            switch (type)
            {
                case AnimalType.Dog:
                    UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(413357241, Level, this); // Claws
                    Name = "Frenzy pitbull";
                    MaxHealth = (int)Math.Round(10 * (Level > 1 ? Level * 0.5f : 1));
                    Intelligence = +Level;
                    break;
                case AnimalType.Bear:
                    UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(413357241, Level, this); // Claws
                    Name = "Vicious Grizzly";
                    MaxHealth = (int)Math.Round(20 * (Level > 1 ? Level * 0.5f : 1));
                    Strength =+ (int)Math.Round((1.5f * Level));
                    break;
                case AnimalType.Harpy:
                    UsedItems[ItemSlot.MainWeapon] = ItemSingleton.Instance.GetItem(413357241, Level, this); // Claws
                    Name = "Slaying Harpy";
                    MaxHealth = (int)Math.Round(20 * (Level > 1 ? Level * 0.5f : 1));
                    Initiative= +(int)Math.Round((1.5f * Level));
                    break;
                default:
                    break;
            }

            Health = MaxHealth;
        }

        /// <summary>
        /// Creates a new AI Player
        /// </summary>
        /// <param name="desiredLevel"></param>
        private void CreateAnimal(int desiredLevel)
        {
            Strength = 8 + Random.Range(0, desiredLevel + 1);
            Intelligence = 8 + Random.Range(0, desiredLevel + 1);
            Initiative = 8 + Random.Range(0, desiredLevel + 1);
            Accuracy = 8 + Random.Range(0, desiredLevel + 1);
            Courage = 8 + Random.Range(0, desiredLevel + 1);
            ActionPoints = 8 + (Initiative + desiredLevel);
            Level = desiredLevel;
        }
    }
}

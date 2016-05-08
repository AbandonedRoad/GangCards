using Enum;
using Interfaces;
using System.Collections.Generic;

namespace Assets.Script.Characters
{
    public class CharacterBase
    {
        private int _health;

        public string Name { get; set;}
        public string ActiveStreeName { get; set; }
        public List<string> StreetName { get; set; }

        public int Level { get; set; }
        public int Health
        {
            get { return _health; }
            set
            {
                _health = value;
                _health = _health < 0 ? 0 : _health;
                UpdateHealthStatus();
            }
        }
        public int MaxHealth { get; set; }
        public int Intelligence { get; set; }
        public int Strength { get; set; }
        public int Initiative { get; set; }
        public int Accuracy { get; set; }
        public int Courage { get; set; }
        public HealthStatus HealthStatus { get; private set; }
        public int ActionPoints { get; set; }
        public int MaxActionPoints { get; protected set; }

        public Gangs GangAssignment { get; set; }

        public Dictionary<ItemSlot, IItem> UsedItems { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public CharacterBase()
        {
            UsedItems = new Dictionary<ItemSlot, IItem>();

            foreach (ItemSlot item in System.Enum.GetValues(typeof(ItemSlot)))
            {
                if (item == ItemSlot.NotSet)
                {
                    continue;
                }

                UsedItems.Add(item, null);
            }
        }

        /// <summary>
        /// Post process initialisation
        /// </summary>
        /// <param name="desiredGang"></param>
        public virtual void PostProcessInit(Gangs desiredGang)
        {
            GangAssignment = desiredGang;
        }

        /// <summary>
        /// Updates the Health Status
        /// </summary>
        private void UpdateHealthStatus()
        {
            if (Health == MaxHealth)
            {
                HealthStatus = HealthStatus.Healthy;
            }
            else if (Health <= 0)
            {
                HealthStatus = HealthStatus.Dead;
            }
            else
            {
                var restHealth = (float)((float)Health / ((float)MaxHealth / 100));
                if (restHealth > 80)
                {
                    HealthStatus = HealthStatus.Wounded;
                }
                else if (restHealth > 60)
                {
                    HealthStatus = HealthStatus.Injured;
                }
                else if (restHealth > 30)
                {
                    HealthStatus = HealthStatus.Critical;
                }
                else if (restHealth <= 15)
                {
                    HealthStatus = HealthStatus.Unconscious;
                }
            }            
        }
    }
}

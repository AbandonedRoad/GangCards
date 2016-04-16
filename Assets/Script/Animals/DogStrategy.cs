using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Characters
{
    public class DogStrategy : IAnimalStrategy
    {
        private IGangMember _parent;

        /// <summary>
        /// Create new instance
        /// </summary>
        /// <param name="sender"></param>
        public DogStrategy(IGangMember sender)
        {
            _parent = sender;
        }

        /// <summary>
        /// Executes an attack
        /// </summary>
        /// <returns></returns>
        public float ExecuteAttack()
        {
            return UnityEngine.Random.Range(0, (_parent.Strength * _parent.Level));
        }
    }
}

using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weapons
{
    public class Knife : IWeaponStrategy
    {
        public float ExecuteAttack()
        {
            return 1f;
        }
    }
}
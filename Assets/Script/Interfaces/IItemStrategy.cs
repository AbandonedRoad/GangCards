﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    public interface IItemStrategy
    {
        bool ExecuteAction();

        IItemStrategyOutput GetAttackOutpt(bool isPlayer);

        string GetDamageOutput();
    }
}
